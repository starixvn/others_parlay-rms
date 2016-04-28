using ParlayRMS.DTO.PotentialWinning;
using System.Collections.Generic;

namespace ParlayRMS.Services.Interface.PotentialWinningMonitor
{
    public interface IPotentialWinningMonitor
    {
        IEnumerable<RMS_Monitor_Potential_Winning> GetPotentialWinningList(
            string combos,
            string parlayTypes,
            string sportTypes,
            string levels,
            decimal winningAmount,
            int selectTop);

        IEnumerable<RMS_Monitor_Potential_Winning_Detail> GetPotentialWinningDetailList(
            string combos,
            string parlayTypes,
            string sportTypes,
            long custId);
    }
}