using System.Collections.Generic;
using ParlayRMS.DTO.ParlayMonitor;

namespace ParlayRMS.Services.Implement.ParlayMonitor
{
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
            string markets)
        {
            return ParlayService.GetParlayMonitorTickets(combos, parlayTypes, sportsTypes, levels, stakeAfterPT, netWinAfterPT, reportType, selectTop, maxTransId, markets);
        }
    }
}