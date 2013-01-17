namespace ParlayRMS.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.ManageMember;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Services.Implement.Members;
    using ParlayRMS.Services.Interface.Members;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Services.Implement.Users;

    public class ManageMemberController : BaseController
    {
        public const string HIDDEN_GROUPS_KEY = "HiddenGroups";
        private readonly IUsersService userService = new UsersService();
        private readonly IMembersService customerService = new MembersService();
        private int pagesize = Utility.AppSetting.PageSize;

        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Read)]
        [ValidateInput(false)]
        public ActionResult SpecialMemberList()
        {
            int level = string.IsNullOrEmpty(Request["level"]) ? -1 : DbConverter.ToInt32(Request["level"]);
            string userName = DbConverter.ToString(Request["username"]);
            int page = string.IsNullOrEmpty(Request["page"]) || DbConverter.ToInt32(Request["page"]) < 1 ? 1 : DbConverter.ToInt32(Request["page"]);

            pagesize = string.IsNullOrEmpty(Request["pagesize"]) ? Utility.AppSetting.PageSize : DbConverter.ToInt32(Request["pagesize"]);

            var specialMemberList = this.customerService.GetListCustomer(userName, level);
            ViewBag.Pager = Pager.Items(specialMemberList.Count()).PerPage(pagesize).Move(page).Segment(5).Center();

            return this.View(specialMemberList.ToPagedList(page, pagesize));
        }

        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Read)]
        [ValidateInput(false)]
        public ActionResult Index()
        {
            return this.View();
        }

        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Create)]
        public ActionResult AddMember()
        {
            return this.PartialView(new AddMemberModel());
        }

        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Create)]
        [ValidateInput(false)]
        public ActionResult SaveMember(AddMemberModel model)
        {
            var result = this.customerService.AddCustomer(model.UserName, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(new { code = result.Code, message = result.Message });
        }

        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Update)]
        public ActionResult UpdateLevel(UpdateLevelModel model)
        {
            var result = this.customerService.UpdateLevelCustomer(model.custId, model.level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(new { code = result.Code, message = result.Message });
        }

        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Read)]
        [HttpGet]
        [ValidateInput(false)]
        public void ExportSpecialMembers()
        {
            int level = string.IsNullOrEmpty(Request["level"]) ? -1 : DbConverter.ToInt32(Request["level"]);
            string userName = Request["username"];

            var specialMemberList = this.customerService.GetListCustomer(userName, level);
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
            Response.AddHeader("content-disposition", "attachment; filename=" + "SpecialMember.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.MemberManagement, Policy = ePolicy.Update)]
        public ActionResult ChangeCustomerLevel()
        {
            int custId = string.IsNullOrEmpty(Request["custid"]) ? 0 : DbConverter.ToInt32(Request["custid"]);
            int level = string.IsNullOrEmpty(Request["level"]) ? -1 : DbConverter.ToInt32(Request["level"]);
            string userName = Request["username"];

            var result = this.customerService.ChangeLevelCustomer(custId, userName, level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(new { code = result.Code, message = result.Message });
        }
    }
}