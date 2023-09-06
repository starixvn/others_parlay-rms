namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using ExcelService.Writers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using PagedList;
    using Services.Members;
    using Utility;
    using ViewModel.ManageMember;

    public class ManageMemberController : BaseController
    {
        private readonly IUserService _userService = new UserService();
        private readonly MembersService _customerService = new MembersService();
        private readonly int _pageSize = AppSetting.PageSize;

        public ManageMemberController()
        {
            ViewBag.HasWritePermission = _userService.IsHavingPermission(
                CurrentSession.UserId, 
                NapSettings.Settings.SiteId,
                FunctionKeys.ManageMember,
                Constant.Policy.Host);
        }

        [NapAuth(FunctionKey = FunctionKeys.ManageMember, Policy = Constant.Policy.Read)]
        public ActionResult MemberList(MemberListModel model)
        {
            var specialMemberList = _customerService.GetListCustomer(model.UserName, model.Level);
            var page = model.Page ?? 1;
            ViewBag.Pager = Pager.Items(specialMemberList.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return View(specialMemberList.ToPagedList(page, _pageSize));
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMember, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMember, Policy = Constant.Policy.Host)]
        public ActionResult AddMember()
        {
            var model = new AddMemberModel();
            return PartialView(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMember, Policy = Constant.Policy.Host)]
        public ActionResult AddMember(AddMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _customerService.AddCustomer(model.UserName, model.Level, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMember, Policy = Constant.Policy.Host)]
        public ActionResult UpdateLevel(UpdateLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _customerService.UpdateLevelCustomer(model.CustId, model.Level, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMember, Policy = Constant.Policy.Read)]
        public ActionResult ExportExcel(ExportExcelModel model)
        {
            var specialMemberList = _customerService.GetListCustomer(model.UserName, model.Level);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\SpecialMember.xls";
            var writer = new TemplateWriter(templatePath);

            const string title = "Special Members";
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
                level = x.Level == 0 ? ViewLang.None : ViewLang.Level + " " + x.Level
            });
            writer.WriteRows(2, select);
            writer.SyncColumnWidths(4);

            var ms = writer.Render("SpecialMember");
            ms.Seek(0, 0);
            return File(ms, "application/vnd.ms-excel", "SpecialMember.xls");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMember, Policy = Constant.Policy.Host)]
        public ActionResult ChangeLevel(ChangeLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _customerService.ChangeLevelCustomer(model.CustId, model.UserName, model.Level, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }

        public ActionResult SearchBar(SearchBarModel model)
        {
            return PartialView(model);
        }
    }
}
