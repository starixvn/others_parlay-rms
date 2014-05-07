// --------------------------------------------------------------------------
// <copyright file="CombinationDetailsViews.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.22">Created</change>
// </history>

namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.ViewIPInfo;

    /// <summary>
    /// List of Bet combination details view model.
    /// </summary>
    public class CombinationDetailsViews
    {
        public decimal Outstanding { get; set; }

        public IEnumerable<CombinationDetailsView> ViewList { get; set; }
    }
}