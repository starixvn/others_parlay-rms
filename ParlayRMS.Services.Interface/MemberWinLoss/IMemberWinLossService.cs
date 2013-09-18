// --------------------------------------------------------------------------
// <copyright file="IMemberWinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.Services.Interface.MemberWinLoss
{
    using System;
    using ParlayRMS.Models;
    using ParlayRMS.Models.MemberWinLoss;

    /// <summary>
    /// Member Win Loss Service contact.
    /// </summary>
    public interface IMemberWinLossService
    {
        /// <summary>
        /// Get Member Win Loss data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combos value.</param>
        /// <param name="username">Username.</param>
        /// <returns></returns>
        MemberWinLossData GetMemberWinLoss(DateTime fromDate, 
                                           DateTime toDate, 
                                           BetType betType, 
                                           SportsType sportsType, 
                                           string combos,
                                           string username);
    }
}
