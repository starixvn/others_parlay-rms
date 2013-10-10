// --------------------------------------------------------------------------
// <copyright file="ManageMemberController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.Members;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Members;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.ManageMember;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// Member Management Controller.
    /// </summary>
    public class ManageMemberController : BaseController
    {
        private readonly IUsersService _userService = new UsersService();
        private readonly IMembersService _customerService = new MembersService();
        private int _pageSize = AppSetting.PageSize;

        public ManageMemberController()
        {
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.MemberManagement);
        }

        /// <summary>
        /// Gets member list.
        /// </summary>
        /// <param name="model">Filter conditions.</param>
        /// <returns>List of members.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.MemberManagement, Policy = UserPolicy.Read)]
        public ActionResult MemberList(MemberListModel model)
        {
            var specialMemberList = this._customerService.GetListCustomer(model.UserName, model.Level);
            int page = model.Page == null ? 1 : model.Page.Value;
            ViewBag.Pager = Pager.Items(specialMemberList.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return this.View(specialMemberList.ToPagedList(page, _pageSize));
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.MemberManagement, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// GET method: redirects to Add New Member form.
        /// </summary>
        /// <returns>Partial view represents the Add New Member form.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.MemberManagement, Policy = UserPolicy.Create)]
        public ActionResult AddMember()
        {
            var model = new AddMemberModel();
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: adds a new member.
        /// </summary>
        /// <param name="model">The model contains the information of new member.</param>
        /// <returns>Whether the new member is added successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MemberManagement, Policy = UserPolicy.Create)]
        public ActionResult AddMember(AddMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._customerService.AddCustomer(model.UserName, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// POST method: updates the level of customer.
        /// </summary>
        /// <param name="model">New customer's level.</param>
        /// <returns>Whether the level of customer is updated successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MemberManagement, Policy = UserPolicy.Update)]
        public ActionResult UpdateLevel(UpdateLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._customerService.UpdateLevelCustomer(model.CustId, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// Exports the member list to Excel.
        /// </summary>
        /// <param name="model">Filter conditions.</param>
        /// <returns>File stream contains the Excel file.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.MemberManagement, Policy = UserPolicy.Read)]
        public ActionResult ExportExcel(ExportExcelModel model)
        {
            var specialMemberList = this._customerService.GetListCustomer(model.UserName, model.Level);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "ExcelTemplates\\SpecialMember.xls";
            var writer = new TemplateWriter(templatePath);

            string title = "Special Members";
            writer.WriteRow(0, new object[] { title });

            var header = new string[]
            {
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
            return this.File(ms, "application/vnd.ms-excel", "SpecialMember.xls");
        }

        /// <summary>
        /// Changes the level of current member.
        /// </summary>
        /// <param name="model">The model contains username and new level value.</param>
        /// <returns>Whether the level of member is updated successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MemberManagement, Policy = UserPolicy.Update)]
        public ActionResult ChangeLevel(ChangeLevelModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._customerService.ChangeLevelCustomer(model.CustId, model.UserName, model.Level, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// Gets partial view of Search bar.
        /// </summary>
        /// <param name="model">Model that is used to bind with the search bar.</param>
        /// <returns>Partial view of Search bar.</returns>
        public ActionResult SearchBar(SearchBarModel model)
        {
            return this.PartialView(model);
        }
    }
}