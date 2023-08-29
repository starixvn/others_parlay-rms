// --------------------------------------------------------------------------
// <copyright file="MenusDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Menus
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Menus;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Menus Data Access.
    /// </summary>
    public class MenusDataAccess : BaseDataAccess
    {
        /// <summary>
        /// Gets list of menus.
        /// </summary>
        /// <returns> List of menus.</returns>
        public IEnumerable<RMS_MenuSel> RMS_MenuSel()
        {
            var result = new List<RMS_MenuSel>();

            using (var reader = ExecSpDataReader("RMS_MenuSel"))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_MenuSel
                    {
                        MenuId = reader.ToInt32("menuid"),
                        MenuName = reader.ToString("menuname"),
                        MenuDesc = reader.ToString("menudesc"),
                        Url = reader.ToString("url"),
                        ParentId = reader.ToInt32("parentid"),
                        ParentName = reader.ToString("parentname"),
                        FuncId = reader.ToInt32("funcid"),
                        FuncName = reader.ToString("funcname"),
                        DisplayOrder = reader.ToInt32("displayorder"),
                        ModifiedDate = reader.ToDateTime("ModifiedDate"),
                        Enabled = reader.ToBoolean("enabled")
                    });
                }
            }

            return result;
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
        public Result RMS_MenuIns(
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
            menuId = 0;
            var result = Result.Empty;

            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("menuname", name);
            param.Add("menudesc", description);
            param.Add("url", url);
            param.Add("funcid", functionId);
            param.Add("parentid", parentId);
            param.Add("parentname", parentName);
            param.Add("displayorder", displayOrder);
            param.Add("enabled", isActive);
            param.Add("ip", ip);

            var r = ExecSp("RMS_MenuIns", param);
            result = r.ToResult();
            if (result.IsSuccessful)
            {
                menuId = DbConverter.ToInt32(r.Outputs["menuid"]);
            }

            return result;
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
        public Result RMS_MenuUpd(
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
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("menuid", id);
            param.Add("menuname", name);
            param.Add("menudesc", description);
            param.Add("url", url);
            param.Add("funcid", functionId);
            param.Add("parentid", parentId);
            param.Add("parentname", parentName == null ? DBNull.Value : (object)parentName);
            param.Add("displayorder", displayOrder);
            param.Add("ip", ip);

            var r = ExecSp("RMS_MenuUpd", param);

            return r.ToResult();
        }

        /// <summary>
        /// Updates an existing menu status.
        /// </summary>
        /// <param name="id">Menu ID value.</param>
        /// <param name="isActive">Menu is activated or not.</param>
        /// <param name="adminId">The ID of administrator who updates the menu.</param>
        /// <param name="ip">IP of machine which updates the menu.</param>
        /// <returns>Whether or not menu is updated successfully.</returns>
        public Result RMS_MenuUpdStatus(int id, bool isActive, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("menuid", id);
            param.Add("enabled", isActive);
            param.Add("ip", ip);

            var r = ExecSp("RMS_MenuUpdStatus", param);

            return r.ToResult();
        }
    }
}