namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Session;
    using Utility;
    using ViewModel.Shared;

    public class SharedController : BaseController
    {
        private readonly ICurrentSession session = new CurrentSession();

    
        public ActionResult _Menu()
        {
            if (session.UserId == 0)
            {
                return Content(string.Empty);
            }

            var menuList = session.MenuItemList;
            var rootMenuList = menuList.Where(x => x.ParentId == 0);
            var menuDict = new Dictionary<string, MenuItem>();

            foreach (var item in rootMenuList)
            {
                var key = item.Name;
                var menuItem = new MenuItem
                {
                    Name = item.Name,
                    Url = item.Url,
                    Children = new List<MenuItem>(),
                    DisplayOrder = item.DisplayOrder
                };

                var value = menuList.Where(x => x.ParentId == item.Id).Select(
                    x => new MenuItem
                    {
                        Name = x.Name,
                        Url = x.Url,
                        DisplayOrder = x.DisplayOrder
                    })
                    .OrderBy(x=>x.DisplayOrder);
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

        public ActionResult _Pager(Pager model)
        {
            return PartialView(model);
        }
    }
}
