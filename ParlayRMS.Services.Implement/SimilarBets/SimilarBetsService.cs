using ParlayRMS.DataAccess.Implement.SimilarBets;
using ParlayRMS.DataAccess.Interface.SimilarBets;
using ParlayRMS.DTO.SimilarBets;
using ParlayRMS.Services.Interface.SimilarBets;
using ParlayRMS.Utility;
using System;

namespace ParlayRMS.Services.Implement.SimilarBets
{
    public class SimilarBetsService : BaseService, ISimilarBetsService
    {
        private readonly ISimilarBetsDataAccess _similarbetsDataAccess = new SimilarBetsDataAccess();

        public RMS_Similar_Parlay_Betlist GetSimilarBets(long transId)
        {
            var data = _similarbetsDataAccess.RMS_Similar_Parlay_Betlist(transId);
            if (data != null)
            {
                data.Stake = Math.Round(data.Stake, 2);
                data.Max_payout = Math.Round(data.Max_payout, 2);
                data.SportTypeName = CacheRef.GetSportNameDropDown(data.SportType);
            }

            return data;
        }
    }
}