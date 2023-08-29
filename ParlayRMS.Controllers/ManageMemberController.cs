namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using ExcelService.Writers;
    using PagedList;
    using Models.Functions;
    using Services.Members;
    using Services.Users;
    using Utility;
    using ViewModel.ManageMember;
    using Web.Authorization;

    public class ManageMemberController : BaseController
    {
        private readonly UsersService _userService = new UsersService();
        private readonly MembersService _customerService = new MembersService();
        private readonly int _pageSize = AppSetting.PageSize;

        public ManageMemberController()
        {
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageCustomer);
        }

        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Read)]
        public ActionResult MemberList(MemberListModel model)
        {
            var specialMemberList = _customerService.GetListCustomer(model.UserName, model.Level);
            var page = model.Page == null ? 1 : model.Page.Value;
            ViewBag.Pager = Pager.Items(specialMemberList.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return View(specialMemberList.ToPagedList(page, _pageSize));
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Create)]
        public ActionResult AddMember()
        {
            var model = new AddMemberModel();
            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Create)]
        public ActionResult AddMember(AddMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _customerService.AddCustomer(model.UserName, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Update)]
        public ActionResult UpdateLevel(UpdateLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _customerService.UpdateLevelCustomer(model.CustId, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Read)]
        public ActionResult ExportExcel(ExportExcelModel model)
        {
            var specialMemberList = _customerService.GetListCustomer(model.UserName, model.Level);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\SpecialMember.xls";
            var writer = new TemplateWriter(templatePath);

            var title = "Special Members";
            writer.WriteRow(0, new object[] { title });

            var header = new[]
            {
                "No",
                ViewLang.Site,
                ViewLang.Username,
                ViewLang.Level
            };
            writer.WriteRow(1, header);

            var select = specialMemberList.Select(x => new
            {
                x.No,
                site = x.Site,
                userName = x.Username,
                level = x.Level == 0 ? ViewLang.None : ViewLang.Level + " " + x.Level,
            });
            writer.WriteRows(2, select);
            writer.SyncColumnWidths(4);

            var ms = writer.Render("SpecialMember");
            ms.Seek(0, 0);
            return File(ms, "application/vnd.ms-excel", "SpecialMember.xls");
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Update)]
        public ActionResult ChangeLevel(ChangeLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _customerService.ChangeLevelCustomer(model.CustId, model.UserName, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return Json(result);
        }

        public ActionResult SearchBar(SearchBarModel model)
        {
            return PartialView(model);
        }
    }
}
