// --------------------------------------------------------------------------
// <copyright file="DataModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.MixPalayMonitor
{
    using ParlayRMS.Web.Mvc.DataAnnotations;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Data Model.
    /// </summary>
    public class DataModel
    {
        public string Combos { get; set; }

        public string ParlayTypes { get; set; }

        public string SportsTypes { get; set; }

        public string Levels { get; set; }

        public string Markets { get; set; }

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