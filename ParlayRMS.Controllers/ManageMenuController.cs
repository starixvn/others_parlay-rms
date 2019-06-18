using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ParlayRMS.Models.Functions;
using ParlayRMS.Models.Menus;
using ParlayRMS.Services.Implement.Functions;
using ParlayRMS.Services.Implement.Menus;
using ParlayRMS.Services.Implement.Users;
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

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            var menuList = this._menuService.GetAllMenus();
            var parents = menuList.Where(x => x.Parent == null);
            var model = new List<Menu>();
            foreach (var parent in parents)
            {
                model.Add(parent);
                var items = menuList.Where(x => x.Parent != null && x.Parent.Id == parent.Id);
                if (items.Count() > 0)
                {
                    model.AddRange(items);
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// GET method: redirects to Create Menu form.
        /// </summary>
        /// <returns>Create Menu partial view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Create)]
        public ActionResult CreateMenu()
        {
            ViewBag.FunctionList = this._functionService.GetAllFunctionsInOrder();

            var allMenus = this._menuService.GetAllMenus();
            var parentMenus = allMenus.Where(x => x.Parent == null);
            var menuSelectList = parentMenus.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            menuSelectList.Insert(0, new SelectListItem { Value = "0", Text = ViewLang.None });
            ViewBag.MenuSelectList = menuSelectList;

            var model = new CreateMenuModel();
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: creates a new menu.
        /// </summary>
        /// <param name="model">The model wraps the information of a new menu.</param>
        /// <returns>Whether the new menu is created successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Create)]
        public ActionResult CreateMenu(CreateMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            int menuId = 0;
            var result = this._menuService.CreateMenu(
                                               model.Name,
                                               model.Description,
                                               model.Url,
                                               model.FunctionId,
                                               model.ParentId,
                                               model.ParentName,
                                               model.DisplayOrder,
                                               model.IsActive,
                                               this.CurrentSession.UserId,
                                               this.CurrentContext.Ip4Address,
                                               out menuId);
            return this.Json(new { code = result.Code, message = result.Message, menuId = menuId });
        }

        /// <summary>
        /// GET method: redirects to Edit Menu form.
        /// </summary>
        /// <param name="menuId">ID of menu that needs to be edit.</param>
        /// <returns>Partial view of Edit Menu form.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Update)]
        public ActionResult EditMenu(int menuId)
        {
            ViewBag.FunctionList = this._functionService.GetAllFunctionsInOrder();

            var allMenus = this._menuService.GetAllMenus();
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

            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: edits the existing menu.
        /// </summary>
        /// <param name="model">The model abstract the menu information.</param>
        /// <returns>Whether the menu is updated successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Update)]
        public ActionResult EditMenu(EditMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._menuService.UpdateMenu(
                                               model.MenuId,
                                               model.MenuName,
                                               model.Description,
                                               model.Url,
                                               model.FunctionId,
                                               model.ParentId,
                                               model.ParentName,
                                               model.DisplayOrder,
                                               this.CurrentSession.UserId,
                                               this.CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="model">The model wraps menu ID and status.</param>
        /// <returns>Whether the menu's status is changed successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = UserPolicy.Update)]
        public ActionResult UpdateStatus(EditMenuStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._menuService.UpdateMenuStatus(model.MenuId, model.IsActive, CurrentSession.UserId, CurrentContext.Ip4Address);
            return this.Json(result);
        }
    }
}