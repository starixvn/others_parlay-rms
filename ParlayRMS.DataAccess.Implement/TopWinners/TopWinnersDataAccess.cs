// --------------------------------------------------------------------------
// <copyright file="TopWinnersDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.TopWinners
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.TopWinners;
    using ParlayRMS.DTO.BetList;
    using ParlayRMS.DTO.TopWinners;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Top Winners Data Access.
    /// </summary>
    public class TopWinnersDataAccess : BaseDataAccess, ITopWinnersDataAccess
    {
        /// <summary>
        /// Gets top winners.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="balanceUpDay">Balance Up Day.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="selectTop">Number of top winners will be selected.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>List of top winners.</returns>
        public IEnumerable<RMS_Rpt_Parlay_TopWinner> RMS_Rpt_Parlay_TopWinners(
                                                         DateTime fromDate, 
                                                         DateTime toDate, 
                                                         DateTime moveBetsDay, 
                                                         DateTime balanceUpDay, 
                                                         int sportsType, 
                                                         int selectTop, 
                                                         string combos)
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
                if (sportsType != 0)
                {
                    db.AddInParameter("sporttype", DbType.Int32, sportsType);
                }

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
                            Level = reader.ToInt32("level")
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a top winner details.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="moveBetsDay">The move bets day.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Top winner details.</returns>
        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details(
                                                                long custId, 
                                                                DateTime fromDate, 
                                                                DateTime toDate, 
                                                                DateTime moveBetsDay, 
                                                                int sportsType, 
                                                                string combos)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner_Detail>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Details"))
            {
                db.AddInParameter("custid", DbType.Int64, custId);
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                db.AddInParameter("selection", DbType.String, combos);
                if (sportsType != 0)
                {
                    db.AddInParameter("sporttype", DbType.Int32, sportsType);
                }

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
                            Combo = reader.ToInt32("combo"),
                            Odds = reader.ToDecimal("odds")
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a top winner bet details.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>Top winner bet details.</returns>
        public IEnumerable<RMS_Parlay_TransDetails> RMS_Rpt_Parlay_TopWinners_Bet_Details(long refNo)
        {
            var result = new List<RMS_Parlay_TransDetails>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Bet_Details"))
            {
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
                            BetStatus = reader.ToString("betstatus")
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a top winner bet details.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sportsType">Sport type ID.</param>
        /// <param name="combos">Combo values.</param>
        /// <returns>Top winner bet details.</returns>
        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details_old(
                                                                long custId, 
                                                                DateTime fromDate, 
                                                                DateTime toDate, 
                                                                int sportsType, 
                                                                string combos)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner_Detail>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Details_old"))
            {
                db.AddInParameter("custid", DbType.Int64, custId);
                db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                db.AddInParameter("todate", DbType.DateTime, toDate);
                db.AddInParameter("selection", DbType.String, combos);
                if (sportsType != 0)
                {
                    db.AddInParameter("sporttype", DbType.Int32, sportsType);
                }

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
                            Combo = reader.ToInt32("combo"),
                            Odds = reader.ToDecimal("odds")
                        });
                    }
                }
            }

            return result;
        }
    }
}