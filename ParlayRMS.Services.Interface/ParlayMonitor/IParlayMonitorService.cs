using ParlayRMS.DTO.ParlayMonitor;
using System.Collections.Generic;

namespace ParlayRMS.Services.Interface.ParlayMonitor
{
    public interface IParlayMonitorService
    {
        IEnumerable<RMS_Monitor_Parlay_Betlist> GetParlayMonitorTickets(
            string combos,
            string parlayTypes,
            string sportTypes,
            string levels,
            decimal stakeAfterPT,
            decimal netWinAfterPT,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets);
    }
}