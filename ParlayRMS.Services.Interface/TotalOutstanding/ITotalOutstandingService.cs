// --------------------------------------------------------------------------
// <copyright file="ITotalOutstandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.TotalOutstanding
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.TotalOutstanding;

    public interface ITotalOutstandingService
    {
        /// <summary>
        /// Gets parlay outstanding of a member.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="combo">Combo values.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <returns>Parlay Outstanding of a member.</returns>
        List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(long custId, string combo, string sportsType);

        /// <summary>
        /// Gets Parlay Outstanding of a agent.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="combo">Combo values.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <returns>Parlay outstanding of a agent.</returns>
        List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(long custId, string combo, string sportsType);
    }
}