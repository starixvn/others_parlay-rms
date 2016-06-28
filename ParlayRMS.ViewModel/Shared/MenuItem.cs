// --------------------------------------------------------------------------
// <copyright file="MenuItem.cs" company="">
// Copyright (c) . All rights reserved.
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