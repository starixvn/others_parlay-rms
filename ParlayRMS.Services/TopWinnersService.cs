using ParlayRMS.Utility;

namespace ParlayRMS.Services
{
    using System.Collections.Generic;
    using ParlayRMS.DataAccess;
    using ParlayRMS.DTO.TopWinners;
    using ParlayRMS.Models;
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

        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(TopWinnerDetailsOptions options)
        {
            var info = _commonService.GetBalanceUpDayInfo();

            return _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(options.CustId, options.FromDate, options.ToDate, info.MoveBetsDay, options.SportsType, options.Combos, options.ParlayTypes, options.LiveType, options.Currencies);
        }

        public string GetTotalComboBets(RMS_Rpt_Parlay_TopWinner_Detail detail)
        {
            var betText = detail.TotalCombo > 1 ? " bets" : " bet";
            var parlayText = string.Empty;
            if (detail.IsParlayGenerator)
            {
                parlayText += " (PG)";
            }

            if (detail.IsBoostBonus)
            {
                parlayText += " (B)";
            }

            if (detail.ParlayType == (int)ParlayType.SingleMatchParlay)
            {
                parlayText += " (S)";
            }

            if (detail.ParlayType == (int)ParlayType.PenaltyShootoutCombo)
            {
                parlayText = " (P)";
            }

            return $"{Formatter.FormatNumber3(detail.TotalCombo)} {betText} {parlayText}";
        }
    }
}
