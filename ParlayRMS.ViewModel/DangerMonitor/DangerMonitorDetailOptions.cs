// --------------------------------------------------------------------------
// <copyright file="DangerMonitorDetailOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.ViewModel.DangerMonitor
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Options that are used to filter bet details of a specific match.
    /// </summary>
    public class DangerMonitorDetailOptions
    {
        public int MatchId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [AllowHtml]
        public string AgentIdList { get; set; }

        public string Combos { get; set; }

        public long MaxTransId { get; set; }

        public decimal MaxPayout { get; set; }
    }
}
