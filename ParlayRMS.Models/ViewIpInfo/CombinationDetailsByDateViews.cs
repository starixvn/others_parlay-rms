// --------------------------------------------------------------------------
// <copyright file="CombinationDetailsByDateViews.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.24">Created</change>
// </history>

namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary of Bet combination details view model.
    /// </summary>
    public class CombinationDetailsByDateViews
    {
        public decimal Outstanding { get; set; }

        public Dictionary<DateTime, List<CombinationDetailsView>> ViewDict { get; set; }
    }
}