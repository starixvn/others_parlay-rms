// -----------------------------------------------------------------------
// <copyright file="MonitoringSummaryCache.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Models.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Charts;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChartSummaryCache
    {
        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_Sum> Sum
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> SelectionByTicket
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> SelectionByTurnover
        { get; set; }
    }
}
