namespace ParlayRMS.Services.MemberWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using DataAccess;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Members;
    using ParlayRMS.DTO.MemberWinLoss;
    using Models;
    using ParlayRMS.Models.MemberWinLoss;
    using Utility;

    public class MemberWinLossService
    {
        private const string MemberWinLossReportSessionKey = "Fanex.ParlayRMS.MemberWinLossReport";

        private static readonly ICache CacheInstance = new MemoryCache(
            new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(30),
                CompactionPercentage = 0.15,
                CompactOnMemoryPressure = true
            }
        );
        private readonly MemberWinLossDataAccess _memberWinLossDataAccess = new MemberWinLossDataAccess();
        private readonly CommonService _commonService = new CommonService();
        private readonly MembersService _membersService = new MembersService();

        private const byte SORT_BY_USERNAME = 1;
        private const byte SORT_BY_CUSTOMERCLASS = 2;
        private const byte SORT_BY_TICKET_WON = 3;
        private const byte SORT_BY_TICKET_LOSE = 4;
        private const byte SORT_BY_STAKE_WON = 5;
        private const byte SORT_BY_STAKE_LOSE = 6;
        private const byte SORT_BY_WINLOSS_WON = 7;
        private const byte SORT_BY_WINLOSS_LOSE = 8;
        private const byte SORT_BY_HOUSEWL_WON = 9;
        private const byte SORT_BY_HOUSEWL_LOSE = 10;
        private const byte SORT_BY_HOUSEWL_NETWL = 11;


        public MemberWinLossData GetMemberWinLoss(
                                     DateTime fromDate,
                                     DateTime toDate,
                                     string parlayTypes,
                                     string sportsType,
                                     byte liveType,
                                     string combos,
                                     string username,
                                     bool isCached,
                                     out int roleId)
        {
            var user = _membersService.GetCustomerInfo(username);
            var cacheIsAvailable = CacheInstance.TryGet(MemberWinLossReportSessionKey, out MemberWinLossData memberWinLossData);
            roleId = user.RoleId;

         
            if (!isCached || !cacheIsAvailable)
            {
                var info = _commonService.GetBalanceUpDayInfo();

                if (user.RoleId == (int)RoleId.Member)
                {
                    memberWinLossData = GetMemberWinLossData(
                                 fromDate,
                                 toDate,
                                 info.MoveBetsDay,
                                 info.BalanceUpDay,
                                 parlayTypes,
                                 sportsType,
                                 liveType,
                                 combos,
                                 user.CustId);
                    CacheInstance.Set(MemberWinLossReportSessionKey, memberWinLossData, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(30) });
                }
                else if (user.RoleId == (int)RoleId.Agent || user.RoleId == (int)RoleId.Master)
                {
                    var agentIds = user.RoleId == (int)RoleId.Agent
                        ? new List<int> { user.CustId }
                        : _membersService.GetAgents(user.CustId);

                    memberWinLossData = GetAgentWinLossData(
                                 fromDate,
                                 toDate,
                                 info.MoveBetsDay,
                                 info.BalanceUpDay,
                                 parlayTypes,
                                 sportsType,
                                 liveType,
                                 combos,
                                 agentIds);
                    CacheInstance.Set(MemberWinLossReportSessionKey, memberWinLossData, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(30) });
                }
            }

            memberWinLossData = memberWinLossData ?? new MemberWinLossData();
            return memberWinLossData;
        }

        private MemberWinLossData GetMemberWinLossData(
                                      DateTime fromDate,
                                      DateTime toDate,
                                      DateTime moveBetsDay,
                                      DateTime balanceUpDay,
                                      string parlayTypes,
                                      string sportsType,
                                      byte liveType,
                                      string combos,
                                      int custId)
        {
            var total = new RMS_Rpt_Parlay_MemberWinLoss();

            var records = _memberWinLossDataAccess.RMS_Rpt_Parlay_MemberWinLoss(
                fromDate,
                toDate,
                moveBetsDay,
                balanceUpDay,
                parlayTypes,
                sportsType,
                liveType,
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
                                      byte liveType,
                                      string combos,
                                      List<int> custIds)
        {
            var total = new RMS_Rpt_Parlay_MemberWinLoss();

            var rawData = _memberWinLossDataAccess.RMS_Rpt_Parlay_AgentWinLoss(
                                                       fromDate,
                                                       toDate,
                                                       moveBetsDay,
                                                       balanceUpDay,
                                                       parlayTypes,
                                                       sportsType,
                                                       liveType,
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
                Records = fromDate.Month == toDate.Month ? rawData : GroupAgentDataByMonth(rawData),
                Total = total
            };
        }

        private List<RMS_Rpt_Parlay_MemberWinLoss> GroupAgentDataByMonth(List<RMS_Rpt_Parlay_MemberWinLoss> rawData)
        {
            var records = new List<RMS_Rpt_Parlay_MemberWinLoss>();
            var subtotal = new RMS_Rpt_Parlay_MemberWinLoss();

            var currentRecordIndex = 0;
            var firstRecordIndex = 0;
            var numOfRecords = rawData.Count;

            if (numOfRecords > 0)
            {
                while (currentRecordIndex < numOfRecords)
                {
                    var currentRecord = rawData[currentRecordIndex];
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
            var separatorIndex = date.IndexOf('-');
            var year = date.Substring(0, separatorIndex);
            var month = date.Substring(separatorIndex + 1, 2);

            return $"{month}-{year}";
        }

        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> SortData(IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> list, byte sortId, bool isSortAsc)
        {
            switch (sortId)
            {
                case SORT_BY_USERNAME:
                    return isSortAsc ? list.OrderBy(m => m.Username) : list.OrderByDescending(m => m.Username);
                case SORT_BY_CUSTOMERCLASS:
                    return isSortAsc ? list.OrderBy(m => m.CustomerClass) : list.OrderByDescending(m => m.CustomerClass);
                case SORT_BY_TICKET_WON:
                    return isSortAsc ? list.OrderBy(m => m.Tickets_Won) : list.OrderByDescending(m => m.Tickets_Won);
                case SORT_BY_TICKET_LOSE:
                    return isSortAsc ? list.OrderBy(m => m.Tickets_Lose) : list.OrderByDescending(m => m.Tickets_Lose);
                case SORT_BY_STAKE_WON:
                    return isSortAsc ? list.OrderBy(m => m.Stake_Won) : list.OrderByDescending(m => m.Stake_Won);
                case SORT_BY_STAKE_LOSE:
                    return isSortAsc ? list.OrderBy(m => m.Stake_Lose) : list.OrderByDescending(m => m.Stake_Lose);
                case SORT_BY_WINLOSS_WON:
                    return isSortAsc ? list.OrderBy(m => m.Member_Won) : list.OrderByDescending(m => m.Member_Won);
                case SORT_BY_WINLOSS_LOSE:
                    return isSortAsc ? list.OrderBy(m => m.Member_Lose) : list.OrderByDescending(m => m.Member_Lose);
                case SORT_BY_HOUSEWL_WON:
                    return isSortAsc ? list.OrderBy(m => m.OurPT_Won) : list.OrderByDescending(m => m.OurPT_Won);
                case SORT_BY_HOUSEWL_LOSE:
                    return isSortAsc ? list.OrderBy(m => m.OurPT_Lose) : list.OrderByDescending(m => m.OurPT_Lose);
                case SORT_BY_HOUSEWL_NETWL:
                    return isSortAsc ? list.OrderBy(m => m.OurPT_NetWinloss) : list.OrderByDescending(m => m.OurPT_NetWinloss);
                default:
                    return list;
            }
        }

        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss_Detail> GetDetails(DateTime fromDate, DateTime toDate,
	        string parlayTypes, string sportsType, byte liveType, string combos, long custId)
        {
	        var info = _commonService.GetBalanceUpDayInfo();
	        return _memberWinLossDataAccess.RMS_Rpt_ComboParlay_Winloss_Details(fromDate, toDate, info.MoveBetsDay,
		        parlayTypes, sportsType, liveType, combos, custId);

        }
    }
}
