// --------------------------------------------------------------------------
// <copyright file="RMS_MenuSelByUserID_Menu.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Users
{
    /// <summary>
    /// Menu model for selecting menu item by user ID SP.
    /// </summary>
    public class RMS_MenuSelByUserID_Menu
    {
        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public string Url { get; set; }

        public int DisplayOrder { get; set; }
    }
}