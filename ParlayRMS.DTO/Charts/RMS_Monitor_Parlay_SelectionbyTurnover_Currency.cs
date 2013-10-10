// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_Parlay_SelectionbyTurnover_Currency.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Charts
{
    /// <summary>
    /// RMS_Monitor_Parlay_SelectionByTurnover_Currency model.
    /// </summary>
    public class RMS_Monitor_Parlay_SelectionbyTurnover_Currency
    {
        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public decimal Margin { get; set; }

        public decimal Turnover { get; set; }
    }
}