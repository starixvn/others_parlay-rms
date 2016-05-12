using ParlayRMS.DataAccess.Implement.Winloss;
using ParlayRMS.DataAccess.Interface.Winloss;
using ParlayRMS.DTO.Winloss;
using ParlayRMS.Models;
using ParlayRMS.Models.Winloss;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.Services.Interface;
using ParlayRMS.Services.Interface.Winloss;
using ParlayRMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParlayRMS.Services.Implement.Winloss
{
    public class WinlossService : IWinlossService
    {
        private readonly IWinlossDataAccess _winlossDataAccess = new WinlossDataAccess();
        private readonly ICommonService _commonService = new CommonService();

        public WinlossData GetWinloss(
                               DateTime fromDate,
                               DateTime toDate,
                               string parlayTypes,
                               SportsType sportsType,
                               string combos)
        {
            var info = _commonService.GetBalanceUpDayInfo();
            var data = _winlossDataAccess.RMS_Rpt_Parlay_Winloss(
                                fromDate,
                                toDate,
                                info.MoveBetsDay,
                                info.BalanceUpDay,
                                parlayTypes,
                                (int)sportsType,
                                combos);

            Utils.Round(data);
            data.ToList().ForEach(x =>
            {
                x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose;
                x.Combo = (x.Combo == "10") ? string.Format("{0} UP", x.Combo) : x.Combo;
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