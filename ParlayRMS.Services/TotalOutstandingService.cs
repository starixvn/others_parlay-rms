namespace ParlayRMS.Services
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess;
    using ParlayRMS.DTO.TotalOutstanding;
    using ParlayRMS.Utility;

    public class TotalOutstandingService
    {
        private readonly TotalOutstandingDataAccess _dataAccess = new TotalOutstandingDataAccess();

        public List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(int custId, string combo, string sportsType, string parlaysType, bool isCornerParlay)
        {
            var data = _dataAccess.RMS_Rpt_Parlay_Outstanding_Mem(custId, combo, sportsType, parlaysType, isCornerParlay);
            foreach (var item in data)
            {
                item.StakePt = Math.Round(item.StakePt, 2);
                item.MaxPayoutPt = Math.Floor(item.MaxPayoutPt);
                item.SportTypeName = CacheRef.GetSportNameDropDown(item.SportType);
            }

            return data;
        }

        public List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(int custId, string combo, string sportsType, string parlaysType, bool isCornerParlay)
        {
            var data = _dataAccess.RMS_Rpt_Parlay_Outstanding_Ag(custId, combo, sportsType, parlaysType, isCornerParlay);
            foreach (var item in data)
            {
                item.StakePt = Math.Round(item.StakePt, 2);
                item.MaxPayoutPt = Math.Floor(item.MaxPayoutPt);
            }

            return data;
        }
    }
}
