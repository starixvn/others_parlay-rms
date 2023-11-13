namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria.ParlayWon;
    using DTO.ParlayWon;

    public class ParlayWonDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<RMS_Rpt_Parlay_ParlayWon> RMS_Rpt_Parlay_ParlayWon(
                                                       DateTime fromDate,
                                                       DateTime toDate,
                                                       DateTime moveBetsDay,
                                                       string parlayTypes,
                                                       string sportsType,
                                                       string combos,
                                                       string level,
                                                       decimal houseAmountFrom,
                                                       decimal houseAmountTo,
                                                       int sortById,
                                                       bool isSortAsc,
                                                       int pageIndex,
                                                       int pageSize,
                                                       string currency,
                                                       byte site,
                                                       byte liveType,
                                                       int? customerClass,
                                                       string ip,
                                                       out int totalRows)
        {
            var criteria = new Rpt_ParlayWon_Criteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                MoveBetsDay = moveBetsDay,
                ParlayType = parlayTypes,
                Selection = combos,
                Sporttype = sportsType,
                Level = level,
                FromHouseAmt = houseAmountFrom,
                ToHouseAmt = houseAmountTo,
                SortById = sortById,
                IsSortAsc = isSortAsc,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Currency = currency,
                Site = site,
                LiveStatus = liveType,
                CustomerClass = customerClass,
                Ip = ip
            };

            var list = repository.Fetch<RMS_Rpt_Parlay_ParlayWon>(criteria).ToList();
            list.ForEach(l =>
            {
                l.CustomerClass = string.IsNullOrWhiteSpace(l.CustomerClass)?"None":l.CustomerClass;
            });

            totalRows = criteria.TotalRows;

            return list;
        }
    }
}
