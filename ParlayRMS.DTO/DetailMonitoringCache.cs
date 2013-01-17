// -----------------------------------------------------------------------
// <copyright file="DetailMonitoringCachecs.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DetailMonitoringCache
    {
        /// <summary>
        ///
        /// </summary>
        public List<RMS_Monitor_Parlay_SelectionByTicket_League> RMS_Monitor_Parlay_SelectionByTicket_League
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<RMS_Monitor_Parlay_SelectionByTurnover_League> RMS_Monitor_Parlay_SelectionByTurnover_League
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<RMS_Monitor_Parlay_SelectionbyTicket_Currency> RMS_Monitor_Parlay_SelectionbyTicket_Currency
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> RMS_Monitor_Parlay_SelectionbyTurnover_Currency
        { get; set; }
    }
}
