using ParlayRMS.DataAccess.Implement.LeagueWinningMonitor;
using ParlayRMS.DTO.LeagueWinningMonitor;
using System.Collections.Generic;

namespace ParlayRMS.Services.Implement.LeagueWinningMonitor
{
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
    }
}