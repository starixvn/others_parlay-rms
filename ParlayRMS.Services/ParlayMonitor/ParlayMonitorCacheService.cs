namespace ParlayRMS.Services.ParlayMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.ParlayMonitor;
    using DTO.PotentialWinning;

    public class ParlayMonitorCacheService : BaseService
    {
        private static readonly ParlayMonitorService ParlayService = new ParlayMonitorService();

        public IEnumerable<RMS_Monitor_Parlay_Betlist> GetParlayMonitorTickets(
            string combos,
            string parlayTypes,
            string sportsTypes,
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
            return ParlayService.GetParlayMonitorTickets(combos, parlayTypes, sportsTypes, levels, stakeAfterPT, netWinAfterPT, reportType, selectTop, maxTransId, markets, isCornerParlay, odds, leagueGroups, customerClass, company, username);
        }

        public UserTracking UpdateCheckedUser(int userid, string username, string transid)
        {
            return ParlayService.UpdateCheckedUser(userid, username, transid);
        }
    }
}
