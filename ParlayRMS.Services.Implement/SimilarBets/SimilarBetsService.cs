// -----------------------------------------------------------------------
// <copyright file="SimilarBetsService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.SimilarBets
{
    using System;
    using ParlayRMS.DataAccess.Implement.SimilarBets;
    using ParlayRMS.DataAccess.Interface.SimilarBets;
    using ParlayRMS.DTO.SimilarBets;
    using ParlayRMS.Services.Interface.SimilarBets;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimilarBetsService : BaseService, ISimilarBetsService
    {
        private readonly ISimilarBetsDataAccess similarbetsDataAccess = new SimilarBetsDataAccess();

        public RMS_Similar_Parlay_Betlist GetSimilarBets(long transId)
        {
            var data = this.similarbetsDataAccess.RMS_Similar_Parlay_Betlist(transId);
            if (data != null)
            {
                data.Odds = Math.Round(data.Odds, 2);
                data.Stake = Math.Round(data.Stake, 2);
                data.Max_payout = Math.Round(data.Odds * data.Stake, 2);
            }

            return data;
        }
    }
}