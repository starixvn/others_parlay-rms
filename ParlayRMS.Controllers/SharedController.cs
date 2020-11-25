namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Services.Implement.Menus;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Shared;

    /// <summary>
    /// Shared controller object.
    /// </summary>
    public class SharedController : BaseController
    {
        private readonly MenusService _menuService = new MenusService();

        /// <summary>
        /// Gets authentication information menu.
        /// </summary>
        /// <returns>ActionResult object.</returns>
        public ActionResult _AuthInfo()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Gets menu form view.
        /// </summary>
        /// <returns>ActionResult object.</returns>
        public ActionResult _Menu()
        {
            // load menus
            var menuList = this._menuService.GetMenusByUserId(CurrentSession.UserId).ToList();
            var rootMenuList = menuList.Where(x => x.Parent == null);
            var menuDict = new Dictionary<string, MenuItem>();

            foreach (var item in rootMenuList)
            {
                string key = item.Name;
                MenuItem menuItem = new MenuItem
                {
                    Name = item.Name,
                    Url = GetMenuUrl(item.Url),
                    Children = new List<MenuItem>()
                };

                var value = menuList.Where(x => x.Parent != null && x.Parent.Id == item.Id).Select(
                    x => new MenuItem
                    {
                        Name = x.Name,
                        Url = GetMenuUrl(x.Url),
                    }).ToList();
                menuItem.Children.AddRange(value);

                if (menuItem.Children.Count > 0 || !string.IsNullOrEmpty(menuItem.Url))
                {
                    menuDict.Add(key, menuItem);
                }
            }

            return this.PartialView(menuDict);
        }

        private string GetMenuUrl(string menuUrl)
        {
            menuUrl = string.IsNullOrEmpty(menuUrl) ? string.Empty : menuUrl;
            if (!menuUrl.Contains("~/NetCore/"))
            {
                return menuUrl;
            }

            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
            if (!string.IsNullOrEmpty(aspNetCoreAuth))
            {
                var applicationPath = Request.ApplicationPath.Replace("/", "");
                applicationPath = "NetCore" + applicationPath;
                aspNetCoreAuth = aspNetCoreAuth.Replace("{ApplicationPath}", applicationPath);

                return aspNetCoreAuth.Replace("PropagationAuth", "") + menuUrl.Replace("~/NetCore/", "");
            }

            return string.Empty;
        }


        /// <summary>
        /// Gets message box view.
        /// </summary>
        /// <param name="title">Title of message box.</param>
        /// <param name="content">Content of message box.</param>
        /// <returns>ActionResult object.</returns>
        public ActionResult MessageBox(string title, string content)
        {
            ViewBag.Title = title;
            ViewBag.Content = content;

            return this.PartialView();
        }

        /// <summary>
        /// Gets search bar.
        /// </summary>
        /// <returns>ActionResult object.</returns>
        public virtual ActionResult _SearchBar()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Generates pager part.
        /// </summary>
        /// <param name="model">Pager model for paging purpose.</param>
        /// <returns>Partial view.</returns>
        public ActionResult _Pager(Pager model)
        {
            return this.PartialView(model);
        }
    }
}