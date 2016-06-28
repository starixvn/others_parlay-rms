// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_SelectionByTurnover_Event_BetType model.
    /// </summary>
    public class RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype
    {
        public int BetType { get; set; }

        public decimal Turnover { get; set; }

        public decimal Margin { get; set; }

        public int RowNumber { get; set; }
    }
}