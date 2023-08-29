namespace ParlayRMS.DataAccess.BetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DTO.BetDetails;
    using ParlayRMS.DTO.BetList;
    using Utility.Data;

    public class BetDetailsDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_Parlay_TransDetails> RMS_Rpt_Parlay_Bet_Details(long transId, long refNo)
        {
            var result = new List<RMS_Parlay_TransDetails>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_Bet_Details"))
            {
                db.AddInParameter("transid", DbType.Int64, transId);
                db.AddInParameter("refno", DbType.Int64, refNo);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Parlay_TransDetails
                        {
                            TransId = reader.ToInt64("transid"),
                            BetType = reader.ToInt32("bettype"),
                            HomeId = reader.ToInt32("homeid"),
                            AwayId = reader.ToInt32("awayid"),
                            Hdp1 = reader.ToDecimal("hdp1"),
                            Hdp2 = reader.ToDecimal("hdp2"),
                            BetTeam = reader.ToString("betteam"),
                            Odds = reader.ToDecimal("odds"),
                            StatusId = reader.ToInt32("statusid"),
                            Status = reader.ToString("status"),
                            LiveHomeScore = reader.ToInt32("livehomescore"),
                            LiveAwayScore = reader.ToInt32("liveawayscore"),
                            LiveIndicator = reader.ToBoolean("liveindicator"),
                            LeagueId = reader.ToInt32("leagueid"),
                            SportType = reader.ToInt32("sporttype"),
                            IsNeutral = reader.ToBoolean("IsNeutral"),
                            MatchId = reader.ToInt32("matchid"),
                            MatchCode = reader.ToInt32("matchcode"),
                            BetStatus = reader.ToString("betstatus"),
                            BetCheck = reader.ToString("betcheck"),
                            TransDesc = reader.ToString("transdesc"),
                            Stake = reader.ToDecimal("stake"),
                            IsBlacklistLeague = Convert.ToInt32(reader["isblacklistleague"]) == 1
                        });
                    }
                }
            }

            return result;
        }


        public BetInfo GetBetInfo(long transId)
        {
            using (var db = new ObjectDb("RMS_Rpt_Parlay_BetInfo"))
            {
                db.AddInParameter("transid", DbType.Int64, transId);
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new BetInfo
                        {
                            BetTeam = reader.ToString("betteam"),
                            BetCheck = reader.ToString("betcheck"),
                            Hdp1 = Convert.ToInt32(reader.ToDecimal("hdp1")),
                            Hdp2 = Convert.ToInt32(reader.ToDecimal("hdp2")),
                        };
                    }
                }
            }

            return null;
        }
    }
}
