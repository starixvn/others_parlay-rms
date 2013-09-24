// --------------------------------------------------------------------------
// <copyright file="IMemberWinLossDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Interface.MemberWinLoss
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.MemberWinLoss;

    /// <summary>
    /// Data Access contract for Member Win Loss report.
    /// </summary>
    public interface IMemberWinLossDataAccess
    {
        /// <summary>
        /// Get Member Win Loss data based on passed filter conditions.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="combos">Combos value.</param>
        /// <param name="roleId">Role ID of the customer.</param>
        /// <param name="custId">Customer ID.</param>
        /// <returns>List of RMS_Rpt_Parlay_MemberWinLoss</returns>
        IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> RMS_Rpt_Parlay_MemberWinloss(DateTime fromDate, 
                                                                               DateTime toDate, 
                                                                               DateTime moveBetsDay, 
                                                                               DateTime balanceUpDay, 
                                                                               int betType, 
                                                                               int sportsType, 
                                                                               string combos,
                                                                               int roleId,
                                                                               long custId);
    }
}
