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
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
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
        private static readonly string MemberWinLossReportSessionKey = "Fanex.ParlayRMS.MemberWinLossReport";

        private readonly ICache _cacheService = CacheService.CreateInstance(CacheType.MemoryCache);
        private readonly IMemberWinLossDataAccess _memberWinLossDataAccess = new MemberWinLossDataAccess();
        private readonly ICommonService _commonService = new CommonService();
        private readonly IUsersService _usersService = new UsersService();

        /// <summary>
        /// Get Member Win Loss data.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combos value.</param>
        /// <param name="username">Username value.</param>
        /// <param name="isCached">Whether the data is cached or not.</param>
        /// <param name="roleId">Role ID of user.</param>
        /// <returns></returns>
        public MemberWinLossData GetMemberWinLoss(
                                     DateTime fromDate,
                                     DateTime toDate,
                                     BetType betType,
                                     SportsType sportsType,
                                     string combos,
                                     string username,
                                     bool isCached,
                                     out int roleId)
        {

            var user = _usersService.GetCustomerInfo(username);
            MemberWinLossData result = null;
            roleId = user.RoleId;

            if (isCached)
            {
                result = _cacheService.Get<MemberWinLossData>(MemberWinLossReportSessionKey);
            }

            if (result == null)
            {
                var info = _commonService.GetBalanceUpDayInfo();

                if (user.RoleId == (int)RoleId.Member)
                {
                    result = GetMemberWinLossData(
                                 fromDate,
                                 toDate,
                                 info.MoveBetsDay,
                                 info.BalanceUpDay,
                                 (int)betType,
                                 (int)sportsType,
                                 combos,
                                 user.CustId);
                    _cacheService.Set<MemberWinLossData>(MemberWinLossReportSessionKey, result);
                }
                else if (user.RoleId == (int)RoleId.Agent)
                {
                    result = GetAgentWinLossData(
                                 fromDate,
                                 toDate,
                                 info.MoveBetsDay,
                                 info.BalanceUpDay,
                                 (int)betType,
                                 (int)sportsType,
                                 combos,
                                 user.CustId);
                    _cacheService.Set<MemberWinLossData>(MemberWinLossReportSessionKey, result);
                }
            }

            result = result ?? new MemberWinLossData();
            return result;
        }

        /// <summary>
        /// Get member win loss data for Member Win Loss report.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="combos">Combos value.</param>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Member Win Loss data.</returns>
        private MemberWinLossData GetMemberWinLossData(
                                      DateTime fromDate,
                                      DateTime toDate,
                                      DateTime moveBetsDay,
                                      DateTime balanceUpDay,
                                      int betType,
                                      int sportsType,
                                      string combos,
                                      long custId)
        {
            List<RMS_Rpt_Parlay_MemberWinLoss> records = new List<RMS_Rpt_Parlay_MemberWinLoss>();
            RMS_Rpt_Parlay_MemberWinLoss total = new RMS_Rpt_Parlay_MemberWinLoss();

            records = this._memberWinLossDataAccess.RMS_Rpt_Parlay_MemberWinLoss(
                                                       fromDate,
                                                       toDate,
                                                       moveBetsDay,
                                                       balanceUpDay,
                                                       betType,
                                                       sportsType,
                                                       combos,
                                                       custId).ToList();

            Utils.Round(records);
            records.ToList().ForEach(x =>
            {
                x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose;
                x.Month = ReFormatMonthStr(x.Month.Trim());
                total.Add(x);
            });

            return new MemberWinLossData
            {
                Records = records,
                Total = total
            };
        }

        /// <summary>
        /// Get member win loss data for Agent Win Loss report.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="betType">Bet type ID.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="combos">Combos value.</param>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Member Win Loss data.</returns>
        private MemberWinLossData GetAgentWinLossData(
                                      DateTime fromDate,
                                      DateTime toDate,
                                      DateTime moveBetsDay,
                                      DateTime balanceUpDay,
                                      int betType,
                                      int sportsType,
                                      string combos,
                                      long custId)
        {
            List<RMS_Rpt_Parlay_MemberWinLoss> rawData = new List<RMS_Rpt_Parlay_MemberWinLoss>();
            RMS_Rpt_Parlay_MemberWinLoss total = new RMS_Rpt_Parlay_MemberWinLoss();
            

            rawData = this._memberWinLossDataAccess.RMS_Rpt_Parlay_AgentWinLoss(
                                                       fromDate,
                                                       toDate,
                                                       moveBetsDay,
                                                       balanceUpDay,
                                                       betType,
                                                       sportsType,
                                                       combos,
                                                       custId).ToList();

            Utils.Round(rawData);
            rawData.ToList().ForEach(x =>
            {
                x.OurPT_NetWinloss = x.OurPT_Won + x.OurPT_Lose;
                x.Month = ReFormatMonthStr(x.Month.Trim());
                total.Add(x);
            });

            return new MemberWinLossData
            {
                Records = GroupAgentDataByMonth(rawData),
                Total = total
            };
        }

        /// <summary>
        /// Group raw data of agent win loss report by month
        /// </summary>
        /// <param name="rawData">Raw data queried from DB.</param>
        /// <returns>Data for agent win loss report that are grouped by month.</returns>
        private List<RMS_Rpt_Parlay_MemberWinLoss> GroupAgentDataByMonth(List<RMS_Rpt_Parlay_MemberWinLoss> rawData)
        {
            List<RMS_Rpt_Parlay_MemberWinLoss> records = new List<RMS_Rpt_Parlay_MemberWinLoss>();
            RMS_Rpt_Parlay_MemberWinLoss currentRecord = new RMS_Rpt_Parlay_MemberWinLoss();
            RMS_Rpt_Parlay_MemberWinLoss subtotal = new RMS_Rpt_Parlay_MemberWinLoss();

            int currentRecordIndex = 0;
            int firstRecordIndex = 0;
            int numOfRecords = rawData.Count;

            if (numOfRecords > 0)
            {
                while (currentRecordIndex < numOfRecords)
                {
                    currentRecord = rawData[currentRecordIndex];
                    if (!currentRecord.Month.Equals(subtotal.Month, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(subtotal.Month))
                        {
                            records[firstRecordIndex].NumOfSubrows = subtotal.NumOfSubrows;
                            records.Add(subtotal);
                            firstRecordIndex = records.Count;
                        }

                        subtotal = new RMS_Rpt_Parlay_MemberWinLoss
                        {
                            Month = currentRecord.Month,
                            IsSutotalRow = true
                        };
                    }

                    subtotal.Add(currentRecord);
                    records.Add(currentRecord);
                    currentRecordIndex++;
                }

                if (!string.IsNullOrEmpty(subtotal.Month))
                {
                    records[firstRecordIndex].NumOfSubrows = subtotal.NumOfSubrows;
                    records.Add(subtotal);
                }
            }

            return records;
        }

        /// <summary>
        /// Format month string from '2013-01' to '01-2013'.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string ReFormatMonthStr(string date)
        {
            int separatorIndex = date.IndexOf('-');
            string year = date.Substring(0, separatorIndex);
            string month = date.Substring(separatorIndex + 1, 2);

            return string.Format("{0}-{1}", month, year);
        }
    }
}
