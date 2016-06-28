// --------------------------------------------------------------------------
// <copyright file="ChartDetailCache.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Charts
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.Charts;

    /// <summary>
    /// Chart Model.
    /// </summary>
    public class ChartDetailCache
    {
        public long MaxTransId { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> SelectionByTicket_League
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> SelectionByTurnover_League
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> SelectionbyTicket_Currency
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> SelectionbyTurnover_Currency
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> SelectionbyTicketEventByCurrency
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> SelectionbyTurnoverEventByCurrency
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> SelectionbyTicketEventByLeague
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> SelectionbyTurnoverEventByLeague
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice> SelectionByTicketEventBettypeChoice
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice> SelectionByTurnoverEventBettypeChoice
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> SelectionByTicketEventBettype
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> SelectionByTurnoverEventBettype
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> SelectionByTicketEvent
        { 
            get; 
            set; 
        }

        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> SelectionByTurnoverEvent
        { 
            get; 
            set; 
        }
    }
}