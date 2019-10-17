using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using ParlayRMS.DTO.BetList;
using ParlayRMS.Utility.Data;

namespace ParlayRMS.DataAccess.Implement.BetList
{
    public class BetListDataAccess : BaseDataAccess
    {
        public BetListData RMS_Parlay_TransDetails(long transId, long refNo)
        {
            var tickets = new List<RMS_Parlay_TransDetails>();
            var currencyName = string.Empty;
            using (var db = new ObjectDb("RMS_Parlay_TransDetails"))
            {
                db.AddInParameter("transid", DbType.Int64, transId);
                db.AddInParameter("refno", DbType.Int64, refNo);
                db.AddOutParameter("currencyname", DbType.String, 50);
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tickets.Add(new RMS_Parlay_TransDetails
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
                            Stake = reader.ToDecimal("stake"),
                            TransDate = reader.ToDateTime("transdate"),
                            TransDesc = reader.ToString("transdesc")
                        });
                    }
                }

                var outputParam = db.GetOuputParams();
                currencyName = outputParam["currencyname"] == DBNull.Value ? string.Empty : (string)outputParam["currencyname"];
            }

            return new BetListData
            {
                Tickets = tickets,
                CurrencyName = currencyName
            };
        }

        public RMS_Monitor_Parlay_BetInfo RMS_Monitor_Parlay_BetInfo(long transId)
        {
            using (var db = new ObjectDb("RMS_Monitor_Parlay_BetInfo"))
            {
                db.AddInParameter("transid", DbType.Int64, transId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new RMS_Monitor_Parlay_BetInfo
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