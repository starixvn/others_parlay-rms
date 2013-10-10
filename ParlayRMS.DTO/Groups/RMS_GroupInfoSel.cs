// --------------------------------------------------------------------------
// <copyright file="RMS_GroupInfoSel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Groups
{
    using System;

    /// <summary>
    /// Group Information model.
    /// </summary>
    public class RMS_GroupInfoSel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int NoUsers { get; set; }

        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}