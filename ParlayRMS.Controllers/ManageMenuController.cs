namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Services.Implement.Functions;
    using ParlayRMS.Services.Implement.Menus;
    using ParlayRMS.Services.Interface.Functions;
    using ParlayRMS.Services.Interface.Menus;
    using ParlayRMS.ViewModel.ManageMenu;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Utility;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Services.Implement.Users;

    public class ManageMenuController : BaseController
    {
        private readonly IMenusService menuService = new MenusService();
        private readonly IFunctionsService functionService = new FunctionsService();
        private readonly IUsersService userService = new UsersService();

        public ManageMenuController()
        {
            ViewBag.HasWritePermission = userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageMenu);
        }

        // GET: ~/ManangeMenu
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = ePolicy.Read)]
        public ActionResult Index()
        {
            var menuList = this.menuService.GetAllMenus();
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

        // GET: ~/ManangeMenu/CreateMenu
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = ePolicy.Create)]
        public ActionResult CreateMenu()
        {
            ViewBag.FunctionList = this.functionService.GetAllFunctionsInOrder();

            var allMenus = this.menuService.GetAllMenus();
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

        // POST: ~/ManangeMenu/CreateMenu
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = ePolicy.Create)]
        public ActionResult CreateMenu(CreateMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            int menuId = 0;
            var result = this.menuService.CreateMenu(model.Name, model.Description, model.Url, model.FunctionId, model.ParentId, model.ParentName, model.DisplayOrder, model.IsActive, this.CurrentSession.UserId, this.CurrentContext.Ip4Address, out menuId);
            return this.Json(new { code = result.Code, message = result.Message, menuId = menuId });
        }

        // GET: ~/ManangeMenu/EditMenu
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = ePolicy.Update)]
        public ActionResult EditMenu(int menuId)
        {
            ViewBag.FunctionList = this.functionService.GetAllFunctionsInOrder();

            var allMenus = this.menuService.GetAllMenus();
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

        // POST: ~/ManangeMenu/EditMenu
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = ePolicy.Update)]
        public ActionResult EditMenu(EditMenuModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.menuService.UpdateMenu(model.MenuId, model.MenuName, model.Description, model.Url, model.FunctionId, model.ParentId, model.ParentName, model.DisplayOrder, this.CurrentSession.UserId, this.CurrentContext.Ip4Address);
            return this.Json(result);
        }

        // POST: ~/ManangeMenu/UpdateStatus
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageMenu, Policy = ePolicy.Update)]
        public ActionResult UpdateStatus(EditMenuStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this.menuService.UpdateMenuStatus(model.MenuId, model.IsActive, CurrentSession.UserId, CurrentContext.Ip4Address);
            return this.Json(result);
        }
    }
}