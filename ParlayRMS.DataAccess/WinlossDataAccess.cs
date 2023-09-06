namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria.Winloss;
    using DTO.Winloss;
    using Models;

    public class WinlossDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();
        public IEnumerable<RMS_Rpt_Parlay_Winloss> RMS_Rpt_Parlay_Winloss(
                                                       DateTime fromDate,
                                                       DateTime toDate,
                                                       DateTime moveBetsDay,
                                                       DateTime balanceUpDay,
                                                       string parlayTypes,
                                                       string sportsType,
                                                       string combos,
                                                       int reportType,
                                                       byte site,
                                                       byte liveType)
        {
            var criteria = new Rpt_ComboParlay_Winloss_Criteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                MoveBetsDay = moveBetsDay,
                BalanceUpDay = balanceUpDay,
                ParlayType = parlayTypes,
                Sporttype = sportsType,
                Selection = combos,
                ReportType = reportType,
                Site = site,
                LiveStatus = liveType
            };

            var list = repository.Fetch<RMS_Rpt_Parlay_Winloss>(criteria).ToList();
            list.ForEach(l =>
            {
                l.Combo = reportType == (int)ReportType.Combo ? l.Combo : string.Empty;
                l.SportType = reportType == (int)ReportType.SportType ? l.SportType : 0;
            });

            return list;
        }
    }
}
