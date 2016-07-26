using ParlayRMS.DataAccess.Implement.TotalOutstanding;
using ParlayRMS.DataAccess.Interface.TotalOutstanding;
using ParlayRMS.DTO.TotalOutstanding;
using ParlayRMS.Services.Interface.TotalOutstanding;
using ParlayRMS.Utility;
using System;
using System.Collections.Generic;

namespace ParlayRMS.Services.Implement.TotalOutstanding
{
    public class TotalOutstandingService : ITotalOutstandingService
    {
        private readonly ITotalOutstandingDataAccess _dataAccess = new TotalOutstandingDataAccess();

        public List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(long custId, string combo, string sportsType, string parlaysType)
        {
            var data = _dataAccess.RMS_Rpt_Parlay_Outstanding_Mem(custId, combo, sportsType, parlaysType);
            foreach (var item in data)
            {
                item.StakePt = Math.Round(item.StakePt, 2);
                item.MaxPayoutPt = Math.Floor(item.MaxPayoutPt);
                item.SportTypeName = CacheRef.GetSportNameDropDown(item.SportType);
            }

            return data;
        }

        public List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(long custId, string combo, string sportsType, string parlaysType)
        {
            var data = _dataAccess.RMS_Rpt_Parlay_Outstanding_Ag(custId, combo, sportsType, parlaysType);
            foreach (var item in data)
            {
                item.StakePt = Math.Round(item.StakePt, 2);
                item.MaxPayoutPt = Math.Floor(item.MaxPayoutPt);
            }

            return data;
        }
    }
}