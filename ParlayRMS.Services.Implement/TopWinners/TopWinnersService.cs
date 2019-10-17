using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ParlayRMS.DataAccess.Implement.TopWinners;
using ParlayRMS.DTO.BetList;
using ParlayRMS.DTO.TopWinners;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.ViewModel.Report;

namespace ParlayRMS.Services.Implement.TopWinners
{
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
                options.ParlayTypes);
        }

        public IEnumerable<RMS_Parlay_TransDetails> GetBetDetail(long transId, long refNo)
        {
            return _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Bet_Details(transId, refNo);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(TopWinnerDetailsOptions options)
        {
            var info = _commonService.GetBalanceUpDayInfo();

            if (info.BalanceUpDay.CompareTo(options.FromDate) <= 0 && info.BalanceUpDay.CompareTo(options.ToDate) <= 0)
            {
                return _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(options.CustId, options.FromDate, options.ToDate, info.MoveBetsDay, options.SportsType, options.Combos, options.ParlayTypes);
            }
            else if (info.BalanceUpDay.CompareTo(options.FromDate) > 0 && info.BalanceUpDay.CompareTo(options.ToDate) > 0)
            {
                return _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details_old(options.CustId, options.FromDate, options.ToDate, options.SportsType, options.Combos, options.ParlayTypes);
            }
            else
            {
                IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> listEntity_old = null;
                IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> listEntity = null;

                listEntity_old = _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details_old(options.CustId, options.FromDate, info.BalanceUpDay.AddDays(-1), options.SportsType, options.Combos, options.ParlayTypes);
                listEntity = _topwinnersDataAccess.RMS_Rpt_Parlay_TopWinners_Details(options.CustId, info.BalanceUpDay, options.ToDate, info.MoveBetsDay, options.SportsType, options.Combos, options.ParlayTypes);

                if (listEntity_old.Count() != 0 && listEntity.Count() != 0)
                {
                    return listEntity.Union(listEntity_old);
                }
                else if (listEntity_old.Count() != 0 && listEntity.Count() <= 0)
                {
                    return listEntity_old;
                }
                else
                {
                    return listEntity;
                }
            }
        }

        public BetInfo GetBetInfo(long transId)
        {
            return _topwinnersDataAccess.GetBetInfo(transId);
        }
    }
}