// --------------------------------------------------------------------------
// <copyright file="IMixParlayMonitorDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.MixParlayMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.MixParlayMonitor;

    public interface IMixParlayMonitorDataAccess
    {
        /// <summary>
        /// Gets Monitor Parlay Bet List.
        /// </summary>
        /// <param name="combos">Combo values.</param>
        /// <param name="sportsTypes">Sport types.</param>
        /// <param name="levels">Level values.</param>
        /// <param name="maxPayout">Maximum payout value.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_BetList objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_Betlist> RMS_Monitor_Parlay_Betlist(
                                                    string combos,
                                                    string sportsTypes,
                                                    string levels,
                                                    decimal maxPayout,
                                                    int selectTop,
                                                    long maxTransId);
    }
}