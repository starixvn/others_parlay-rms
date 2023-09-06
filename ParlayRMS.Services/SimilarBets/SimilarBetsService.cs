namespace ParlayRMS.Services.SimilarBets
{
    using System;
    using System.Linq;
    using DataAccess;
    using ParlayRMS.DTO.SimilarBets;
    using Utility;

    public class SimilarBetsService : BaseService
    {
        private readonly SimilarBetsDataAccess _similarbetsDataAccess = new SimilarBetsDataAccess();

        public RMS_Similar_Parlay_Betlist GetSimilarBets(long transId, bool isSimilar)
        {
            var data = _similarbetsDataAccess.RMS_Similar_Parlay_Betlist(transId, isSimilar);
            if (data != null)
            {
                if (data.Details.Any())
                {
                    var stake = data.Stake;
                    var maxPayout = data.Max_payout;
                    var potentialWinningAmount = data.PotentialWinningAmount;
                    var houseLoseAmount = data.HouseLoseAmount;
                    foreach (var item in data.Details)
                    {
                        stake += item.Stake;
                        maxPayout += item.Max_Payout;
                        potentialWinningAmount += item.PotentialWinningAmount;
                        houseLoseAmount += item.HouseLoseAmount;
                    }

                    data.TotalData = new TotalDataModel
                    {
                        SimilarBetsTotal = data.Details.Count() + 1,
                        Stake = Math.Round(stake, 2),
                        MaxPayout = Math.Round(maxPayout, 2),
                        PotentialWinningAmount = potentialWinningAmount,
                        HouseLoseAmount = houseLoseAmount
                    };
                }

                data.Stake = Math.Round(data.Stake, 2);
                data.Max_payout = Math.Round(data.Max_payout, 2);
                data.SportTypeName = CacheRef.GetSportNameDropDown(data.SportType);
            }

            return data;
        }
    }
}
