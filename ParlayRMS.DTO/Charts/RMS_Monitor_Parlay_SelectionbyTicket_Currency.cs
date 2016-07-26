// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_SelectionbyTicket_Currency.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_SelectionByTicket_Currency model.
    /// </summary>
    public class RMS_Monitor_Parlay_SelectionbyTicket_Currency
    {
        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public decimal Margin { get; set; }

        public int NoTickets { get; set; }
    }
}