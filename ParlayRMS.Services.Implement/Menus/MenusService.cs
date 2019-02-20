// --------------------------------------------------------------------------
// <copyright file="MenusService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Menus
{
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.Menus;
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Services.Implement.Systems;

    /// <summary>
    /// Menus Service.
    /// </summary>
    public class MenusService : BaseService
    {
        private readonly MenusDataAccess _menuDataAccess = new MenusDataAccess();

        /// <summary>
        /// Gets all menus.
        /// </summary>
        /// <returns>List of menus.</returns>
        public IEnumerable<Menu> GetAllMenus()
        {
            var data = this._menuDataAccess.RMS_MenuSel();
            return data.Select(x => new Menu
            {
                Id = x.MenuId,
                Name = x.MenuName,
                Description = x.MenuDesc,
                Url = x.Url,
                Parent = x.ParentId == 0 ? null : new Menu
                {
                    Id = x.ParentId,
                    Name = x.ParentName
                },
                Function = new Function
                {
                    Id = x.FuncId,
                    Name = x.FuncName
                },
                DisplayOrder = x.DisplayOrder,
                IsActive = x.Enabled,
                ModifiedDate = x.ModifiedDate
            });
        }

        /// <summary>
        /// Gets menus by user ID.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>List of menus that are available to the user.</returns>
        public IEnumerable<Menu> GetMenusByUserId(int userId)
        {
            var cachingService = new CachingService();
            var userCache = cachingService.GetUserCache(userId);
            return userCache.Menus;
        }

        /// <summary>
        /// Inserts a new menu.
        /// </summary>
        /// <param name="name">Name of menu.</param>
        /// <param name="description">Menu description.</param>
        /// <param name="url">Associated URL of menu.</param>
        /// <param name="functionId">Associated function ID.</param>
        /// <param name="parentId">Parent menu ID.</param>
        /// <param name="parentName">Parent menu name.</param>
        /// <param name="displayOrder">Display order.</param>
        /// <param name="isActive">Menu is activated or not.</param>
        /// <param name="adminId">The ID of administrator who creates the menu.</param>
        /// <param name="ip">IP of machine which creates the menu.</param>
        /// <param name="menuId">Output menu ID value.</param>
        /// <returns>Whether or not menu is created successfully.</returns>
        public Result CreateMenu(
                          string name,
                          string description,
                          string url,
                          int functionId,
                          int parentId,
                          string parentName,
                          int displayOrder,
                          bool isActive,
                          int adminId,
                          string ip,
                          out int menuId)
        {
            return this._menuDataAccess.RMS_MenuIns(name, description, url, functionId, parentId, parentName, displayOrder, isActive, adminId, ip, out menuId);
        }

        /// <summary>
        /// Updates an existing menu.
        /// </summary>
        /// <param name="id">Menu ID value.</param>
        /// <param name="name">Name of menu.</param>
        /// <param name="description">Menu description.</param>
        /// <param name="url">Associated URL of menu.</param>
        /// <param name="functionId">Associated function ID.</param>
        /// <param name="parentId">Parent menu ID.</param>
        /// <param name="parentName">Parent menu name.</param>
        /// <param name="displayOrder">Display order.</param>
        /// <param name="adminId">The ID of administrator who updates the menu.</param>
        /// <param name="ip">IP of machine which updates the menu.</param>
        /// <returns>Whether or not menu is updated successfully.</returns>
        public Result UpdateMenu(
                          int id,
                          string name,
                          string description,
                          string url,
                          int functionId,
                          int parentId,
                          string parentName,
                          int displayOrder,
                          int adminId,
                          string ip)
        {
            return this._menuDataAccess.RMS_MenuUpd(id, name, description, url, functionId, parentId, parentName, displayOrder, adminId, ip);
        }

        /// <summary>
        /// Updates an existing menu status.
        /// </summary>
        /// <param name="id">Menu ID value.</param>
        /// <param name="isActive">Menu is activated or not.</param>
        /// <param name="adminId">The ID of administrator who updates the menu.</param>
        /// <param name="ip">IP of machine which updates the menu.</param>
        /// <returns>Whether or not menu is updated successfully.</returns>
        public Result UpdateMenuStatus(int id, bool isActive, int adminId, string ip)
        {
            return this._menuDataAccess.RMS_MenuUpdStatus(id, isActive, adminId, ip);
        }
    }
}