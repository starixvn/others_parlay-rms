// --------------------------------------------------------------------------
// <copyright file="ITopWinnersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.TopWinners
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.BetList;
    using ParlayRMS.DTO.TopWinners;

    public interface ITopWinnersService
    {
        /// <summary>
        /// Gets top winners.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>List of top winners.</returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner> GetTopWinners(DateTime fromDate, DateTime toDate, int sportsType, int selectTop, string combos);

        /// <summary>
        /// Gets top winner details.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sportsType">Sport type values.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Detail of a top winner.</returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(long custId, DateTime fromDate, DateTime toDate, int sportsType, string combos);

        /// <summary>
        /// Gets bet detail.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>Transaction detail.</returns>
        IEnumerable<RMS_Parlay_TransDetails> GetBetDetail(long refNo);
    }
}