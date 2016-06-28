// --------------------------------------------------------------------------
// <copyright file="CombinationDetailsByDateView.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.24">Created</change>
// </history>

namespace ParlayRMS.Models.ViewIpInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.ViewIPInfo;

    /// <summary>
    /// Bet combination details view model.
    /// </summary>
    public class CombinationDetailsByDateView
    {
        public DateTime Date { get; set; }

        public List<CombinationDetailsView> ViewItem { get; set; }
    }
}