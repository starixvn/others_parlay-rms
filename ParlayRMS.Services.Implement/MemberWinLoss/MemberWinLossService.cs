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
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.MemberWinLoss;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;

    /// <summary>
    /// Member Win Loss Service.
    /// </summary>
    public class MemberWinLossService : IMemberWinLossService
    {
        private readonly IMemberWinLossDataAccess memberWinLossDataAccess = new MemberWinLossDataAccess();
        private readonly ICommonService commonService = new CommonService();
        private readonly IUsersService usersService = new UsersService();

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
            var user = usersService.GetCustomerInfo(username);
            var records = this.memberWinLossDataAccess.RMS_Rpt_Parlay_MemberWinloss(fromDate,
                                                                                    toDate,
                                                                                    info.MoveBetsDay,
                                                                                    info.BalanceUpDay,
                                                                                    (int)betType,
                                                                                    (int)sportsType,
                                                                                    combos,
                                                                                    user.CustId);
            Utils.Round(records);
            records.ToList().ForEach(x => { x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose; 
                                            x.Month = ReFormatMonthStr(x.Month.Trim()); 
            });

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

        /// <summary>
        /// Format month string from '2013-01' to '01-2013'
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string ReFormatMonthStr(string date)
        {
            int separatorIndex = date.IndexOf('-');
            string year = date.Substring(0, separatorIndex);
            string month = date.Substring(separatorIndex + 1, 2);

            return string.Format("{0}-{1}", month, year);
        }
    }
}
