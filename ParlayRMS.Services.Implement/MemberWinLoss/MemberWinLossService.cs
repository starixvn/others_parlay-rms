// --------------------------------------------------------------------------
// <copyright file="MemberWinLossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.Services.Implement.MemberWinLoss
{
    using System;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.MemberWinLoss;
    using ParlayRMS.DataAccess.Interface.MemberWinLoss;
    using ParlayRMS.DTO.MemberWinLoss;
    using ParlayRMS.Models;
    using ParlayRMS.Models.MemberWinLoss;
    using ParlayRMS.Services.Implement.Common;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.MemberWinLoss;
    using ParlayRMS.Utility;

    /// <summary>
    /// Member Win Loss Service.
    /// </summary>
    public class MemberWinLossService : IMemberWinLossService
    {
        private readonly IMemberWinLossDataAccess memberWinLossDataAccess = new MemberWinLossDataAccess();
        private readonly ICommonService commonService = new CommonService();

        /// <summary>
        /// Get Member Win Loss data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combos value.</param>
        /// <param name="username">Username.</param>
        /// <returns></returns>
        public MemberWinLossData GetMemberWinLoss(DateTime fromDate,
                                                  DateTime toDate,
                                                  BetType betType,
                                                  SportsType sportsType,
                                                  string combos,
                                                  string username)
        {
            var info = commonService.GetBalanceUpDayInfo();
            var records = this.memberWinLossDataAccess.RMS_Rpt_Parlay_MemberWinloss(fromDate,
                                                                                    toDate,
                                                                                    info.MoveBetsDay,
                                                                                    info.BalanceUpDay,
                                                                                    (int)betType,
                                                                                    (int)sportsType,
                                                                                    combos,
                                                                                    username);
            Utils.Round(records);
            records.ToList().ForEach(x => x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose);

            var total = new RMS_Rpt_Parlay_MemberWinLoss
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

            return new MemberWinLossData
            {
                Records = records,
                Total = total
            };
        }
    }
}
