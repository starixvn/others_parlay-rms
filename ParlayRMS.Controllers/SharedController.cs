// --------------------------------------------------------------------------
// <copyright file="SharedController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.5.23">Format styles</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.ViewModel.Shared;
    using ParlayRMS.Services.Interface.Menus;
    using ParlayRMS.Services.Implement.Menus;

    /// <summary>
    /// Shared controller object
    /// </summary>
    public class SharedController : BaseController
    {
        private readonly IMenusService menuService = new MenusService();
        /// <summary>
        /// Gets authentication information menu
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult _AuthInfo()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Gets menu form view
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult _Menu()
        {
            // load menus
            var menuList = this.menuService.GetMenusByUserId(CurrentSession.UserId);
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

            ViewBag.MenuDict = menuDict;

            return this.PartialView();
        }

        /// <summary>
        /// Gets footer menu
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult _Footer()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Gets popup form
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult _Popup()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Gets loading view
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult _Loading()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Gets loading in popup view
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult _LoadingInPopUp()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Gets message box view
        /// </summary>
        /// <param name="title">Title of message box</param>
        /// <param name="content">Content of message box</param>
        /// <returns>ActionResult object</returns>
        public ActionResult MessageBox(string title, string content)
        {
            ViewBag.Title = title;
            ViewBag.Content = content;

            return this.PartialView();
        }

        /// <summary>
        /// Gets search bar
        /// </summary>
        /// <returns>ActionResult object</returns>
        public virtual ActionResult _SearchBar()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Gets unauthorization form
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult Unauthorized()
        {
            return this.View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ActionResult _Pager()
        {
            return this.PartialView();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ActionResult _AlertBox()
        {
            return this.PartialView();
        }
    }
}