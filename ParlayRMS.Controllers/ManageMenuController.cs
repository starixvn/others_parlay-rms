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

    public class ManageMenuController : BaseController
    {
        private readonly IMenusService menuService = new MenusService();
        private readonly IFunctionsService functionService = new FunctionsService();

        // GET: ~/ManangeMenu
        [HttpGet]
        [AuthorizationAttribute(Function = eFunction.ManageMenu, Policy = ePolicy.Read)]
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
        [AuthorizationAttribute(Function = eFunction.ManageMenu, Policy = ePolicy.Create)]
        public ActionResult CreateMenu()
        {
            var functionList = this.functionService.GetAllFunctionsInOrder();
            ViewBag.FunctionList = functionList;

            var menuList = this.menuService.GetAllMenus();
            var rootMenuList = menuList.Where(x => x.Parent == null);
            ViewBag.MenuList = rootMenuList;

            var model = new CreateMenuModel();
            return this.PartialView(model);
        }

        // POST: ~/ManangeMenu/CreateMenu
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.ManageMenu, Policy = ePolicy.Create)]
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

        [AuthorizationAttribute(Function = eFunction.ManageMenu, Policy = ePolicy.Update)]
        public ActionResult EditMenu(int menuId)
        {
            ViewBag.FunctionList = this.functionService.GetAllFunctionsInOrder();

            var menuList = this.menuService.GetAllMenus();
            var rootMenuList = menuList.Where(x => x.Parent == null && x.Id != menuId);
            ViewBag.MenuList = rootMenuList;
            var menu = menuList.Where(x => x.Id == menuId).FirstOrDefault();
            var model = new EditMenuModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Url = menu.Url,
                Description = menu.Description,
                FunctionId = menu.Function.Id,
                ParentId = menu.Parent != null ? menu.Parent.Id : 0,
                ParentName = menu.Parent != null ? menu.Parent.Name : string.Empty,
                DisplayOrder = menu.DisplayOrder
            };

            return this.PartialView(model);
        }

        // POST: ~/ManangeMenu/SaveUpdateMenu
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.ManageMenu, Policy = ePolicy.Update)]
        public ActionResult SaveUpdateMenu(EditMenuModel model)
        {
            var result = this.menuService.UpdateMenu(model.Id, model.Name, model.Description, model.Url, model.FunctionId, model.ParentId, model.ParentName, model.DisplayOrder, this.CurrentSession.UserId, this.CurrentContext.Ip4Address);
            return this.Json(new { code = result.Code, message = result.Message });
        }

        // POST: ~/ManangeMenu/UpdateStatus
        [HttpPost]
        [AuthorizationAttribute(Function = eFunction.ManageMenu, Policy = ePolicy.Update)]
        public ActionResult UpdateStatus(EditMenuStatusModel model)
        {
            var result = this.menuService.UpdateMenuStatus(model.Id, model.IsActive, CurrentSession.UserId, CurrentContext.Ip4Address);
            return this.Json(new { code = result.Code, message = result.Message });
        }
    }
}