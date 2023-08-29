namespace ParlayRMS.Services.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using DataAccess.Winloss;
    using ParlayRMS.DTO.Winloss;
    using ParlayRMS.Models.Winloss;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Report;

    public class WinlossService
    {
        private readonly WinlossDataAccess _winlossDataAccess = new WinlossDataAccess();
        private readonly CommonService _commonService = new CommonService();

        public WinlossData GetWinloss(WinlossOptions options)
        {
            var info = _commonService.GetBalanceUpDayInfo();
            var data = _winlossDataAccess.RMS_Rpt_Parlay_Winloss(
                                options.FromDate,
                                options.ToDate,
                                info.MoveBetsDay,
                                info.BalanceUpDay,
                                options.ParlayTypes,
                                options.SportsType,
                                options.Combos,
                                (int)options.ReportType,
                                options.Site,
                                options.LiveType);

            Utils.Round(data);
            data.ToList().ForEach(x =>
            {
                x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose;
                x.Combo = (x.Combo == "10") ? string.Format("{0} UP", x.Combo) : x.Combo;
                x.Margin = x.OurPT_NetWinloss / (x.Stake_Won + x.Stake_Lose);
            });

            return GroupData(data);
        }

        /// <summary>
        /// Groups raw data by date.
        /// </summary>
        /// <param name="data">Raw data that need to be grouped.</param>
        /// <returns>Group data.</returns>
        private WinlossData GroupData(IEnumerable<RMS_Rpt_Parlay_Winloss> data)
        {
            WinlossData records = new WinlossData();
            WinlossItemData currentDateData = null;
            DateTime currentDate = new DateTime(1970, 1, 1);

            data.ToList().ForEach(x =>
            {
                if (!currentDate.Equals(x.WinlostDate))
                {
                    records.Add(currentDateData);
                    currentDateData = new WinlossItemData();
                    currentDate = x.WinlostDate;
                }

                currentDateData.Add(x);
            });
            records.Add(currentDateData);

            return records;
        }
    }
}