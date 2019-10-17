using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Caching;
using ParlayRMS.DataAccess.Implement.MemberWinLoss;
using ParlayRMS.DTO.MemberWinLoss;
using ParlayRMS.Models;
using ParlayRMS.Models.MemberWinLoss;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.Services.Implement.Users;
using ParlayRMS.Utility;

namespace ParlayRMS.Services.Implement.MemberWinLoss
{
    public class MemberWinLossService
    {
        private static readonly string MemberWinLossReportSessionKey = "Fanex.ParlayRMS.MemberWinLossReport";

        private readonly CacheService _cacheService = new CacheServiceImpl();
        private readonly MemberWinLossDataAccess _memberWinLossDataAccess = new MemberWinLossDataAccess();
        private readonly CommonService _commonService = new CommonService();
        private readonly UsersService _usersService = new UsersService();

        public MemberWinLossData GetMemberWinLoss(
                                     DateTime fromDate,
                                     DateTime toDate,
                                     string parlayTypes,
                                     string sportsType,
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
                                 parlayTypes,
                                 sportsType,
                                 combos,
                                 user.CustId);
                    _cacheService.Set(MemberWinLossReportSessionKey, result);
                }
                else if (user.RoleId == (int)RoleId.Agent || user.RoleId == (int)RoleId.Master)
                {
                    var agentIds = user.RoleId == (int)RoleId.Agent
                        ? new List<int> { (int)user.CustId }
                        : _usersService.GetAgents((int)user.CustId);

                    result = GetAgentWinLossData(
                                 fromDate,
                                 toDate,
                                 info.MoveBetsDay,
                                 info.BalanceUpDay,
                                 parlayTypes,
                                 sportsType,
                                 combos,
                                 agentIds);
                    _cacheService.Set(MemberWinLossReportSessionKey, result);
                }
            }

            result = result ?? new MemberWinLossData();
            return result;
        }

        private MemberWinLossData GetMemberWinLossData(
                                      DateTime fromDate,
                                      DateTime toDate,
                                      DateTime moveBetsDay,
                                      DateTime balanceUpDay,
                                      string parlayTypes,
                                      string sportsType,
                                      string combos,
                                      long custId)
        {
            List<RMS_Rpt_Parlay_MemberWinLoss> records = new List<RMS_Rpt_Parlay_MemberWinLoss>();
            RMS_Rpt_Parlay_MemberWinLoss total = new RMS_Rpt_Parlay_MemberWinLoss();

            records = _memberWinLossDataAccess.RMS_Rpt_Parlay_MemberWinLoss(
                                                       fromDate,
                                                       toDate,
                                                       moveBetsDay,
                                                       balanceUpDay,
                                                       parlayTypes,
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

        private MemberWinLossData GetAgentWinLossData(
                                      DateTime fromDate,
                                      DateTime toDate,
                                      DateTime moveBetsDay,
                                      DateTime balanceUpDay,
                                      string parlayTypes,
                                      string sportsType,
                                      string combos,
                                      List<int> custIds)
        {
            RMS_Rpt_Parlay_MemberWinLoss total = new RMS_Rpt_Parlay_MemberWinLoss();

            var rawData = _memberWinLossDataAccess.RMS_Rpt_Parlay_AgentWinLoss(
                                                       fromDate,
                                                       toDate,
                                                       moveBetsDay,
                                                       balanceUpDay,
                                                       parlayTypes,
                                                       sportsType,
                                                       combos,
                                                       custIds).ToList();

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

        private string ReFormatMonthStr(string date)
        {
            int separatorIndex = date.IndexOf('-');
            string year = date.Substring(0, separatorIndex);
            string month = date.Substring(separatorIndex + 1, 2);

            return string.Format("{0}-{1}", month, year);
        }
    }
}