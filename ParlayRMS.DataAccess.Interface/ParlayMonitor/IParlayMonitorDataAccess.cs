using ParlayRMS.DTO.ParlayMonitor;
using System.Collections.Generic;

namespace ParlayRMS.DataAccess.Interface.ParlayMonitor
{
    public interface IParlayMonitorDataAccess
    {
        IEnumerable<RMS_Monitor_Parlay_Betlist> RMS_Monitor_Parlay_Betlist(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal stakeAfterPT,
            decimal maxPayout,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets);
    }
}