// --------------------------------------------------------------------------
// <copyright file="ManageUserController.cs" company="">
// Copyright (c) . All rights reserved.
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
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Models.Users;
    using ParlayRMS.Services.Groups;
    using ParlayRMS.Services.Users;
    using ParlayRMS.ViewModel.ManageUser;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// Manages user methods.
    /// </summary>
    public class ManageUserController : BaseController
    {
        private readonly UsersService _userService = new UsersService();
        private readonly GroupsService _groupService = new GroupsService();

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageUserController"/> class.
        /// </summary>
        public ManageUserController()
        {
            var groups = this._groupService.GetGroupNames();
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
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageUser);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <param name="model">The model contains the user name and group ID.</param>
        /// <returns>Index view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult Index(IndexModel model)
        {
            ViewBag.GroupList = this.GetAllGroup();

            ViewBag.UserList = new List<User>();
            CurrentContext.Context.Session["groupId"] = model.GroupId;

            var userList = this._userService.GetListUser(model.UserName, model.GroupId);
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

        /// <summary>
        /// GET method: resets password.
        /// </summary>
        /// <param name="model">The model wraps information needed for reset password process.</param>
        /// <returns>Reset password form.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: resets password.
        /// </summary>
        /// <param name="model">The model wraps information needed for reset password process.</param>
        /// <returns>Whether the password is reset successfully or not.</returns>
        [HttpPost, ActionName("ResetPassword")]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult ResetPasswordPost(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._userService.ResetPassword(CurrentSession.UserId, model.UserId, model.NewPassword, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// GET method: resets the owner's password.
        /// </summary>
        /// <param name="model">The model contains information for reset owner's password.</param>
        /// <returns>Reset owner's password form.</returns>
        [HttpGet]
        public ActionResult ResetOwnerPassword(ResetPasswordModel model)
        {
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: resets the owner's password.
        /// </summary>
        /// <param name="model">The model contains information for reset owner's password.</param>
        /// <returns>Whether the password is reset successfully or not.</returns>
        [HttpPost, ActionName("ResetOwnerPassword")]
        public ActionResult ResetOwnerPasswordPost(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._userService.ResetPassword(CurrentSession.UserId, model.UserId, model.NewPassword, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// GET method: edits the user information.
        /// </summary>
        /// <param name="userId">The ID that is used to get user's data.</param>
        /// <returns>Edit user information form.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult EditUser(int userId)
        {
            var user = _userService.GetUserById(userId);
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

        /// <summary>
        /// POST method: edits user information.
        /// </summary>
        /// <param name="model">The model contains updated information.</param>
        /// <returns>Whether the password is reset successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult EditUser(EditUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._userService.EditUser(CurrentSession.UserId, model.UserId, model.FirstName, model.LastName, model.GroupId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// POST method: Updates user status.
        /// </summary>
        /// <param name="model">The model contains user ID and user status.</param>
        /// <returns>Whether the status is updated successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult UpdateStatus(UpdateStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._userService.UpdateStatus(CurrentSession.UserId, model.UserId, model.IsActive, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// GET method: creates new user.
        /// </summary>
        /// <returns>Create user form.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult CreateUser()
        {
            return this.PartialView();
        }

        /// <summary>
        /// POST method: creates a new user.
        /// </summary>
        /// <param name="model">Model contains input data for creating new user purpose.</param>
        /// <returns>Whether the new user is created successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Create)]
        public ActionResult CreateUser(CreateUserModel model)
        {
            var result = this._userService.CreateUser(
                                               CurrentSession.UserId,
                                               model.UserName,
                                               model.Password,
                                               model.FirstName,
                                               model.LastName,
                                               model.GroupId,
                                               CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// GET method: adds an existing user to the system.
        /// </summary>
        /// <returns>Add an existing user form.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult CreateExistingUser()
        {
            var model = new CreateExistingUserModel();
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: adds an existing user to the system.
        /// </summary>
        /// <param name="model">Model contains information of the user.</param>
        /// <returns>Whether the existing user is added successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Create)]
        public ActionResult CreateExistingUser(CreateExistingUserModel model)
        {
            var result = this._userService.CreateExistUser(CurrentSession.UserId, model.UserName, model.GroupId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// Gets Edit User Policy partial view.
        /// </summary>
        /// <param name="model">The model wraps information that can be edit.</param>
        /// <returns>Edit User Policy partial view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult EditUserPolicy(EditUserPolicyModel model)
        {
            var userFunc = this._userService.GetUserFunction(model.UserId);
            ViewBag.UserFunc = userFunc.Where(o => o.Parent.Id == 0);
            ViewBag.UserSubFunc = userFunc.Where(o => o.Parent.Id != 0);

            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: edits the user policy.
        /// </summary>
        /// <param name="model">The model contains new policy of the user.</param>
        /// <returns>Whether the user policy is reset successfully or not.</returns>
        [HttpPost, ActionName("EditUserPolicy")]
        [AuthorizationAttribute(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult EditUserPolicyPost(EditUserPolicyModel model)
        {
            var result = this._userService.UpdateUserPolicy(CurrentSession.UserId, model.UserId, model.UserPolicy, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// Processes getting all groups.
        /// </summary>
        /// <returns>List of all groups.</returns>
        private IEnumerable<Group> GetAllGroup()
        {
            IEnumerable<Group> groupList = this._groupService.GetGroups();

            string[] hiddenGroupNameList = new string[0];

            if (!this.HideGroup(out hiddenGroupNameList))
            {
                groupList = groupList.Where(x => hiddenGroupNameList.Contains(x.Name) == false);
            }

            return groupList;
        }
    }
}