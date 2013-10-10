// --------------------------------------------------------------------------
// <copyright file="IWinlossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.Winloss
{
    using System;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Winloss;

    public interface IWinlossService
    {
        /// <summary>
        /// Gets win loss data.
        /// </summary>
        /// <param name="fromDate">From Date value.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Win Loss data.</returns>
        WinlossData GetWinloss(DateTime fromDate, DateTime toDate, BetType betType, SportsType sportsType, string combos);
    }
}