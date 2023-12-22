namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Criteria.LeagueBetTypeAnalysis;
    using DTO.LeagueBetTypeAnalysis;
    using DTO.Leagues;
    using Utility;
    using Utility.Data;

    public class LeagueBetTypeAnalysisDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public List<RMS_Rpt_LeagueBettype_Analysis> RMS_Rpt_LeagueBettype_Analysis(
                                                       DateTime fromDate,
                                                       DateTime toDate,
                                                       string parlayTypes,
                                                       string leagueGroups,
                                                       string leagues,
                                                       int site,
                                                       double ratio,
                                                       int reportBy,
                                                       int sportType)
        {
            var criteria = new Rpt_LeagueBettype_Analysis_Criteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                ParlayType = parlayTypes,
                LeagueGroups = leagueGroups,
                Leagues = leagues,
                Site = site,
                Ratio = ratio,
                ReportBy = reportBy,
                SportType = sportType
            };

            var result = new List<RMS_Rpt_LeagueBettype_Analysis>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    var index = 1;
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_LeagueBettype_Analysis
                        {
                            No = index++,
                            Bettype = reportBy == 2 ? CacheRef.GetBettype(reader.ToInt32("Bettype")) : "",
                            League = reportBy == 1  ? CacheRef.GetLeagueName(reader.ToInt32("leagueId")) : "",
                            LeagueId = reader.ToInt32("leagueId"),
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
            var ratio = (double)wonTickets / totalTickets * 100;
            return Math.Round(ratio) + "%";
        }

        public IEnumerable<LeagueGroup> GetLeagueGroups(int sportType)
        {
            var criteria = new LeagueByGroupSel_Criteria { SportType = sportType };
            return repository.Fetch<LeagueGroup>(criteria);
        }

        public List<RMS_LeagueSel> RMS_LeagueSel(string leagueName, int groupId, int sportType, DateTime fromDate, DateTime toDate)
        {
            var criteria = new LeagueSel_Criteria
            {
                LeagueName = leagueName,
                FromDate = fromDate,
                ToDate = toDate,
                SportType = sportType,
                GroupId = groupId
            };
            var counter = 1;
            var leagues = repository.Fetch<RMS_LeagueSel>(criteria).ToList();
            leagues.ForEach(r => { r.No = counter++; });

            return leagues;
        }
    }
}
