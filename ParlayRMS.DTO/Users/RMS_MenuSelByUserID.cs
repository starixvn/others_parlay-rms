// --------------------------------------------------------------------------
// <copyright file="RMS_MenuSelByUserID.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Users
{
    using System.Collections.Generic;

    /// <summary>
    /// List of menus model.
    /// </summary>
    public class RMS_MenuSelByUserID
    {
        public IEnumerable<RMS_MenuSelByUserID_Menu> Menus { get; set; }

        public IEnumerable<RMS_MenuSelByUserID_Function> Functions { get; set; }
    }
}