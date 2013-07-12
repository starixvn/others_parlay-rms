// -----------------------------------------------------------------------
// <copyright file="WinlossOptions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.Report
{
    using System;
    using ParlayRMS.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinlossOptions
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public BetType BetType { get; set; }
    }
}