using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ParlayRMS.Models.Functions;
using ParlayRMS.Models.Menus;
using ParlayRMS.Services.Functions;
using ParlayRMS.Services.Menus;
using ParlayRMS.Services.Users;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.ManageMenu;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Controllers
{
    public class ManageMenuController : BaseController
    {
        private readonly MenusService _menuService = new MenusService();
        private readonly FunctionsService _functionService = new FunctionsService();
        private readonly UsersService _userService = new UsersService();

        public ManageMenuController()
        {
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageMenu);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            var menuList = _menuService.GetAllMenus();
            var parents = menuList.Where(x => x.Parent == null);
            var model = new List<Menu>();
            foreach (var parent in parents)
            {
                model.Add(parent);
                var items = menuList.Where(x => x.Parent != null && x.Parent.Id == parent.Id);
                if (items.Any())
                {
                    model.AddRange(items);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Create)]
        public ActionResult CreateMenu()
        {
            ViewBag.FunctionList = _functionService.GetAllFunctionsInOrder();

            var allMenus = _menuService.GetAllMenus();
            var parentMenus = allMenus.Where(x => x.Parent == null);
            var menuSelectList = parentMenus.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            menuSelectList.Insert(0, new SelectListItem { Value = "0", Text = ViewLang.None });
            ViewBag.MenuSelectList = menuSelectList;

            var model = new CreateMenuModel();
            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Create)]
        public ActionResult CreateMenu(CreateMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _menuService.CreateMenu(
                                               model.Name,
                                               model.Description,
                                               model.Url,
                                               model.FunctionId,
                                               model.ParentId,
                                               model.ParentName,
                                               model.DisplayOrder,
                                               model.IsActive,
                                               CurrentSession.UserId,
                                               CurrentContext.Ip4Address,
                                               out var menuId);
            return Json(new { code = result.Code, message = result.Message, menuId });
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Update)]
        public ActionResult EditMenu(int menuId)
        {
            ViewBag.FunctionList = _functionService.GetAllFunctionsInOrder();

            var allMenus = _menuService.GetAllMenus();
            var parentMenus = allMenus.Where(x => x.Parent == null && x.Id != menuId);
            var menu = allMenus.Where(x => x.Id == menuId).FirstOrDefault();

            var menuSelectList = parentMenus.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            menuSelectList.Insert(0, new SelectListItem { Value = "0", Text = ViewLang.None });
            ViewBag.MenuSelectList = menuSelectList;

            var model = new EditMenuModel
            {
                MenuId = menu.Id,
                MenuName = menu.Name,
                Url = menu.Url,
                Description = menu.Description,
                FunctionId = menu.Function.Id,
                ParentId = menu.Parent != null ? menu.Parent.Id : 0,
                ParentName = menu.Parent != null ? menu.Parent.Name : string.Empty,
                DisplayOrder = menu.DisplayOrder
            };

            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Update)]
        public ActionResult EditMenu(EditMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _menuService.UpdateMenu(
                                               model.MenuId,
                                               model.MenuName,
                                               model.Description,
                                               model.Url,
                                               model.FunctionId,
                                               model.ParentId,
                                               model.ParentName,
                                               model.DisplayOrder,
                                               CurrentSession.UserId,
                                               CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Update)]
        public ActionResult UpdateStatus(EditMenuStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _menuService.UpdateMenuStatus(model.MenuId, model.IsActive, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }
    }
}
