// --------------------------------------------------------------------------
// <copyright file="IPotentialWinningMonitor.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.PotentialWinningMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.PotentialWinning;

    public interface IPotentialWinningMonitor
    {
        /// <summary>
        /// Gets the mix parlay monitor tickets.
        /// </summary>
        /// <param name="combos">The combo values.</param>
        /// <param name="sportTypes">Type of the sport.</param>
        /// <param name="levels">The level.</param>
        /// <param name="winningAmount">The winning amount.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <returns>
        /// List of RMS_Monitor_Parlay_BetList objects.
        /// </returns>
        IEnumerable<RMS_Monitor_Potential_Winning> GetPotentialWinningList(
            string combos,
            string sportTypes,
            string levels,
            decimal winningAmount,
            int selectTop);

        /// <summary>
        /// Gets the potential winning detail list.
        /// </summary>
        /// <param name="combos">The combos.</param>
        /// <param name="sportTypes">The sport types.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Return Potential Winning Detail list.</returns>
        IEnumerable<RMS_Monitor_Potential_Winning_Detail> GetPotentialWinningDetailList(
            string combos,
            string sportTypes,
            long custId);
    }
}