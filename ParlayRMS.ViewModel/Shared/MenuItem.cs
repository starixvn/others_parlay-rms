// --------------------------------------------------------------------------
// <copyright file="MenuItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Shared
{
    using System.Collections.Generic;

    /// <summary>
    /// Menu Item.
    /// </summary>
    public class MenuItem
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public List<MenuItem> Children { get; set; }
    }
}