namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Groups;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Implement.Groups;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.Groups;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.ManageGroup;
    using ParlayRMS.Web.Authorization;

    public class ManageGroupController : BaseController
    {
        public const string HIDDEN_GROUPS_KEY = "HiddenGroups";
        
        private readonly IGroupsService groupService = new GroupsService();

        // GET: ~/ManangeGroup
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageGroup, Policy = ePolicy.Read)]
        public ActionResult Index()
        {
            var groupList = this.groupService.GetGroups();
            string[] hiddenGroupNameList = new string[0];

            if (!this.HideGroup(out hiddenGroupNameList))
            {
                groupList = groupList.Where(x => hiddenGroupNameList.Contains(x.Name) == false);
            }

            return this.View(groupList);
        }

        // GET: ~/ManangeGroup/CreateGroup
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageGroup, Policy = ePolicy.Create)]
        public ActionResult CreateGroup()
        {
            var model = new CreateGroupModel();
            return this.PartialView(model);
        }

        // POST: ~/ManangeGroup/CreateGroup
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.ManageGroup, Policy = ePolicy.Create)]
        public ActionResult CreateGroup(CreateGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            int groupId = 0;
            var result = this.groupService.CreateGroup(model.GroupName, model.Description, CurrentSession.UserId, CurrentContext.Ip4Address, out groupId);
            return this.Json(new { code = result.Code, message = result.Message, groupid = groupId });
        }

        // GET: ~/ManangeGroup/EditGroup
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageGroup, Policy = ePolicy.Update)]
        public ActionResult EditGroup(int groupId)
        {
            var group = this.groupService.GetGroupById(groupId);
            var model = new EditGroupModel
            {
                GroupId = groupId,
                GroupName = group.Name,
                Description = group.Description
            };

            return this.PartialView(model);
        }

        // POST: ~/ManangeGroup/EditGroup
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.ManageGroup, Policy = ePolicy.Update)]
        public ActionResult EditGroup(EditGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.groupService.EditGroup(model.GroupName, model.Description, CurrentSession.UserId, CurrentContext.Ip4Address, model.GroupId);
            return this.Json(new { code = result.Code, message = result.Message });
        }

        // GET: ~/ManangeGroup/CreateGroupPolicy
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageGroup, Policy = ePolicy.Create)]
        public ActionResult CreateGroupPolicy(CreateGroupPolicyModel model)
        {
            var functionList = this.groupService.GetFunctionsByGroupId(model.GroupId);
            ViewBag.Functions = functionList.Where(x => x.Parent.Id == 0);
            ViewBag.SubFunctions = functionList.Where(x => x.Parent.Id != 0);
            return this.PartialView(model);
        }

        // POST: ~/ManangeGroup/CreateGroupPolicy
        [HttpPost, ActionName("CreateGroupPolicy")]
        [AuthorizationAttribute(Function = eFunction.ManageGroup, Policy = ePolicy.Update)]
        public ActionResult CreateGroupPolicyPost(CreateGroupPolicyModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.groupService.EditGroupPolicy(model.GroupPolicy, CurrentSession.UserId, CurrentContext.Ip4Address, model.GroupId);
            return this.Json(new { code = result.Code, message = result.Message });
        }
    }
}