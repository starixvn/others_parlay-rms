namespace ParlayRMS.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Services.Implement.Members;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Members;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.ManageMember;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Models.Functions;
    using System.Net;

    public class ManageMemberController : BaseController
    {
        private readonly IUsersService userService = new UsersService();
        private readonly IMembersService customerService = new MembersService();
        private int pagesize = AppSetting.PageSize;

        // GET: ~/ManangeMember/MemberList
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Read)]
        public ActionResult MemberList(MemberListModel model)
        {
            var specialMemberList = this.customerService.GetListCustomer(model.UserName, model.Level);
            int page = model.Page == null ? 1 : model.Page.Value;
            ViewBag.Pager = Pager.Items(specialMemberList.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return this.View(specialMemberList.ToPagedList(page, pagesize));
        }

        // GET: ~/ManangeMember
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Read)]
        public ActionResult Index()
        {
            return this.View();
        }

        // GET: ~/ManangeMember/AddMember
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Create)]
        public ActionResult AddMember()
        {
            var model = new AddMemberModel();
            return this.PartialView(model);
        }

        // POST: ~/ManangeMember/AddMember
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Create)]
        public ActionResult AddMember(AddMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.customerService.AddCustomer(model.UserName, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // POST: ~/ManangeMember/UpdateLevel
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Update)]
        public ActionResult UpdateLevel(UpdateLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.customerService.UpdateLevelCustomer(model.CustId, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // POST: ~/ManangeMember/ExportExcel
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Read)]
        public ActionResult ExportExcel(ExportExcelModel model)
        {
            var specialMemberList = this.customerService.GetListCustomer(model.UserName, model.Level);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "ExcelTemplates\\SpecialMember.xls";
            var writer = new TemplateWriter(templatePath);

            string title = "Special Members";
            writer.WriteRow(0, new object[] { title });

            var header = new string[]{
                "No",
                ViewLang.Username,
                ViewLang.Level
            };
            writer.WriteRow(1, header);

            var select = specialMemberList.Select(x => new
            {
                No = x.No,
                userName = x.Username,
                level = x.Level == 0 ? ViewLang.None : ViewLang.Level + " " + x.Level,
            });
            writer.WriteRows(2, select);
            writer.SyncColumnWidths(3);

            MemoryStream ms = writer.Render("SpecialMember");
            ms.Seek(0, 0);
            return base.File(ms, "application/vnd.ms-excel", "SpecialMember.xls");
        }

        // POST: ~/ManangeMember/ChangeLevel
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Update)]
        public ActionResult ChangeLevel(ChangeLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.customerService.ChangeLevelCustomer(model.CustId, model.UserName, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        public ActionResult SearchBar(SearchBarModel model)
        {
            return this.PartialView(model);
        }
    }
}