// --------------------------------------------------------------------------
// <copyright file="RMS_UserInfoSel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Users
{
    using System;

    /// <summary>
    /// User Information.
    /// </summary>
    public class RMS_UserInfoSel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Enabled { get; set; }
    }
}