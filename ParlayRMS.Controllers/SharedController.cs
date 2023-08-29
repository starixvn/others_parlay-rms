namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Services.Menus;
    using Utility;
    using ViewModel.Shared;

    public class SharedController : BaseController
    {
        private readonly MenusService _menuService = new MenusService();

        public ActionResult _AuthInfo()
        {
            return PartialView();
        }

        public ActionResult _Menu()
        {
            var menuList = _menuService.GetMenusByUserId(CurrentSession.UserId);
            var rootMenuList = menuList.Where(x => x.Parent == null);
            var menuDict = new Dictionary<string, MenuItem>();

            foreach (var item in rootMenuList)
            {
                var key = item.Name;
                var menuItem = new MenuItem
                {
                    Name = item.Name,
                    Url = item.Url,
                    Children = new List<MenuItem>()
                };

                var value = menuList.Where(x => x.Parent != null && x.Parent.Id == item.Id).Select(
                    x => new MenuItem
                    {
                        Name = x.Name,
                        Url = x.Url
                    });
                menuItem.Children.AddRange(value.ToList());

                if (menuItem.Children.Count > 0 || !string.IsNullOrEmpty(menuItem.Url))
                {
                    menuDict.Add(key, menuItem);
                }
            }

            return PartialView(menuDict);
        }

        public ActionResult MessageBox(string title, string content)
        {
            ViewBag.Title = title;
            ViewBag.Content = content;

            return PartialView();
        }

        public virtual ActionResult _SearchBar()
        {
            return PartialView();
        }

        public ActionResult _Pager(Pager model)
        {
            return PartialView(model);
        }
    }
}
