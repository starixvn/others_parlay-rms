// -----------------------------------------------------------------------
// <copyright file="WinlossOptions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.Report
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Web.Mvc.DataAnnotations;
    using ParlayRMS.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinlossOptions
    {
        [Required(ErrorMessage = "FromDate is required")]
        [DateRange("1900/01/01", "9999/12/31")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "ToDate is required")]
        [DateRange("1900/01/01", "9999/12/31")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "BetType is required")]
        public BetType BetType { get; set; }

        public SportsType SportsType { get; set; }

        public string Combos { get; set; }
    }
}