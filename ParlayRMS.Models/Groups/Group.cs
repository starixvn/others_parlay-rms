// --------------------------------------------------------------------------
// <copyright file="Group.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Groups
{
    using System;
    using ParlayRMS.Models.Users;

    /// <summary>
    /// Group Model.
    /// </summary>
    public class Group : BaseEntityModel
    {
        public string Description { get; set; }

        public int Policy { get; set; }

        public User Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public int NumberOfMember { get; set; }
    }
}