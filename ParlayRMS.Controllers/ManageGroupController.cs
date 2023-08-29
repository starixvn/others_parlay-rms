using System.Linq;
using System.Web.Mvc;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Groups;
using ParlayRMS.Services.Users;
using ParlayRMS.ViewModel.ManageGroup;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Controllers
{
    public class ManageGroupController : BaseController
    {
        private readonly GroupsService _groupService = new GroupsService();
        private readonly UsersService _userService = new UsersService();

        public ManageGroupController()
        {
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageGroup);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            var groupList = this._groupService.GetGroups();
            string[] hiddenGroupNameList = new string[0];

            if (!this.HideGroup(out hiddenGroupNameList))
            {
                groupList = groupList.Where(x => hiddenGroupNameList.Contains(x.Name) == false);
            }

            return this.View(groupList);
        }

        /// <summary>
        /// GET method: redirects to Create Group view.
        /// </summary>
        /// <returns>Create Group view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Create)]
        public ActionResult CreateGroup()
        {
            var model = new CreateGroupModel();
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: creates new group.
        /// </summary>
        /// <param name="model">The model contains the information of new group.</param>
        /// <returns>Whether the new group is created successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Create)]
        public ActionResult CreateGroup(CreateGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            int groupId = 0;
            var result = this._groupService.CreateGroup(model.GroupName, model.Description, CurrentSession.UserId, CurrentContext.Ip4Address, out groupId);
            return this.Json(new { code = result.Code, message = result.Message, groupid = groupId });
        }

        /// <summary>
        /// GET method: redirects to Edit Group view.
        /// </summary>
        /// <param name="groupId">The ID of group that needs to be edit.</param>
        /// <returns>Edit Group view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Update)]
        public ActionResult EditGroup(int groupId)
        {
            var group = this._groupService.GetGroupById(groupId);
            var model = new EditGroupModel
            {
                GroupId = groupId,
                GroupName = group.Name,
                Description = group.Description
            };

            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: edits an existing group.
        /// </summary>
        /// <param name="model">The model contains the information of group.</param>
        /// <returns>Whether the group is edit successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Update)]
        public ActionResult EditGroup(EditGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._groupService.EditGroup(model.GroupName, model.Description, CurrentSession.UserId, CurrentContext.Ip4Address, model.GroupId);
            return this.Json(new { code = result.Code, message = result.Message });
        }

        /// <summary>
        /// GET method: redirect to Edit Group Policy view.
        /// </summary>
        /// <param name="model">The model contains the current information of the group.</param>
        /// <returns>Edit Group Policy view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Create)]
        public ActionResult EditGroupPolicy(EditGroupPolicyModel model)
        {
            var functionList = this._groupService.GetFunctionsByGroupId(model.GroupId);
            ViewBag.Functions = functionList.Where(x => x.Parent.Id == 0);
            ViewBag.SubFunctions = functionList.Where(x => x.Parent.Id != 0);
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: edits an existing group's policy.
        /// </summary>
        /// <param name="model">The model contains the information of group.</param>
        /// <returns>Whether the group's policy is edit successfully or not.</returns>
        [HttpPost, ActionName("EditGroupPolicy")]
        [AuthorizationAttribute(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Update)]
        public ActionResult EditGroupPolicyPost(EditGroupPolicyModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._groupService.EditGroupPolicy(model.GroupPolicy, CurrentSession.UserId, CurrentContext.Ip4Address, model.GroupId);
            return this.Json(new { code = result.Code, message = result.Message });
        }
    }
}