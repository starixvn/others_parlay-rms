// -----------------------------------------------------------------------
// <copyright file="MonitoringDetailCache.cs" company="">
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
    public class ChartDetailCache
    {
        public long MaxTransId { get; set; }

        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> SelectionByTicket_League
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> SelectionByTurnover_League
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> SelectionbyTicket_Currency
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> SelectionbyTurnover_Currency
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> SelectionbyTicketEventByCurrency
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> SelectionbyTurnoverEventByCurrency
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> SelectionbyTicketEventByLeague
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> SelectionbyTurnoverEventByLeague
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice> SelectionByTicketEventBettypeChoice
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice> SelectionByTurnoverEventBettypeChoice
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> SelectionByTicketEventBettype
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> SelectionByTurnoverEventBettype
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> SelectionByTicketEvent
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> SelectionByTurnoverEvent
        { get; set; }
    }
}