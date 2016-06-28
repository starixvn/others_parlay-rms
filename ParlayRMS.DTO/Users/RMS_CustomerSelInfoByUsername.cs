// --------------------------------------------------------------------------
// <copyright file="RMS_CustomerSelInfoByUsername.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Users
{
    /// <summary>
    /// Customer model for selecting by username SP.
    /// </summary>
    public class RMS_CustomerSelInfoByUsername
    {
        public long CustId { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }
    }
}