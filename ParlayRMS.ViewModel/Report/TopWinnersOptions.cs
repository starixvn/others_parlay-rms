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
    }
}
