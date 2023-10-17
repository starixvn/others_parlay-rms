namespace ParlayRMS.Services
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess;
    using ParlayRMS.DTO.ParlayMonitor;
    using ParlayRMS.DTO.PotentialWinning;
    using ParlayRMS.Utility;

    public class ParlayMonitorService : BaseService
    {
        private readonly ParlayMonitorDataAccess _parlayDataAccess = new ParlayMonitorDataAccess();

        public IEnumerable<RMS_Monitor_Parlay_Betlist> GetParlayMonitorTickets(
            string combos,
            string parlayTypes,
            string sportTypes,
            string levels,
            decimal stakeAfterPT,
            decimal netWinAfterPT,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets,
            bool isCornerParlay,
            decimal? odds,
            string leagueGroups,
            int? customerClass,
            string company,
            string username)
        {
            var data = _parlayDataAccess.RMS_Monitor_Parlay_Betlist(combos, parlayTypes, sportTypes, levels, stakeAfterPT, netWinAfterPT, reportType, selectTop, maxTransId, markets, isCornerParlay, odds, leagueGroups, customerClass, company, username);
            foreach (var item in data)
            {
                item.Stake = Math.Round(item.Stake, 2);
                item.NetWinAfterPT = Math.Floor(item.NetWinAfterPT);
                item.SportTypeName = CacheRef.GetSportNameDropDown(item.SportType);
                item.IsTrackedUser = item.TrackingUser != "None";
            }

            return data;
        }

        public UserTracking UpdateCheckedUser(int userid, string username, string transid)
        {
            return _parlayDataAccess.RMS_Monitor_Parlay_UserTracking(userid, username, transid);
        }
    }
}
