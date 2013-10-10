// --------------------------------------------------------------------------
// <copyright file="Member.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Members
{
    using System;
    using ParlayRMS.Models.Users;

    /// <summary>
    /// Member model.
    /// </summary>
    public class Member
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public MemberLevel Level { get; set; }

        public int Following { get; set; }

        public DateTime CreateDate { get; set; }

        public User Creator { get; set; }
    }
}