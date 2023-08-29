namespace ParlayRMS.Services.ParlayWon
{
    using System.Collections;
    using System.Collections.Generic;
    using Common;
    using DataAccess.ParlayWon;
    using ParlayRMS.DTO.ParlayWon;
    using ParlayRMS.ViewModel.Report;

    public class ParlayWonService : BaseService
    {
        private readonly ParlayWonDataAccess _dataAccess = new ParlayWonDataAccess();
        private readonly CommonService _commonService = new CommonService();

        public IEnumerable<RMS_Rpt_Parlay_ParlayWon> GetParlayWonList(ParlayWonOptions options, ref Hashtable outputParams)
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
                options.HouseAmountTo,
                options.SortById,
                options.IsSortAsc,
                options.PageIndex,
                options.PageSize,
                ref outputParams,
                options.Currencies,
                options.Site,
                options.LiveType,
                options.CustomerClass);
        }
    }
}