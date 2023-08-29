namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Services.Menus;
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
            var menuList = this._menuService.GetMenusByUserId(CurrentSession.UserId);
            var rootMenuList = menuList.Where(x => x.Parent == null);
            var menuDict = new Dictionary<string, MenuItem>();

            foreach (var item in rootMenuList)
            {
                string key = item.Name;
                MenuItem menuItem = new MenuItem
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

            return this.PartialView(menuDict);
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