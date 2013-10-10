// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_SelectionByTicket_Event_BetType model.
    /// </summary>
    public class RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype
    {
        public int BetType { get; set; }

        public int Tickets { get; set; }

        public decimal Margin { get; set; }

        public int RowNumber { get; set; }
    }
}