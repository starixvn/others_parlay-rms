// --------------------------------------------------------------------------
// <copyright file="IWinlossDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Winloss
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.Winloss;

    public interface IWinlossDataAccess
    {
        /// <summary>
        /// Gets Win Loss data based on passed filter conditions.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>List of Win Loss objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_Winloss> RMS_Rpt_Parlay_Winloss(
                                                DateTime fromDate, 
                                                DateTime toDate, 
                                                DateTime moveBetsDay, 
                                                DateTime balanceUpDay, 
                                                int betType, 
                                                int sportsType, 
                                                string combos);
    }
}