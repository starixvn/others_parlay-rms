// --------------------------------------------------------------------------
// <copyright file="IPotentialWinningMonitorDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.PotentialWinningMonitor
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.PotentialWinning;

    public interface IPotentialWinningMonitorDataAccess
    {
        /// <summary>
        /// Gets Potential winning list
        /// </summary>
        /// <param name="combos">Combo values.</param>
        /// <param name="sportsTypes">Sport types.</param>
        /// <param name="levels">Level values.</param>
        /// <param name="winningAmount">The winning amount.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <returns>
        /// List of RMS_Monitor_Parlay_BetList objects.
        /// </returns>
        IEnumerable<RMS_Monitor_Potential_Winning> RMS_Monitor_Parlay_PotentialWinning(
            string combos,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop);

        /// <summary>
        /// Gets Potential winning detail.
        /// </summary>
        /// <param name="combos">The combos.</param>
        /// <param name="sportsTypes">The sports types.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Return Potential winning detail.</returns>
        IEnumerable<RMS_Monitor_Potential_Winning_Detail> RMS_Monitor_Parlay_PotentialWinningDetail(
            string combos,
            string sportsTypes,
            long custId);
    }
}