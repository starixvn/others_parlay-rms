// --------------------------------------------------------------------------
// <copyright file="RMS_UserInfoSelByUserID.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Users
{
    /// <summary>
    /// Model for selecting user information by user ID.
    /// </summary>
    public class RMS_UserInfoSelByUserID
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public bool Enabled { get; set; }
    }
}