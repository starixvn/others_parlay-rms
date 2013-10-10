// --------------------------------------------------------------------------
// <copyright file="ITotalOutstandingDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.TotalOutstanding
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.TotalOutstanding;

    public interface ITotalOutstandingDataAccess
    {
        /// <summary>
        /// Gets Parlay Outstanding of a member.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="combo">Combo values.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <returns>Parlay Outstanding of a member.</returns>
        List<RMS_Rpt_Parlay_Outstanding_Mem> RMS_Rpt_Parlay_Outstanding_Mem(long custId, string combo, string sportsType);

        /// <summary>
        /// Gets Parlay Outstanding of a agent.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="combo">Combo values.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <returns>Parlay Outstanding of a agent.</returns>
        List<RMS_Rpt_Parlay_Outstanding_Ag> RMS_Rpt_Parlay_Outstanding_Ag(long custId, string combo, string sportsType);
    }
}