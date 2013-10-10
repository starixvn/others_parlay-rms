// --------------------------------------------------------------------------
// <copyright file="WinlossOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Report
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Models;
    using ParlayRMS.Utility;
    using ParlayRMS.Web.Mvc.DataAnnotations;

    /// <summary>
    /// Win Loss Options.
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

        /// <summary>
        /// Gets the title of win loss report.
        /// </summary>
        /// <returns>Title of win loss report.</returns>
        public string GetTitle()
        {
            var combosTitle = ViewLang.All;
            var combos = this.Combos.Split(',');
            if (combos.Length < 5)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "6" ? ViewLang.SixUp : x));
            }

            var title = string.Format(
                                   "{0} - from {1} to {2} - {3} [{4}] - {5} [{6}] - {7} [{8}]",
                                   ViewLang.WinLoss,
                                   this.FromDate.ToString("MMMM dd, yyyy"),
                                   this.ToDate.ToString("MMMM dd, yyyy"),
                                   ViewLang.Bettype,
                                   ModelsHelper.GetBetType((int)this.BetType),
                                   ViewLang.Combo,
                                   combosTitle,
                                   ViewLang.SportsType,
                                   ModelsHelper.GetSportType((int)this.SportsType));
            return title;
        }
    }
}