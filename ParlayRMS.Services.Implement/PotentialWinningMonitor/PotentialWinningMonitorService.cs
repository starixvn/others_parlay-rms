// --------------------------------------------------------------------------
// <copyright file="PotentialWinningMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.PotentialWinningMonitor
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.PotentialWinningMonitor;
    using ParlayRMS.DataAccess.Interface.PotentialWinningMonitor;
    using ParlayRMS.DTO.PotentialWinning;
    using ParlayRMS.Services.Interface.PotentialWinningMonitor;

    /// <summary>
    /// Potential Winning Monitor Service.
    /// </summary>
    public class PotentialWinningMonitorService : BaseService, IPotentialWinningMonitor
    {
        private readonly IPotentialWinningMonitorDataAccess _potentialWinningDataAccess = new PotentialWinningMonitorDataAccess();

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
        public IEnumerable<RMS_Monitor_Potential_Winning> GetPotentialWinningList(
            string combos,
            string sportTypes,
            string levels,
            decimal winningAmount,
            int selectTop)
        {
            return _potentialWinningDataAccess.RMS_Monitor_Parlay_PotentialWinning(combos, sportTypes, levels, winningAmount, selectTop);
        }

        /// <summary>
        /// Gets the potential winning detail list.
        /// </summary>
        /// <param name="combos">The combos.</param>
        /// <param name="sportTypes">The sport types.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Return Potential Winning Detail list.</returns>
        public IEnumerable<RMS_Monitor_Potential_Winning_Detail> GetPotentialWinningDetailList(
            string combos,
            string sportTypes,
            long custId)
        {
            return _potentialWinningDataAccess.RMS_Monitor_Parlay_PotentialWinningDetail(combos, sportTypes, custId);
        }
    }
}