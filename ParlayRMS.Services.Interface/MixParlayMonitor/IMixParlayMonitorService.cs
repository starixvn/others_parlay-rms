// --------------------------------------------------------------------------
// <copyright file="IMixParlayMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.MixParlayMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.MixParlayMonitor;

    public interface IMixParlayMonitorService
    {
        /// <summary>
        /// Gets the mix parlay monitor tickets.
        /// </summary>
        /// <param name="combos">The combo values.</param>
        /// <param name="sportTypes">Type of the sport.</param>
        /// <param name="levels">The level.</param>
        /// <param name="maxPayout">The max payout.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <returns>List of RMS_Monitor_Parlay_BetList objects.</returns>
        IEnumerable<RMS_Monitor_Parlay_Betlist> GetMixParlayMonitorTickets(
                                                string combos, 
                                                string sportTypes, 
                                                string levels, 
                                                decimal maxPayout, 
                                                int selectTop, 
                                                long maxTransId);
    }
}