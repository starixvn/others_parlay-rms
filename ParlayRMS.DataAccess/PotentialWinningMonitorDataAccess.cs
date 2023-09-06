namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.PotentialWinningMonitor;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Criteria.ParlayMonitor;
    using DTO.PotentialWinning;
    using Utility.Data;

    public class PotentialWinningMonitorDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();
        public IEnumerable<RMS_Monitor_Potential_Winning> RMS_Monitor_Parlay_PotentialWinning(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            bool isCornerParlay,
            byte filterBy,
            int? agentId,
            int? customerClass,
            string company,
            string username)
        {
            var criteria = new Monitor_ComboParlay_PotentialWinning_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                WinningAmount = winningAmount,
                LimitRowCount = selectTop,
                IsCornerParlay = isCornerParlay,
                IsAgentView = filterBy > 0,
                Recommend = agentId,
                CustomerClass = customerClass,
                Company = company,
                Username = string.IsNullOrWhiteSpace(username) ? null : username
            };


            using (var db = ObjectDbFactory.CreateInstance("RMS_Monitor_ComboParlay_PotentialWinning"))
            {

                using (var reader = db.ExecuteReader(criteria))
                {
                    return filterBy > 0
                        ? GetRmsMonitorPotentialWinningsByAgent(reader)
                        : GetRmsMonitorPotentialWinnings(reader);
                }
            }
        }

        private static IEnumerable<RMS_Monitor_Potential_Winning> GetRmsMonitorPotentialWinnings(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return new RMS_Monitor_Potential_Winning
                {
                    TransDate = reader.ToDateTime("transdate"),
                    TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                    CustId = reader.ToInt32("custid"),
                    UserName = reader.ToString("username"),
                    Currency = reader.ToString("currency"),
                    Odds = reader.ToDecimal("odds"),
                    Stake = reader.ToDecimal("stake"),
                    WinningAmount = reader.ToDecimal("winningamount"),
                    CompanyLoseAmount = reader.ToDecimal("companyloseamount"),
                    Level = reader.ToInt32("level"),
                    TotalTicket = reader.ToInt32("totalbets"),
                    CustomerClass = reader["CustomerClass"] == DBNull.Value
                        ? "None"
                        : reader.ToInt32("CustomerClass").ToString(),
                    TotalTrackingBets = reader.ToInt32("TotalTrackingBets"),
                    SiteGroup = reader.ToString("SiteGroup")
                };
            }
        }

        private static IEnumerable<RMS_Monitor_Potential_Winning> GetRmsMonitorPotentialWinningsByAgent(IDataReader reader)
        {
            while (reader.Read())
            {
                yield return new RMS_Monitor_Potential_Winning
                {
                    AgentName = reader.ToString("agentname"),
                    WinningAmount = reader.ToDecimal("winningamount"),
                    TotalMember = reader.ToInt32("totalmember"),
                    Recommend = reader.ToInt32("recommend"),
                    Stake = reader.ToDecimal("TotalStake")
                };
            }
        }

        public IEnumerable<RMS_Monitor_Potential_Winning_Detail> RMS_Monitor_Parlay_PotentialWinningDetail(
            string combos,
            string parlayTypes,
            string sportsTypes,
            int custId,
            bool isCornerParlay)
        {
            var criteria = new Monitor_ComboParlay_PotentialWinningDetails_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                CustId = custId,
                IsCornerParlay = isCornerParlay
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return new RMS_Monitor_Potential_Winning_Detail
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                            TransId = reader.ToInt64("transid"),
                            Odds = reader.ToDecimal("odds"),
                            RefNo = reader.ToInt64("refno"),
                            Stake = reader.ToDecimal("stake"),
                            WinningAmount = reader.ToDecimal("winningamount"),
                            CompanyLoseAmount = reader.ToDecimal("companyloseamount"),
                            Ip = reader.ToString("ip"),
                            HousePT = reader.ToDecimal("housept"),
                            TotalBet = reader.ToInt32("totalbet"),
                            TotalSelection = Convert.ToInt32(reader.ToDecimal("totalselection")),
                            NumberWon = Convert.ToInt32(reader.ToDecimal("numberwon")),
                            SportType = reader.ToInt32("sporttype"),
                            BetType = reader.ToInt32("bettype"),
                            IsGOF = reader.ToBoolean("IsGOF"),
                            TrackingUser = reader["TrackingUser"] == DBNull.Value ? "None" : reader.ToString("TrackingUser")
                        };
                    }
                }
            }
        }

        public UserTracking RMS_ComboParlay_PotentialWinning_UserTracking(int userid, string username, string transid)
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
