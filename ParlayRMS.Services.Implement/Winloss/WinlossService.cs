// -----------------------------------------------------------------------
// <copyright file="WinlossService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.Winloss;
    using ParlayRMS.DataAccess.Interface.Winloss;
    using ParlayRMS.DTO.Winloss;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Winloss;
    using ParlayRMS.Services.Implement.Common;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.Winloss;
    using ParlayRMS.Utility;

    /// <summary>
    /// Win Loss service.
    /// </summary>
    public class WinlossService : IWinlossService
    {
        private readonly IWinlossDataAccess winlossDataAccess = new WinlossDataAccess();
        private readonly ICommonService commonService = new CommonService();

        /// <summary>
        /// Get Win Loss data.
        /// </summary>
        /// <param name="fromDate">From Date value.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Win Loss data.</returns>
        public WinlossData GetWinloss(
                               DateTime fromDate, 
                               DateTime toDate, 
                               BetType betType, 
                               SportsType sportsType, 
                               string combos)
        {
            var info = commonService.GetBalanceUpDayInfo();
            var data = this.winlossDataAccess.RMS_Rpt_Parlay_Winloss(
                                fromDate, 
                                toDate, 
                                info.MoveBetsDay, 
                                info.BalanceUpDay, 
                                (int)betType, 
                                (int)sportsType,
                                combos);
            

            Utils.Round(data);
            data.ToList().ForEach(x =>
            {
                x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose;
                x.Combo = (x.Combo == "6") ? string.Format("{0} & up", x.Combo) : x.Combo;
            });
            ////data.OrderBy(x => x.WinlostDate);

            return GroupData(data);
        }

        /// <summary>
        /// Group raw data by Date
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