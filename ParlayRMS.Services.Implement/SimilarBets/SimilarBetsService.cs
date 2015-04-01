// --------------------------------------------------------------------------
// <copyright file="SimilarBetsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.SimilarBets
{
    using System;
    using ParlayRMS.DataAccess.Implement.SimilarBets;
    using ParlayRMS.DataAccess.Interface.SimilarBets;
    using ParlayRMS.DTO.SimilarBets;
    using ParlayRMS.Services.Interface.SimilarBets;

    /// <summary>
    /// Similar Bets Service.
    /// </summary>
    public class SimilarBetsService : BaseService, ISimilarBetsService
    {
        private readonly ISimilarBetsDataAccess _similarbetsDataAccess = new SimilarBetsDataAccess();

        /// <summary>
        /// Gets similar bets.
        /// </summary>
        /// <param name="transId">Transaction ID.</param>
        /// <returns>List of similar bets.</returns>
        public RMS_Similar_Parlay_Betlist GetSimilarBets(long transId)
        {
            var data = this._similarbetsDataAccess.RMS_Similar_Parlay_Betlist(transId);
            if (data != null)
            {
                data.Odds = Math.Round(data.Odds, 2);
                data.Stake = Math.Round(data.Stake, 2);
                data.Max_payout = Math.Round(data.Max_payout, 2);
            }

            return data;
        }
    }
}