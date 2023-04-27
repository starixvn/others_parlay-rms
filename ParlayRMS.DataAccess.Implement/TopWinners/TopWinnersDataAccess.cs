using Fanex.Data;
using ParlayRMS.DTO.BetList;
using ParlayRMS.DTO.TopWinners;
using ParlayRMS.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParlayRMS.DataAccess.Implement.TopWinners
{
    public class TopWinnersDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_Rpt_Parlay_TopWinner> RMS_Rpt_Parlay_TopWinners(
                                                         DateTime fromDate,
                                                         DateTime toDate,
                                                         DateTime moveBetsDay,
                                                         DateTime balanceUpDay,
                                                         string sportsType,
                                                         int selectTop,
                                                         string combos,
                                                         string parlayTypes,
                                                         byte liveType,
                                                         string currencies,
                                                         byte site,
                                                         Int64? customerClass)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners"))
            {
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                db.AddInParameter("balanceupday", DbType.DateTime, balanceUpDay);
                db.AddInParameter("limitedRow", DbType.Int32, selectTop);
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("LiveStatus", DbType.Int16, liveType);
                db.AddInParameter("sporttype", DbType.String, sportsType);
                db.AddInParameter("currency", DbType.String, currencies);
                db.AddInParameter("Site", DbType.Byte, site);
                db.AddInParameter("CustomerClass", DbType.Int32, customerClass);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_TopWinner
                        {
                            CustId = reader.ToInt64("custid"),
                            Username = reader.ToString("username"),
                            Currency = reader.ToString("currency"),
                            Stake = reader.ToDecimal("stake"),
                            Winlost = reader.ToDecimal("winlost"),
                            Company_Winlost = reader.ToDecimal("company_winlost"),
                            Stake_BaseRM = reader.ToDecimal("stake_baseRM"),
                            Winlost_BaseRM = reader.ToDecimal("winlost_baseRM"),
                            Company_Winlost_BaseRM = reader.ToDecimal("company_winlost_baseRM"),
                            BetCount = reader.ToInt32("betcount"),
                            Avg_Odds = reader.ToDecimal("avg_odds"),
                            Level = reader.ToInt32("level"),
                            CustomerClass = reader["CustomerClass"] == DBNull.Value ? "None" : reader.ToInt32("CustomerClass").ToString(),
                            SiteGroup = reader.ToString("SiteGroup")
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details(
                                                                long custId,
                                                                DateTime fromDate,
                                                                DateTime toDate,
                                                                DateTime moveBetsDay,
                                                                string sportsType,
                                                                string combos,
                                                                string parlayTypes,
                                                                byte liveType,
                                                                string currencies)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner_Detail>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Details"))
            {
                db.AddInParameter("custid", DbType.Int64, custId);
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("LiveStatus", DbType.Int16, liveType);
                db.AddInParameter("sporttype", DbType.String, sportsType);
                db.AddInParameter("currency", DbType.String, currencies);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_TopWinner_Detail
                        {
                            TransId = reader.ToInt64("transid"),
                            RefNo = reader.ToInt64("refno"),
                            SportType = reader.ToInt32("sporttype"),
                            BetType = reader.ToInt32("bettype"),
                            Stake = reader.ToDecimal("stake"),
                            Winlost = reader.ToDecimal("winlost"),
                            Company_Winlost = reader.ToDecimal("company_winlost"),
                            Stake_BaseRM = reader.ToDecimal("stake_baseRM"),
                            Winlost_BaseRM = reader.ToDecimal("winlost_baseRM"),
                            Company_Winlost_BaseRM = reader.ToDecimal("company_winlost_baseRM"),
                            TotalCombo = reader.ToInt32("totalcombo"),
                            Selection = reader.ToInt32("combo"),
                            ParlayType = reader.ToInt32("ParlayType"),
                            IsGOF = reader.ToBoolean("IsGOF")
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<RMS_Parlay_TransDetails> RMS_Rpt_Parlay_TopWinners_Bet_Details(long transId, long refNo)
        {
            var result = new List<RMS_Parlay_TransDetails>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Bet_Details"))
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
                            Stake = reader.ToDecimal("stake")
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details_old(
                                                                long custId,
                                                                DateTime fromDate,
                                                                DateTime toDate,
                                                                string sportsType,
                                                                string combos,
                                                                string parlayTypes,
                                                                string currencies)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner_Detail>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Details_old"))
            {
                db.AddInParameter("custid", DbType.Int64, custId);
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("sporttype", DbType.String, sportsType);
                db.AddInParameter("currency", DbType.String, currencies);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_TopWinner_Detail
                        {
                            TransId = reader.ToInt64("transid"),
                            RefNo = reader.ToInt64("refno"),
                            SportType = reader.ToInt32("sporttype"),
                            Stake = reader.ToDecimal("stake"),
                            Winlost = reader.ToDecimal("winlost"),
                            Company_Winlost = reader.ToDecimal("company_winlost"),
                            Stake_BaseRM = reader.ToDecimal("stake_baseRM"),
                            Winlost_BaseRM = reader.ToDecimal("winlost_baseRM"),
                            Company_Winlost_BaseRM = reader.ToDecimal("company_winlost_baseRM"),
                            TotalCombo = reader.ToInt32("totalcombo"),
                            Selection = reader.ToInt32("combo"),
                            ParlayType = reader.ToInt32("ParlayType"),
                            BetType = reader.ToInt32("bettype"),
                            IsGOF = reader.ToBoolean("IsGOF")
                        });
                    }
                }
            }

            return result;
        }
    }
}