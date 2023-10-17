namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria.TopWinners;
    using DTO.BetList;
    using DTO.TopWinners;

    public class TopWinnersDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();
        public IEnumerable<RMS_Rpt_Parlay_TopWinner> RMS_Rpt_Parlay_TopWinners(
                                                         DateTime fromDate,
                                                         DateTime toDate,
                                                         DateTime moveBetsDay,
                                                         DateTime balanceUpDay,
                                                         string sportsType,
                                                         int selectTop,
                                                         string combos,
                                                         string parlayTypes,
                                                         byte liveType,
                                                         string currencies,
                                                         byte site,
                                                         int? customerClass)
        {
            var criteria = new Rpt_ComboParlay_TopWinners_Criteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                MoveBetsDay = moveBetsDay,
                BalanceUpDay = balanceUpDay,
                Sporttype = sportsType,
                LimitedRow = selectTop,
                Selection = combos,
                ParlayType = parlayTypes,
                LiveStatus = liveType,
                CustomerClass = customerClass,
                Site = site,
                Currency = currencies
            };

            var list = repository.Fetch<RMS_Rpt_Parlay_TopWinner>(criteria).ToList();
            list.ForEach(l =>
            {
                l.CustomerClass = string.IsNullOrWhiteSpace(l.CustomerClass) ? "None" : l.CustomerClass;
            });

            return list;
        }

        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details(
                                                                int custId,
                                                                DateTime fromDate,
                                                                DateTime toDate,
                                                                DateTime moveBetsDay,
                                                                string sportsType,
                                                                string combos,
                                                                string parlayTypes,
                                                                byte liveType,
                                                                string currencies)
        {
            var criteria = new Rpt_ComboParlay_TopWinnersDetails_Criteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                MoveBetsDay = moveBetsDay,
                Sporttype = sportsType,
                Selection = combos,
                ParlayType = parlayTypes,
                LiveStatus = liveType,
                Currency = currencies,
                CustId = custId
            };

            return repository.Fetch<RMS_Rpt_Parlay_TopWinner_Detail>(criteria);
        }

        public IEnumerable<RMS_Parlay_TransDetails> RMS_Rpt_Parlay_TopWinners_Bet_Details(long transId, long refNo)
        {
            var criteria = new Rpt_Parlay_TopWinners_Bet_Details_Criteria
            {
                TransId = transId,
                RefNo = refNo
            };
            return repository.Fetch<RMS_Parlay_TransDetails>(criteria);
        }

        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details_old(
                                                                int custId,
                                                                DateTime fromDate,
                                                                DateTime toDate,
                                                                string sportsType,
                                                                string combos,
                                                                string parlayTypes,
                                                                string currencies)
        {
            var criteria = new Rpt_ComboParlay_TopWinnersDetails_Old_Criteria
            {
                CustId = custId,
                FromDate = fromDate,
                ToDate = toDate,
                Selection = combos,
                ParlayType = parlayTypes,
                Currency = currencies,
                Sporttype = sportsType
            };

            return repository.Fetch<RMS_Rpt_Parlay_TopWinner_Detail>(criteria);
        }
    }
}
