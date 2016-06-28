// --------------------------------------------------------------------------
// <copyright file="IMenusService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.Menus
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Menus;

    public interface IMenusService
    {
        /// <summary>
        /// Gets all menus.
        /// </summary>
        /// <returns>List of menus.</returns>
        IEnumerable<Menu> GetAllMenus();

        /// <summary>
        /// Gets menus by user ID.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>List of menus that are available to the user.</returns>
        IEnumerable<Menu> GetMenusByUserId(int userId);

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
        Result CreateMenu(
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
                   out int menuId);

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
        Result UpdateMenu(
                   int id,
                   string name,
                   string description,
                   string url,
                   int functionId,
                   int parentId,
                   string parentName,
                   int displayOrder,
                   int adminId,
                   string ip);

        /// <summary>
        /// Updates an existing menu status.
        /// </summary>
        /// <param name="id">Menu ID value.</param>
        /// <param name="isActive">Menu is activated or not.</param>
        /// <param name="adminId">The ID of administrator who updates the menu.</param>
        /// <param name="ip">IP of machine which updates the menu.</param>
        /// <returns>Whether or not menu is updated successfully.</returns>
        Result UpdateMenuStatus(int id, bool isActive, int adminId, string ip);
    }
}