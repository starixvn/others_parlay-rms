namespace ParlayRMS.Services.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using DataAccess;
    using ParlayRMS.DTO.Winloss;
    using ParlayRMS.Models.Winloss;
    using Utility;
    using ViewModel.Report;

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
                x.Combo = x.Combo == "10" ? $"{x.Combo} UP" : x.Combo;
                x.Margin = x.OurPT_NetWinloss / (x.Stake_Won + x.Stake_Lose);
            });

            return GroupData(data);
        }

        private WinlossData GroupData(IEnumerable<RMS_Rpt_Parlay_Winloss> data)
        {
            var records = new WinlossData();
            WinlossItemData currentDateData = null;
            var currentDate = new DateTime(1970, 1, 1);

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
