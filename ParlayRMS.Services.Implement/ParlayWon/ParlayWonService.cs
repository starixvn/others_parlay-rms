using ParlayRMS.DataAccess.Implement.ParlayWon;
using ParlayRMS.DTO.ParlayWon;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.ViewModel.Report;
using System.Collections.Generic;

namespace ParlayRMS.Services.Implement.ParlayWon
{
    public class ParlayWonService : BaseService
    {
        private readonly ParlayWonDataAccess _dataAccess = new ParlayWonDataAccess();
        private readonly CommonService _commonService = new CommonService();

        public IEnumerable<RMS_Rpt_Parlay_ParlayWon> GetParlayWonList(ParlayWonOptions options)

        {
            var info = _commonService.GetBalanceUpDayInfo();

            return _dataAccess.RMS_Rpt_Parlay_ParlayWon(
                options.FromDate,
                options.ToDate,
                info.MoveBetsDay,
                options.ParlayTypes,
                options.SportsType,
                options.Combos,
                options.Levels,
                options.HouseAmountFrom,
                options.HouseAmountTo);
        }
    }
}
