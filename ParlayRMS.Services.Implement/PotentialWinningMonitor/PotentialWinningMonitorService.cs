using System;
using System.Collections.Generic;
using ParlayRMS.DataAccess.Implement.PotentialWinningMonitor;
using ParlayRMS.DTO.PotentialWinning;
using ParlayRMS.Utility;

namespace ParlayRMS.Services.Implement.PotentialWinningMonitor
{
    public class PotentialWinningMonitorService : BaseService
    {
        private readonly PotentialWinningMonitorDataAccess _potentialWinningDataAccess = new PotentialWinningMonitorDataAccess();

        public IEnumerable<RMS_Monitor_Potential_Winning> GetPotentialWinningList(
            string combos,
            string parlayTypes,
            string sportTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            bool isCornerParlay,
            byte filterBy,
            int? agentId,
            Int64? customerClass,
            string company,
            string username)
        {
            return _potentialWinningDataAccess.RMS_Monitor_Parlay_PotentialWinning(combos, parlayTypes, sportTypes, levels, winningAmount, selectTop, isCornerParlay, filterBy, agentId, customerClass, company, username);
        }

        public IEnumerable<RMS_Monitor_Potential_Winning_Detail> GetPotentialWinningDetailList(
            string combos,
            string parlayTypes,
            string sportTypes,
            long custId,
            bool isCornerParlay)
        {
            var result = _potentialWinningDataAccess.RMS_Monitor_Parlay_PotentialWinningDetail(combos, parlayTypes, sportTypes, custId, isCornerParlay);
            foreach (var item in result)
            {
                item.SportTypeName = CacheRef.GetSportNameDropDown(item.SportType);
                item.IsTrackedUser = item.TrackingUser != "None";
            }

            return result;
        }

        public UserTracking UpdateCheckedUser(int userid, string username, string transid)
        {
            return _potentialWinningDataAccess.RMS_ComboParlay_PotentialWinning_UserTracking(userid, username, transid);
        }
    }
}