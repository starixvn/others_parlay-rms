using Fanex.Data;
using ParlayRMS.DTO.LeagueWinningMonitor;
using ParlayRMS.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParlayRMS.DataAccess.Implement.LeagueWinningMonitor
{
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
            var result = new List<RmsRptLeagueWinningMonitor>();
            using (var db = new ObjectDb("RMS_Rpt_League_Winning_Monitor"))
            {
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("ParlayType", DbType.String, parlayTypes);
                db.AddInParameter("sporttype", DbType.String, sportsTypes);
                db.AddInParameter("level", DbType.String, levels);
                db.AddInParameter("maxpayout", DbType.Decimal, winningAmount);
                db.AddInParameter("limitRowCount", DbType.Int32, selectTop);
                db.AddInParameter("leagueGroupId", DbType.Byte, group);
                db.AddInParameter("ReportType", DbType.Byte, reportBy);

                using (var reader = db.ExecuteReader())
                {
                    if (reportBy == ReportByLeague)
                    {
                        GetLeagueList(reader, result);
                    }
                    else
                    {
                        GetUsernameList(reader, result);
                    }
                }
            }

            return result;
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
            var result = new List<RmsRptLeagueWinningMonitorDetail>();
            using (var db = new ObjectDb("RMS_Rpt_League_Winning_Monitor"))
            {
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("ParlayType", DbType.String, parlayTypes);
                db.AddInParameter("sporttype", DbType.String, sportsTypes);
                db.AddInParameter("level", DbType.String, levels);
                db.AddInParameter("maxpayout", DbType.Decimal, winningAmount);
                db.AddInParameter("limitRowCount", DbType.Int32, selectTop);
                db.AddInParameter("leagueGroupId", DbType.Byte, group);
                db.AddInParameter("ReportType", DbType.Byte, reportBy);
                db.AddInParameter("LeagueID", DbType.Int32, leagueId);
                db.AddInParameter("CustID", DbType.Int32, custId);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RmsRptLeagueWinningMonitorDetail
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransId = reader.ToInt64("transid"),
                            Username = reader.ToString("Username"),
                            Level = reader.ToByte("level"),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("stake"),
                            WinningAmount = reader.ToDecimal("winningAmt"),
                            NetWinAfterPT = reader.ToDecimal("netWinAfterPT"),
                            CompanyLoseAmount = reader.ToDecimal("companyLoseAmt"),
                            Combo = reader.ToInt32("combo"),
                            TotalSelection = reader.ToDecimal("totalSelection"),
                            SportType = reader.ToInt64("Sporttype"),
                            IP = reader.ToString("IP"),
                            CustId = reader.ToInt32("custid"),
                            BetType = reader.ToInt32("bettype"),
                            RefNo = reader.ToInt64("refno")
                        });
                    }
                }
            }
            //test
            CreateTestDataDetail(result);

            return result;
        }

        public void GetLeagueList(IDataReader reader, List<RmsRptLeagueWinningMonitor> result)
        {
            while (reader.Read())
            {
                result.Add(new RmsRptLeagueWinningMonitor
                {
                    LeagueId = reader.ToInt32("leagueid"),
                    LeagueName = reader.ToString("leagueName"),
                    TotalStake = reader.ToInt32("totalStake"),
                    TotalWinningAmount = reader.ToInt32("totalWinningAmt"),
                    CompanyLoseAmount = reader.ToInt32("companyLoseAmt"),
                    TotalTickets = reader.ToInt32("totalTicket")
                });
            }
            // data test
            CreateTestData(result);
        }

        public void GetUsernameList(IDataReader reader, List<RmsRptLeagueWinningMonitor> result)
        {
            while (reader.Read())
            {
                result.Add(new RmsRptLeagueWinningMonitor
                {
                    CustId = reader.ToInt32("custid"),
                    Username = reader.ToString("username"),
                    TotalStake = reader.ToInt32("totalStake"),
                    TotalWinningAmount = reader.ToInt32("totalWinningAmt"),
                    CompanyLoseAmount = reader.ToInt32("companyLoseAmt"),
                    TotalTickets = reader.ToInt32("totalTicket")
                });
            }
            // data test
            CreateTestData(result);
        }

        public void CreateTestData(List<RmsRptLeagueWinningMonitor> result)
        {
            result.Add(new RmsRptLeagueWinningMonitor
            {
                LeagueId = 1,
                LeagueName = "league name",
                CustId = 22,
                Username = "username",
                TotalStake = 10,
                TotalWinningAmount = 20,
                CompanyLoseAmount = 30,
                TotalTickets = 1
            });

            result.Add(new RmsRptLeagueWinningMonitor
            {
                LeagueId = 2,
                LeagueName = "league name 2",
                CustId = 222,
                Username = "username 2",
                TotalStake = 102,
                TotalWinningAmount = 202,
                CompanyLoseAmount = 302,
                TotalTickets = 2
            });
        }

        public void CreateTestDataDetail(List<RmsRptLeagueWinningMonitorDetail> result)
        {
            result.Add(new RmsRptLeagueWinningMonitorDetail
            {
                TransDate = DateTime.Now,
                TransId = 123456,
                Username = "username",
                Level = 10,
                Odds = 12345,
                Stake = 123456,
                WinningAmount = 654321,
                NetWinAfterPT = 22222,
                CompanyLoseAmount = 30,
                Combo = 1,
                TotalSelection = 11,
                SportType = 1,
                IP = "12.0.0",
                RefNo = 100845221503,
                CustId = 26722481
            });

            result.Add(new RmsRptLeagueWinningMonitorDetail
            {
                TransDate = DateTime.Now,
                TransId = 111880908238,
                Username = "12BETAFF1780CashOut",
                Level = 10,
                Odds = 12345,
                Stake = 123456,
                WinningAmount = 654321,
                NetWinAfterPT = 22222,
                CompanyLoseAmount = 30,
                Combo = 1,
                TotalSelection = 11,
                SportType = 1,
                IP = "12.0.0..7",
                RefNo = 100845221503,
                CustId = 26722481
            });
        }
    }
}