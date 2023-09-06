namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Criteria.LeagueWinningMonitor;
    using DTO.LeagueWinningMonitor;
    using Utility;
    using Utility.Data;

    public class LeagueWinningMonitorDataAccess
    {
        private const byte ReportByLeague = 1;

        public IEnumerable<RmsRptLeagueWinningMonitor> GetData(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            byte group,
            byte reportBy)
        {

            var criteria = new Rpt_League_Winning_Monitor_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                MaxPayout = winningAmount,
                LimitRowCount = selectTop,
                LeagueGroupId = group,
                ReportType = reportBy,
                LeagueId = 0,
                CustId = 0
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    return reportBy == ReportByLeague 
                        ? GetLeagueList(reader) 
                        : GetUsernameList(reader);
                }
            }
        }

        public IEnumerable<RmsRptLeagueWinningMonitorDetail> GetDetailList(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            byte group,
            byte reportBy,
            int leagueId,
            int custId)
        {
            var criteria = new Rpt_League_Winning_Monitor_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                MaxPayout = winningAmount,
                LimitRowCount = selectTop,
                LeagueGroupId = group,
                ReportType = reportBy,
                LeagueId = leagueId,
                CustId = custId
            };


            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    return GetRmsRptLeagueWinningMonitorDetails(reader);
                }
            }
        }

        private static IEnumerable<RmsRptLeagueWinningMonitorDetail> GetRmsRptLeagueWinningMonitorDetails(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return new RmsRptLeagueWinningMonitorDetail
                {
                    TransDate = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                    TransId = reader.ToInt64("transid"),
                    Username = reader.ToString("Username"),
                    Level = reader["level"] == DBNull.Value ? "None" : reader.ToByte("level").ToString(),
                    Odds = reader.ToDecimal("odds"),
                    Stake = reader.ToDecimal("stake"),
                    WinningAmount = reader.ToDecimal("winningAmt"),
                    CompanyLoseAmount = reader.ToDecimal("companyLoseAmt"),
                    Combo = reader.ToInt32("combo"),
                    TotalSelection = Convert.ToInt32(reader.ToDecimal("totalSelection")),
                    SportType = reader.ToInt32("Sporttype"),
                    SportTypeName = CacheRef.GetSportNameDropDown(reader.ToInt32("Sporttype")),
                    IP = reader.ToString("IP"),
                    CustId = reader.ToInt32("custid"),
                    BetType = reader.ToInt32("bettype"),
                    RefNo = reader.ToInt64("refno"),
                    SiteGroup = reader.ToString("SiteGroup")
                };
            }
        }

        private static IEnumerable<RmsRptLeagueWinningMonitor> GetLeagueList(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return new RmsRptLeagueWinningMonitor
                {
                    LeagueId = reader.ToInt32("leagueid"),
                    LeagueName = reader.ToString("leagueName"),
                    TotalStake = reader.ToDecimal("totalStake"),
                    TotalWinningAmount = reader.ToDecimal("totalWinningAmt"),
                    CompanyLoseAmount = reader.ToDecimal("companyLoseAmt"),
                    TotalTickets = reader.ToInt32("totalTicket")
                };
            }
        }

        private static IEnumerable<RmsRptLeagueWinningMonitor> GetUsernameList(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return new RmsRptLeagueWinningMonitor
                {
                    CustId = reader.ToInt32("custid"),
                    Username = reader.ToString("username"),
                    TotalStake = reader.ToDecimal("totalStake"),
                    TotalWinningAmount = reader.ToDecimal("totalWinningAmt"),
                    CompanyLoseAmount = reader.ToDecimal("companyLoseAmt"),
                    TotalTickets = reader.ToInt32("totalTicket"),
                    SiteGroup = reader.ToString("SiteGroup")
                };
            }
        }
    }
}
