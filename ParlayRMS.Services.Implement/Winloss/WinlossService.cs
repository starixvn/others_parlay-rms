// -----------------------------------------------------------------------
// <copyright file="WinlossService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.Winloss
{
    using System;
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
    /// TODO: Update summary.
    /// </summary>
    public class WinlossService : IWinlossService
    {
        private readonly IWinlossDataAccess winlossDataAccess = new WinlossDataAccess();
        private readonly ICommonService commonService = new CommonService();

        public WinlossData GetWinloss(DateTime fromDate, DateTime toDate, BetType betType, SportsType sportsType, string combos)
        {
            var info = commonService.GetBalanceUpDayInfo();
            var records = this.winlossDataAccess.RMS_Rpt_Parlay_Winloss(fromDate, toDate, info.MoveBetsDay, info.BalanceUpDay, (int)betType, (int)sportsType, combos);
            Utils.Round(records);
            records.ToList().ForEach(x => x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose);

            var total = new RMS_Rpt_Parlay_Winloss
            {
                Tickets_Won = records.Sum(x => x.Tickets_Won),
                Tickets_Lose = records.Sum(x => x.Tickets_Lose),
                Stake_Won = records.Sum(x => x.Stake_Won),
                Stake_Lose = records.Sum(x => x.Stake_Lose),
                Member_Won = records.Sum(x => x.Member_Won),
                Member_Lose = records.Sum(x => x.Member_Lose),
                OurPT_Won = records.Sum(x => x.OurPT_Won),
                OurPT_Lose = records.Sum(x => x.OurPT_Lose),
                OurPT_NetWinloss = records.Sum(x => x.OurPT_NetWinloss)
            };

            return new WinlossData
            {
                Records = records,
                Total = total
            };
        }
    }
}