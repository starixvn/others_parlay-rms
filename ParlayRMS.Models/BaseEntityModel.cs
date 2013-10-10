// --------------------------------------------------------------------------
// <copyright file="BaseEntityModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Base Entity Model.
    /// </summary>
    public class BaseEntityModel : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
