namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Criteria.ParlayMonitor;
    using DTO.ParlayMonitor;
    using DTO.PotentialWinning;
    using Utility.Data;

    public class ParlayMonitorDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<RMS_Monitor_Parlay_Betlist> RMS_Monitor_Parlay_Betlist(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal stakeAfterPT,
            decimal maxPayout,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets,
            bool isCornerParlay,
            decimal? odds,
            string leagueGroups,
            int? customerClass,
            string company,
            string username)
        {
            var criteria = new Monitor_ComboParlay_Betlist_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                Stake = stakeAfterPT,
                MaxPayout = maxPayout,
                ReportType = reportType,
                Max_TransId = maxTransId,
                LimitRowCount = selectTop,
                Market = markets,
                IsCornerParlay = isCornerParlay,
                Odds = odds,
                CustomerClass = customerClass,
                LeagueGroupId = leagueGroups,
                Company = company,
                Username = string.IsNullOrWhiteSpace(username) ? null : username
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return new RMS_Monitor_Parlay_Betlist
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                            TransId = reader.ToString("transid"),
                            RefNo = reader.ToString("refno"),
                            Username = reader.ToString("username"),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("stake"),
                            PotentialWinningAmount = reader.ToDecimal("potentialWinningAmt"),
                            NetWinAfterPT = reader.ToDecimal("maxpayout"),
                            TotalSelection = Convert.ToInt32(reader.ToDecimal("totalselection")),
                            TotalBet = reader.ToInt32("totalbet"),
                            NumberWon = reader.ToInt32("numberwon"),
                            IP = reader.ToString("ip"),
                            Level = reader.ToInt32("level"),
                            IsBetRadar = reader.ToBoolean("IsBetRadar"),
                            SportType = reader.ToInt32("sporttype"),
                            BetType = reader.ToInt32("bettype"),
                            CustId = reader.ToInt32("custid"),
                            IsLive = reader.ToBoolean("islive"),
                            IsBlacklistLeague = Convert.ToInt32(reader["isblacklistleague"]) == 1,
                            IsGOF = reader.ToBoolean("IsGOF"),
                            CustomerClass = reader["CustomerClass"] == DBNull.Value ? "None" : reader.ToInt32("CustomerClass").ToString(),
                            TrackingUser = reader["TrackingUser"] == DBNull.Value ? "None" : reader.ToString("TrackingUser"),
                            SiteGroup = reader.ToString("SiteGroup")
                        };
                    }
                }
            }
        }

        public UserTracking RMS_Monitor_Parlay_UserTracking(int userid, string username, string transid)
        {
            var command = new ComboParlay_PotentialWinning_UserTracking_Command
            {
                Username = username,
                UserId = userid,
                TransId = transid
            };
            
            repository.Execute(command);

            var userTracking = new UserTracking
            {
                StatusId = command.StatusId,
                Message = command.Message
            };

            return userTracking;
        }
    }
}
