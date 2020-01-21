namespace ParlayRMS.Controllers
{
    using ExcelService.Writers;
    using PagedList;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.Leagues;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.Web.Authorization;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModel.ManageLeague;

    /// <summary>
    /// League Management Controller.
    /// </summary>
    public class ManageLeagueController : BaseController
    {
        private readonly UsersService _userService = new UsersService();
        private readonly LeaguesService leagueService = new LeaguesService();
        private readonly int _pageSize = AppSetting.PageSize;


        [AuthorizationAttribute(Function = FunctionKeys.ManageLeague, Policy = UserPolicy.Read)]
        public ActionResult LeagueList(LeagueListModel model)
        {
            var specialLeagueList = this.leagueService.GetListLeague(model.League, model.Group);
            int page = model.Page <= 0 ? 1 : model.Page;
            ViewBag.Pager = Pager.Items(specialLeagueList.Count).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return this.View(specialLeagueList.ToPagedList(page, _pageSize));
        }

        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageLeague, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageLeague, Policy = UserPolicy.Create)]
        public ActionResult AddLeague()
        {
            var model = new AddLeagueModel();
            return this.PartialView(model);
        }

        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageLeague, Policy = UserPolicy.Create)]
        public ActionResult AddLeague(AddLeagueModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.leagueService.AddLeague(model.League, model.Group, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageLeague, Policy = UserPolicy.Update)]
        public ActionResult UpdateGroup(UpdateGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.leagueService.UpdateLeague(model.League, model.Group, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageLeague, Policy = UserPolicy.Read)]
        public ActionResult ExportExcel(ExportExcelModel model)
        {
            var specialLeagueList = this.leagueService.GetListLeague(model.League, model.Group);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\SpecialLeague.xls";
            var writer = new TemplateWriter(templatePath);

            string title = "Special Leagues";
            writer.WriteRow(0, new object[] { title });

            var header = new string[]
            {
                "No",
                "League Name",
                "Group"
            };
            writer.WriteRow(1, header);

            var select = specialLeagueList.Select(x => new
            {
                No = x.No,
                LeagueName = x.LeagueName,
                GroupName = x.GroupName,
            });
            writer.WriteRows(2, select);
            writer.SyncColumnWidths(3);

            MemoryStream ms = writer.Render("SpecialLeague");
            ms.Seek(0, 0);
            return this.File(ms, "application/vnd.ms-excel", "SpecialLeague.xls");
        }

        public ActionResult SearchBar(SearchBarModel model)
        {
            return this.PartialView(model);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.HasWritePermission = CurrentSession.UserId > 0 && _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageLeague);
        }
    }
}