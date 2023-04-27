using Fanex.Data;
using ParlayRMS.DTO.LeagueWinningMonitor;
using ParlayRMS.Utility;
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
                        });
                    }
                }
            }

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
                    TotalStake = reader.ToDecimal("totalStake"),
                    TotalWinningAmount = reader.ToDecimal("totalWinningAmt"),
                    CompanyLoseAmount = reader.ToDecimal("companyLoseAmt"),
                    TotalTickets = reader.ToInt32("totalTicket")
                });
            }
        }

        public void GetUsernameList(IDataReader reader, List<RmsRptLeagueWinningMonitor> result)
        {
            while (reader.Read())
            {
                result.Add(new RmsRptLeagueWinningMonitor
                {
                    CustId = reader.ToInt32("custid"),
                    Username = reader.ToString("username"),
                    TotalStake = reader.ToDecimal("totalStake"),
                    TotalWinningAmount = reader.ToDecimal("totalWinningAmt"),
                    CompanyLoseAmount = reader.ToDecimal("companyLoseAmt"),
                    TotalTickets = reader.ToInt32("totalTicket"),
                    SiteGroup = reader.ToString("SiteGroup")
                });
            }
        }
    }
}