namespace ParlayRMS.Controllers
{
    using PagedList;
    using Utility;
    using System.Linq;
    using System.Web.Mvc;
    using ExcelService.Writers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using Services;
    using ViewModel.ManageLeague;

    public class ManageLeagueController : BaseController
    {
        private readonly IUserService _userService = new UserService();
        private readonly LeaguesService leagueService = new LeaguesService();
        private readonly int _pageSize = AppSetting.PageSize;
        private const int SOCCER = 1;


        [NapAuth(FunctionKey = FunctionKeys.ManageLeague, Policy = Constant.Policy.Read)]
        public ActionResult LeagueList(LeagueListModel model)
        {
            var specialLeagueList = leagueService.GetListLeague(model.League, model.Group, SOCCER);
            var page = model.Page <= 0 ? 1 : model.Page;
            ViewBag.Pager = Pager.Items(specialLeagueList.Count).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return View(specialLeagueList.ToPagedList(page, _pageSize));
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageLeague, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageLeague, Policy = Constant.Policy.Host)]
        public ActionResult AddLeague()
        {
            var model = new AddLeagueModel();
            return PartialView(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageLeague, Policy = Constant.Policy.Host)]
        public ActionResult AddLeague(AddLeagueModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = leagueService.AddLeague(model.League, model.Group, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageLeague, Policy = Constant.Policy.Host)]
        public ActionResult UpdateGroup(UpdateGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = leagueService.UpdateLeague(model.League, model.Group, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageLeague, Policy = Constant.Policy.Read)]
        public ActionResult ExportExcel(ExportExcelModel model)
        {
            var specialLeagueList = leagueService.GetListLeague(model.League, model.Group, SOCCER);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\SpecialLeague.xls";
            var writer = new TemplateWriter(templatePath);

            const string title = "Special Leagues";
            writer.WriteRow(0, new object[] { title });

            var header = new[]
            {
                "No",
                "League Name",
                "Group"
            };
            writer.WriteRow(1, header);

            var select = specialLeagueList.Select(x => new
            {
                x.No,
                x.LeagueName,
                x.GroupName
            });
            writer.WriteRows(2, select);
            writer.SyncColumnWidths(3);

            var ms = writer.Render("SpecialLeague");
            ms.Seek(0, 0);
            return File(ms, "application/vnd.ms-excel", "SpecialLeague.xls");
        }

        public ActionResult SearchBar(SearchBarModel model)
        {
            return PartialView(model);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.HasWritePermission = CurrentSession.UserId > 0 && _userService.IsHavingPermission(
                CurrentSession.UserId, 
                NapSettings.Settings.SiteId,
                FunctionKeys.ManageLeague,
                Constant.Policy.Host);
        }
    }
}
