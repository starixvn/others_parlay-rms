// -----------------------------------------------------------------------
// <copyright file="TopWinnersOptions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TopWinnersOptions
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SportsType { get; set; }

        public int SelectTop { get; set; }

        public string Combos { get; set; }

        public string GetTile()
        {
            var combosTitle = ViewLang.All;
            var combos = this.Combos.Split(',');
            if (combos.Length < 5)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "6" ? ViewLang.SixUp : x));
            }

            string title = string.Format("Top {0} winners - from {1} to {2} - {3} [{4}] - {5} [{6}]",
                    this.SelectTop,
                    this.FromDate.ToString("MMMM dd, yyyy"),
                    this.ToDate.ToString("MMMM dd, yyyy"),
                    ViewLang.Combo,
                    combosTitle,
                    ViewLang.SportsType,
                    ModelsHelper.GetSportType(this.SportsType));
            return title;
        }
    }
}
