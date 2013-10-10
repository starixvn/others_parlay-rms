// --------------------------------------------------------------------------
// <copyright file="Function.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Functions
{
    using System;
    using ParlayRMS.Models.Users;

    /// <summary>
    /// Function Model.
    /// </summary>
    public class Function : BaseEntityModel
    {
        public string Description { get; set; }

        public string Key { get; set; }

        public Function Parent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public User Creator { get; set; }

        public int DisplayOrder { get; set; }

        public Policy GroupPolicy { get; set; }

        public Policy UserPolicy { get; set; }
    }
}