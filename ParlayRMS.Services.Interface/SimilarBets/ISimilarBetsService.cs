// --------------------------------------------------------------------------
// <copyright file="ISimilarBetsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.SimilarBets
{
    using ParlayRMS.DTO.SimilarBets;

    public interface ISimilarBetsService
    {
        /// <summary>
        /// Gets similar bets.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <returns>List of similar bets.</returns>
        RMS_Similar_Parlay_Betlist GetSimilarBets(long transId);
    }
}