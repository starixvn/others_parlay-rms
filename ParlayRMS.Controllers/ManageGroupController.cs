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

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            var groupList = _groupService.GetGroups();

            if (!HideGroup(out var hiddenGroupNameList))
            {
                groupList = groupList.Where(x => hiddenGroupNameList.Contains(x.Name) == false);
            }

            return View(groupList);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Create)]
        public ActionResult CreateGroup()
        {
            var model = new CreateGroupModel();
            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Create)]
        public ActionResult CreateGroup(CreateGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _groupService.CreateGroup(model.GroupName, model.Description, CurrentSession.UserId, CurrentContext.Ip4Address, out var groupId);
            return Json(new { code = result.Code, message = result.Message, groupid = groupId });
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Update)]
        public ActionResult EditGroup(int groupId)
        {
            var group = _groupService.GetGroupById(groupId);
            var model = new EditGroupModel
            {
                GroupId = groupId,
                GroupName = group.Name,
                Description = group.Description
            };

            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Update)]
        public ActionResult EditGroup(EditGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _groupService.EditGroup(model.GroupName, model.Description, CurrentSession.UserId, CurrentContext.Ip4Address, model.GroupId);
            return Json(new { code = result.Code, message = result.Message });
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Create)]
        public ActionResult EditGroupPolicy(EditGroupPolicyModel model)
        {
            var functionList = _groupService.GetFunctionsByGroupId(model.GroupId);
            ViewBag.Functions = functionList.Where(x => x.Parent.Id == 0);
            ViewBag.SubFunctions = functionList.Where(x => x.Parent.Id != 0);
            return PartialView(model);
        }

        [HttpPost, ActionName("EditGroupPolicy")]
        [Authorization(Function = FunctionKeys.ManageGroup, Policy = UserPolicy.Update)]
        public ActionResult EditGroupPolicyPost(EditGroupPolicyModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _groupService.EditGroupPolicy(model.GroupPolicy, CurrentSession.UserId, CurrentContext.Ip4Address, model.GroupId);
            return Json(new { code = result.Code, message = result.Message });
        }
    }
}
