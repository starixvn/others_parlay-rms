// --------------------------------------------------------------------------
// <copyright file="RMS_MenuSel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Menus
{
    using System;

    /// <summary>
    /// Menu Model.
    /// </summary>
    public class RMS_MenuSel
    {
        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public string MenuDesc { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public string Url { get; set; }

        public int FuncId { get; set; }

        public string FuncName { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool Enabled { get; set; }
    }
}