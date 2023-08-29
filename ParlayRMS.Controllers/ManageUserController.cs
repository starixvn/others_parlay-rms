namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models.Functions;
    using Models.Groups;
    using Models.Users;
    using Services.Groups;
    using Services.Users;
    using ViewModel.ManageUser;
    using Web.Authorization;

    public class ManageUserController : BaseController
    {
        private readonly UsersService _userService = new UsersService();
        private readonly GroupsService _groupService = new GroupsService();

        public ManageUserController()
        {
            var groups = _groupService.GetGroupNames();
            var selectedItems = new List<SelectListItem>();

            if (!HideGroup(out var hiddenGroupNameList))
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

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult Index(IndexModel model)
        {
            ViewBag.GroupList = GetAllGroup();

            ViewBag.UserList = new List<User>();
            CurrentContext.Context.Session["groupId"] = model.GroupId;

            var userList = _userService.GetListUser(model.UserName, model.GroupId);

            if (!HideGroup(out var hiddenGroupNameList))
            {
                userList = userList.Where(x => hiddenGroupNameList.Contains(x.Group.Name) == false);
            }

            var active = userList.Count(o => o.IsActive);
            var inactive = userList.Count() - active;

            ViewBag.ActiveUser = active;
            ViewBag.InactiveUser = inactive;
            ViewBag.UserList = userList;

            return View();
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            return PartialView(model);
        }

        [HttpPost, ActionName("ResetPassword")]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult ResetPasswordPost(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _userService.ResetPassword(CurrentSession.UserId, model.UserId, model.NewPassword, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        public ActionResult ResetOwnerPassword(ResetPasswordModel model)
        {
            return PartialView(model);
        }

        [HttpPost, ActionName("ResetOwnerPassword")]
        public ActionResult ResetOwnerPasswordPost(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _userService.ResetPassword(CurrentSession.UserId, model.UserId, model.NewPassword, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult EditUser(int userId)
        {
            var user = _userService.GetUserById(userId);
            var model = new EditUserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = userId,
                UserName = user.Name,
                GroupId = user.Group.Id,
                GroupName = user.Group.Name
            };

            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult EditUser(EditUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _userService.EditUser(CurrentSession.UserId, model.UserId, model.FirstName, model.LastName, model.GroupId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult UpdateStatus(UpdateStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _userService.UpdateStatus(CurrentSession.UserId, model.UserId, model.IsActive, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult CreateUser()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Create)]
        public ActionResult CreateUser(CreateUserModel model)
        {
            var result = _userService.CreateUser(
                                               CurrentSession.UserId,
                                               model.UserName,
                                               model.Password,
                                               model.FirstName,
                                               model.LastName,
                                               model.GroupId,
                                               CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult CreateExistingUser()
        {
            var model = new CreateExistingUserModel();
            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Create)]
        public ActionResult CreateExistingUser(CreateExistingUserModel model)
        {
            var result = _userService.CreateExistUser(CurrentSession.UserId, model.UserName, model.GroupId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Read)]
        public ActionResult EditUserPolicy(EditUserPolicyModel model)
        {
            var userFunc = _userService.GetUserFunction(model.UserId);
            ViewBag.UserFunc = userFunc.Where(o => o.Parent.Id == 0);
            ViewBag.UserSubFunc = userFunc.Where(o => o.Parent.Id != 0);

            return PartialView(model);
        }

        [HttpPost, ActionName("EditUserPolicy")]
        [Authorization(Function = FunctionKeys.ManageUser, Policy = UserPolicy.Update)]
        public ActionResult EditUserPolicyPost(EditUserPolicyModel model)
        {
            var result = _userService.UpdateUserPolicy(CurrentSession.UserId, model.UserId, model.UserPolicy, CurrentContext.Ip4Address);
            return Json(result);
        }

        private IEnumerable<Group> GetAllGroup()
        {
            var groupList = _groupService.GetGroups();

            if (!HideGroup(out var hiddenGroupNameList))
            {
                groupList = groupList.Where(x => hiddenGroupNameList.Contains(x.Name) == false);
            }

            return groupList;
        }
    }
}
