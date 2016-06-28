// --------------------------------------------------------------------------
// <copyright file="User.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Users
{
    using System;
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Groups;

    /// <summary>
    /// User Model.
    /// </summary>
    public class User : BaseEntityModel
    {
        public string PassWord { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public User Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public Group Group { get; set; }

        public Request Request { get; set; }
    }
}