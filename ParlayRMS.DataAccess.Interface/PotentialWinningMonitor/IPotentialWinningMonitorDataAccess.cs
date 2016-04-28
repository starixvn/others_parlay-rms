using ParlayRMS.DTO.PotentialWinning;
using System.Collections.Generic;

namespace ParlayRMS.DataAccess.Interface.PotentialWinningMonitor
{
    public interface IPotentialWinningMonitorDataAccess
    {
        IEnumerable<RMS_Monitor_Potential_Winning> RMS_Monitor_Parlay_PotentialWinning(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop);

        IEnumerable<RMS_Monitor_Potential_Winning_Detail> RMS_Monitor_Parlay_PotentialWinningDetail(
            string combos,
            string parlayTypes,
            string sportsTypes,
            long custId);
    }
}