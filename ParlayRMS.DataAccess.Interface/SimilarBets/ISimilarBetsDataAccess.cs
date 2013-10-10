// --------------------------------------------------------------------------
// <copyright file="ISimilarBetsDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.SimilarBets
{
    using ParlayRMS.DTO.SimilarBets;

    public interface ISimilarBetsDataAccess
    {
        /// <summary>
        /// Gets similar bet list.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <returns>List of similar bets.</returns>
        RMS_Similar_Parlay_Betlist RMS_Similar_Parlay_Betlist(long transId);
    }
}