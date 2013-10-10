// --------------------------------------------------------------------------
// <copyright file="MemberWinLossOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.ViewModel.Report
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Models;
    using ParlayRMS.Utility;
    using ParlayRMS.Web.Mvc.DataAnnotations;

    /// <summary>
    /// ViewModel for Member Win Loss filter form.
    /// </summary>
    public class MemberWinLossOptions
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

        public int? PageIndex { get; set; }

        public bool IsCached { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        /// <summary>
        /// Get minimum date that support querying data.
        /// </summary>
        /// <returns>Minimum date that the system supports to query data.</returns>
        public static DateTime GetLimitDate()
        {
            string memberWinLossLimitDate = AppSetting.MemberWinLossLimitDate;
            DateTime limitDate = Convert.ToDateTime(memberWinLossLimitDate);
            DateTime minDate = DateTime.Now.AddYears(-1);
            return minDate > limitDate ? minDate : limitDate;
        }

        /// <summary>
        /// Get report title.
        /// </summary>
        /// <returns>The agent/member win loss report.</returns>
        public string GetTitle()
        {
            var combosTitle = ViewLang.All;
            var combos = this.Combos.Split(',');
            if (combos.Length < 5)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "6" ? ViewLang.SixUp : x));
            }

            var title = string.Format(
                                "{0} of {1} - from {2} to {3} - {4} [{5}] - {6} [{7}] - {8} [{9}]",
                                ViewLang.AgentMemberWinLoss,
                                this.Username,
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
