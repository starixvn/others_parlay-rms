// --------------------------------------------------------------------------
// <copyright file="Menu.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Menus
{
    using System;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Users;

    /// <summary>
    /// Menu Model.
    /// </summary>
    public class Menu : BaseEntityModel
    {
        public string Description { get; set; }

        public Menu Parent { get; set; }

        public Function Function { get; set; }

        public string Url { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public User Creator { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}