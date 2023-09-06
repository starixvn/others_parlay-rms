namespace ParlayRMS.Services.LeagueWinningMonitor
{
    using System.Collections.Generic;
    using DataAccess;
    using ParlayRMS.DTO.LeagueWinningMonitor;

    public class LeagueWinningMonitorService : BaseService
    {
        private readonly LeagueWinningMonitorDataAccess leagueWinningMonitorDataAccess = new LeagueWinningMonitorDataAccess();

        public IEnumerable<RmsRptLeagueWinningMonitor> GetLeagueWinningMonitorList(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            byte group,
            byte reportBy)
        {
            return leagueWinningMonitorDataAccess.GetData(combos, parlayTypes, sportsTypes, levels, winningAmount, selectTop, group, reportBy);
        }

        public IEnumerable<RmsRptLeagueWinningMonitorDetail> GetLeagueWinningMonitorListDetail(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            byte group,
            byte reportBy,
            int leagueId,
            int custId)
        {
            return leagueWinningMonitorDataAccess.GetDetailList(combos, parlayTypes, sportsTypes, levels, winningAmount, selectTop, group, reportBy, leagueId, custId);
        }
    }
}
