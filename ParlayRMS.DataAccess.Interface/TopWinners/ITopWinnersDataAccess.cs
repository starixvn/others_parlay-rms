// --------------------------------------------------------------------------
// <copyright file="ITopWinnersDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.TopWinners
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.BetList;
    using ParlayRMS.DTO.TopWinners;

    public interface ITopWinnersDataAccess
    {
        /// <summary>
        /// Gets top winners.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="selectTop">Number of top winners will be selected.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>List of top winners.</returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner> RMS_Rpt_Parlay_TopWinners(
                                                  DateTime fromDate,
                                                  DateTime toDate,
                                                  DateTime moveBetsDay,
                                                  DateTime balanceUpDay,
                                                  int sportsType,
                                                  int selectTop,
                                                  string combos);

        /// <summary>
        /// Gets a top winner details.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Top winner details.</returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details(
                                                         long custId,
                                                         DateTime fromDate,
                                                         DateTime toDate,
                                                         DateTime moveBetsDay,
                                                         int sportsType,
                                                         string combos);

        /// <summary>
        /// Gets a top winner bet details.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>Top winner bet details.</returns>
        IEnumerable<RMS_Parlay_TransDetails> RMS_Rpt_Parlay_TopWinners_Bet_Details(long refNo);

        /// <summary>
        /// Gets a top winner bet details.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Top winner bet details.</returns>
        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details_old(
                                                         long custId,
                                                         DateTime fromDate,
                                                         DateTime toDate,
                                                         int sportsType,
                                                         string combos);
    }
}