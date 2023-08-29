namespace ParlayRMS.Services.TopWinners
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Common;
    using DataAccess.TopWinners;
    using ParlayRMS.DTO.TopWinners;
    using ParlayRMS.ViewModel.Report;

    public class TopWinnersService : BaseService
    {
        private readonly TopWinnersDataAccess _topwinnersDataAccess = new TopWinnersDataAccess();
        private readonly CommonService _commonService = new CommonService();

        public IEnumerable<RMS_Rpt_Parlay_TopWinner> GetTopWinners(TopWinnersOptions options)
        {
            var info = _commonService.GetBalanceUpDayInfo();
            return _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners(
                options.FromDate,
                options.ToDate,
                info.MoveBetsDay,
                info.BalanceUpDay,
                options.SportsType,
                options.SelectTop,
                options.Combos,
                options.ParlayTypes,
                options.LiveType,
                options.Currencies,
                options.Site,
                options.CustomerClass);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(TopWinnerDetailsOptions options)
        {
            var info = _commonService.GetBalanceUpDayInfo();

            return _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(options.CustId, options.FromDate, options.ToDate, info.MoveBetsDay, options.SportsType, options.Combos, options.ParlayTypes, options.LiveType, options.Currencies);
        }
    }
}