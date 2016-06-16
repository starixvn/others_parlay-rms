using ParlayRMS.DataAccess.Implement.PotentialWinningMonitor;
using ParlayRMS.DataAccess.Interface.PotentialWinningMonitor;
using ParlayRMS.DTO.PotentialWinning;
using ParlayRMS.Services.Interface.PotentialWinningMonitor;
using ParlayRMS.Utility;
using System.Collections.Generic;

namespace ParlayRMS.Services.Implement.PotentialWinningMonitor
{
    public class PotentialWinningMonitorService : BaseService, IPotentialWinningMonitor
    {
        private readonly IPotentialWinningMonitorDataAccess _potentialWinningDataAccess = new PotentialWinningMonitorDataAccess();

        public IEnumerable<RMS_Monitor_Potential_Winning> GetPotentialWinningList(
            string combos,
            string parlayTypes,
            string sportTypes,
            string levels,
            decimal winningAmount,
            int selectTop)
        {
            return _potentialWinningDataAccess.RMS_Monitor_Parlay_PotentialWinning(combos, parlayTypes, sportTypes, levels, winningAmount, selectTop);
        }

        public IEnumerable<RMS_Monitor_Potential_Winning_Detail> GetPotentialWinningDetailList(
            string combos,
            string parlayTypes,
            string sportTypes,
            long custId)
        {
            var result = _potentialWinningDataAccess.RMS_Monitor_Parlay_PotentialWinningDetail(combos, parlayTypes, sportTypes, custId);
            foreach (var item in result)
            {
                item.SportTypeName = CacheRef.GetSportNameDropDown(item.SportType);
            }

            return result;
        }
    }
}