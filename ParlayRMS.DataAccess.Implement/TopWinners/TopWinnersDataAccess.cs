// -----------------------------------------------------------------------
// <copyright file="TopWinnersDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Implement.TopWinners
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.TopWinners;
    using ParlayRMS.DTO.TopWinners;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TopWinnersDataAccess : BaseDataAccess, ITopWinnersDataAccess
    {
        public IEnumerable<RMS_Rpt_Parlay_TopWinner> RMS_Rpt_Parlay_TopWinners(DateTime fromDate, DateTime toDate, DateTime moveBetsDay, int sportsType, int selectTop)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner>();

            try
            {
                using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners"))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                    db.AddInParameter("todate", DbType.DateTime, toDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
                    db.AddInParameter("limitedRow", DbType.Int32, selectTop);
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
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details(long custId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, int sportsType)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner_Detail>();

            try
            {
                using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Details"))
                {
                    db.AddInParameter("custid", DbType.Int64, custId);
                    db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                    db.AddInParameter("todate", DbType.DateTime, toDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDay);
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
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        public IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> RMS_Rpt_Parlay_TopWinners_Details_old(long custId, DateTime fromDate, DateTime toDate, int sportsType)
        {
            var result = new List<RMS_Rpt_Parlay_TopWinner_Detail>();

            try
            {
                using (var db = new ObjectDb("RMS_Rpt_Parlay_TopWinners_Details_old"))
                {
                    db.AddInParameter("custid", DbType.Int64, custId);
                    db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                    db.AddInParameter("todate", DbType.DateTime, toDate);
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
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }
    }
}