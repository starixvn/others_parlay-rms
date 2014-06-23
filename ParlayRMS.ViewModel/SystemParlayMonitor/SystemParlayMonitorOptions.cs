// --------------------------------------------------------------------------
// <copyright file="SystemParlayMonitorOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.21">Created</change>
// </history>

namespace ParlayRMS.ViewModel.SystemParlayMonitor
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Web.Mvc.DataAnnotations;

    /// <summary>
    /// System parlay monitor filter options.
    /// </summary>
    public class SystemParlayMonitorOptions
    {
        public string Combos { get; set; }

        public string SportTypes { get; set; }

        public string Levels { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal StakeAfterPT { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal NetWinAfterPT { get; set; }

        public int ReportType { get; set; }

        [Required, Values(10, 20, 50, 100, 200)]
        public int SelectTop { get; set; }

        [Required, Range(0, long.MaxValue)]
        public long MaxTransId { get; set; }
    }
}