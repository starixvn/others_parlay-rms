using Fanex.Data;
using ParlayRMS.DTO.LeagueBetTypeAnalysis;
using ParlayRMS.Utility;
using ParlayRMS.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParlayRMS.DataAccess.Implement.LeagueBetTypeAnalysis
{
    public class LeagueBetTypeAnalysisDataAccess : BaseDataAccess
    {
        public List<RMS_Rpt_LeagueBettype_Analysis> RMS_Rpt_LeagueBettype_Analysis(
                                                       DateTime fromDate,
                                                       DateTime toDate,
                                                       string parlayTypes,
                                                       string leagueGroups,
                                                       string leagues,
                                                       int site,
                                                       double ratio,
                                                       int reportBy)
        {
            var result = new List<RMS_Rpt_LeagueBettype_Analysis>();

            using (var db = new ObjectDb("RMS_Rpt_LeagueBettype_Analysis"))
            {
                db.AddInParameter("FromDate", DbType.DateTime, fromDate);
                db.AddInParameter("ToDate", DbType.DateTime, toDate);
                db.AddInParameter("ParlayType", DbType.String, parlayTypes);
                db.AddInParameter("LeagueGroups", DbType.String, leagueGroups);
                db.AddInParameter("Leagues", DbType.String, leagues);
                db.AddInParameter("Site", DbType.Int32, site);
                db.AddInParameter("Ratio", DbType.Double, ratio);
                db.AddInParameter("ReportBy", DbType.Int32, reportBy);

                using (var reader = db.ExecuteReader())
                {
                    int index = 1;
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_LeagueBettype_Analysis
                        {
                            No = index++,
                            Bettype = reportBy == 2 ? CacheRef.GetBettype(reader.ToInt32("Bettype")) : "",
                            League = reportBy == 1  ? CacheRef.GetLeagueName(reader.ToInt32("leagueId")) : "",
                            GroupName = reportBy == 1 ? reader.ToString("GroupName") : "",
                            TotalTickets = reader.ToInt32("TotalTickets"),
                            WonTickets = reader.ToInt32("WonTickets"),
                            DrawTickets = reader.ToInt32("DrawTickets"),
                            Ratio = GetRatio(reader.ToInt32("WonTickets"), reader.ToInt32("TotalTickets")),
                            ParlayWonTickets = reader.ToInt32("ParlayWonTickets"),
                            TurnOver = reader.ToDecimal("Stake"),
                            MemberWinloss = reader.ToDecimal("MemberWinloss"),
                            MainHouseWinloss = reader.ToDecimal("MainHouseWinloss")
                        });
                    }
                }
            }

            return result;
        }

        private string GetRatio(int wonTickets, int totalTickets)
        {
            if (wonTickets == 0 || totalTickets == 0)
            {
                return "0%";
            }
            double ratio = ((double)wonTickets / totalTickets) * 100;
            return Math.Round(ratio).ToString() + "%";
        }
    }
}
