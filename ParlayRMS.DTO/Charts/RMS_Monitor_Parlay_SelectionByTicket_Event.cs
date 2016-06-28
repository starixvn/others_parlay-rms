// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_SelectionByTicket_Event.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_SelectionByTicket_Event model.
    /// </summary>
    public class RMS_Monitor_Parlay_SelectionByTicket_Event
    {
        public int Awayid { get; set; }

        public int Homeid { get; set; }

        public int Leagueid { get; set; }

        public int Matchid { get; set; }

        public int Tickets { get; set; }
    }
}