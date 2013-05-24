// --------------------------------------------------------------------------
// <copyright file="ManageUserController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.5.23">Format codes</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Implement.Groups;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Groups;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.ViewModel.ManageUser;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Models.Functions;

    /// <summary>
    /// Manages user methods
    /// </summary>
    public class ManageUserController : BaseController
    {
        private readonly IUsersService userService = new UsersService();
        private readonly IGroupsService groupService = new GroupsService();

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageUserController"/> class
        /// </summary>
        public ManageUserController()
        {
            var groups = this.groupService.GetGroupNames();
            var selectedItems = new List<SelectListItem>();
            string[] hiddenGroupNameList = new string[0];

            if (!this.HideGroup(out hiddenGroupNameList))
            {
                groups = groups.Where(x => hiddenGroupNameList.Contains(x.Name) == false);
            }
            foreach (var item in groups)
            {
                var selectedItem = new SelectListItem();

                selectedItem.Text = item.Name;
                selectedItem.Value = item.Id.ToString();

                selectedItems.Add(selectedItem);
            }

            ViewBag.GroupList = selectedItems;
            ViewBag.HasWritePermission = userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageUser);
        }

        // GET: ~/ManangeUser
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Read)]
        public ActionResult Index(IndexModel model)
        {
            ViewBag.GroupList = this.GetAllGroup();

            ViewBag.UserList = new List<User>();
            CurrentContext.Context.Session["groupId"] = model.GroupId;

            var userList = this.userService.GetListUser(model.UserName, model.GroupId);
            string[] hiddenGroupNameList = new string[0];

            if (!this.HideGroup(out hiddenGroupNameList))
            {
                userList = userList.Where(x => hiddenGroupNameList.Contains(x.Group.Name) == false);
            }

            var active = userList.Where(o => o.IsActive == true).Count();
            var inactive = userList.Count() - active;

            ViewBag.ActiveUser = active;
            ViewBag.InactiveUser = inactive;
            ViewBag.UserList = userList;

            return this.View();
        }

        // GET: ~/ManangeUser/ResetPassword
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Update)]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            return this.PartialView(model);
        }

        // POST: ~/ManangeUser/ResetPassword
        [HttpPost, ActionName("ResetPassword")]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Update)]
        public ActionResult ResetPasswordPost(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.userService.ResetPassword(CurrentSession.UserId, model.UserId, model.NewPassword, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // GET: ~/ManangeUser/ResetOwnerPassword
        [HttpGet]
        public ActionResult ResetOwnerPassword(ResetPasswordModel model)
        {
            return this.PartialView(model);
        }

        // POST: ~/ManangeUser/ResetOwnerPassword
        [HttpPost, ActionName("ResetOwnerPassword")]
        public ActionResult ResetOwnerPasswordPost(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.userService.ResetPassword(CurrentSession.UserId, model.UserId, model.NewPassword, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // GET: ~/ManangeUser/EditUser
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Update)]
        public ActionResult EditUser(int userId)
        {
            var user = userService.GetUserById(userId);
            EditUserModel model = new EditUserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = userId,
                UserName = user.Name,
                GroupId = user.Group.Id,
                GroupName = user.Group.Name
            };

            return this.PartialView(model);
        }

        // POST: ~/ManangeUser/EditUser
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Update)]
        public ActionResult EditUser(EditUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.userService.EditUser(CurrentSession.UserId, model.UserId, model.FirstName, model.LastName, model.GroupId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // POST: ~/ManangeUser/UpdateStatus
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Update)]
        public ActionResult UpdateStatus(UpdateStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.userService.UpdateStatus(CurrentSession.UserId, model.UserId, model.IsActive, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // GET: ~/ManangeUser/CreateUser
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Read)]
        public ActionResult CreateUser()
        {
            return this.PartialView();
        }

        // POST: ~/ManangeUser/CreateUser
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Create)]
        public ActionResult CreateUser(CreateUserModel model)
        {
            var result = this.userService.CreateUser(CurrentSession.UserId, model.UserName, model.Password, model.FirstName, model.LastName, model.GroupId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // GET: ~/ManangeUser/CreateExistingUser
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Read)]
        public ActionResult CreateExistingUser()
        {
            var model = new CreateExistingUserModel();
            return this.PartialView(model);
        }

        // POST: ~/ManangeUser/CreateExistingUser
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Create)]
        public ActionResult CreateExistingUser(CreateExistingUserModel model)
        {
            var result = this.userService.CreateExistUser(CurrentSession.UserId, model.UserName, model.GroupId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // GET: ~/ManangeUser/EditUserPolicy
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Read)]
        public ActionResult EditUserPolicy(EditUserPolicyModel model)
        {
            var userFunc = this.userService.GetUserFunction(model.UserId);
            ViewBag.UserFunc = userFunc.Where(o => o.Parent.Id == 0);
            ViewBag.UserSubFunc = userFunc.Where(o => o.Parent.Id != 0);

            return this.PartialView(model);
        }

        // POST: ~/ManangeUser/EditUserPolicy
        [HttpPost, ActionName("EditUserPolicy")]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = ePolicy.Update)]
        public ActionResult EditUserPolicyPost(EditUserPolicyModel model)
        {
            var result = this.userService.UpdateUserPolicy(CurrentSession.UserId, model.UserId, model.UserPolicy, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// Processes getting all groups
        /// </summary>
        private IEnumerable<Group> GetAllGroup()
        {
            IEnumerable<Group> groupList = this.groupService.GetGroups();

            string[] hiddenGroupNameList = new string[0];

            if (!this.HideGroup(out hiddenGroupNameList))
            {
                groupList = groupList.Where(x => hiddenGroupNameList.Contains(x.Name) == false);
            }

            return groupList;
        }
    }
}