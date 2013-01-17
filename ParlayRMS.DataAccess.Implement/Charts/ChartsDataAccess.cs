namespace ParlayRMS.DataAccess.Implement.Charts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.Charts;
    using ParlayRMS.DTO.Charts;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    public class ChartsDataAccess : BaseDataAccess, IChartsDataAccess
    {
        #region Data Sum

        /// <summary>
        ///
        /// </summary>
        /// <param name="bettype"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_Sum> RMS_Monitor_Parlay_Sum(int bettype, DateTime winlostDate, out long maxTransId)
        {
            maxTransId = 0;
            var result = new List<RMS_Monitor_Parlay_Sum>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_Sum"))
                {
                    db.AddInParameter("bettype", DbType.Int32, bettype);
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddOutParameter("max_transid", DbType.Int64, sizeof(long));

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_Sum
                            {
                                Events = reader.ToInt32("Events"),
                                Status = reader.ToString("Status"),
                                Tickets = reader.ToInt32("Tickets"),
                                Turnover = reader.ToDecimal("Turnover"),
                                Winloss = reader.ToDecimal("Winloss"),
                                CompanyWL = reader.ToDecimal("CompanyWL"),
                            });
                        }
                    }

                    maxTransId = DbConverter.ToInt64(db.GetParameterValue("max_transid"));
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        #endregion Data Sum

        #region By Ticket

        /// <summary>
        ///
        /// </summary>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket> RMS_Monitor_Parlay_SelectionByTicket(DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTicket>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTicket"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTicket
                            {
                                Selection = reader.ToInt32("Selection"),
                                All_Sports = reader.ToInt32("All_Sports"),
                                Soccer_Only = reader.ToInt32("Soccer_Only"),
                                Combined_Sports = reader.ToInt32("Combined_Sports"),
                                Other_Sports = reader.ToInt32("Other_Sports")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League> RMS_Monitor_Parlay_SelectionByTicket_League(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTicket_League>();

            try
            {
                Hashtable param = new Hashtable();
                param.Add("selection", selection);
                param.Add("sporttype", sportType);
                param.Add("winlostdate", winlostDate);
                param.Add("max_transid", maxTransId);

                using (var reader = ExecSpDataReader("RMS_Monitor_Parlay_SelectionByTicket_League", param))
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Monitor_Parlay_SelectionByTicket_League
                        {
                            Leagueid = reader.ToInt32("leagueId"),
                            NoTickets = reader.ToInt32("NoTickets")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency> RMS_Monitor_Parlay_SelectionByTicket_Currency(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            var results = new List<RMS_Monitor_Parlay_SelectionbyTicket_Currency>();

            try
            {
                var htb = new Hashtable();
                htb.Add("selection", selection);
                htb.Add("sporttype", sportType);
                htb.Add("winlostdate", winlostDate);
                htb.Add("max_transid", maxTransId);

                using (var reader = ExecSpDataReader("RMS_Monitor_Parlay_SelectionByTicket_Currency", htb))
                {
                    while (reader.Read())
                    {
                        results.Add(new RMS_Monitor_Parlay_SelectionbyTicket_Currency()
                        {
                            CurrencyId = reader.ToInt32("currency"),
                            Currency = reader.ToString("currencyName"),
                            Margin = reader.ToDecimal("margin"),
                            NoTickets = reader.ToInt32("notickets")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return results;
        }

        #endregion By Ticket

        #region By Turnover

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League> RMS_Monitor_Parlay_SelectionByTurnover_League(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTurnover_League>();
            try
            {
                Hashtable param = new Hashtable();
                param.Add("selection", selection);
                param.Add("sporttype", sportType);
                param.Add("winlostdate", winlostDate);
                param.Add("max_transid", maxTransId);

                using (var reader = ExecSpDataReader("RMS_Monitor_Parlay_SelectionByTurnover_League", param))
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Monitor_Parlay_SelectionByTurnover_League
                        {
                            Leagueid = reader.ToInt32("leagueId"),
                            Turnover = reader.ToDecimal("turnover")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover> RMS_Monitor_Parlay_SelectionByTurnover(DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTurnover>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTurnover"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTurnover
                            {
                                Selection = reader.ToInt32("Selection"),
                                All_Sports = reader.ToDecimal("All_Sports"),
                                Soccer_Only = reader.ToDecimal("Soccer_Only"),
                                Combined_Sports = reader.ToDecimal("Combined_Sports"),
                                Other_Sports = reader.ToDecimal("Other_Sports")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTurnover_Currency> RMS_Monitor_Parlay_SelectionByTurnover_Currency(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionbyTurnover_Currency>();

            try
            {
                var htb = new Hashtable();
                htb.Add("selection", selection);
                htb.Add("sporttype", sportType);
                htb.Add("winlostdate", winlostDate);
                htb.Add("max_transid", maxTransId);

                using (var reader = ExecSpDataReader("RMS_Monitor_Parlay_SelectionByTurnover_Currency", htb))
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Monitor_Parlay_SelectionbyTurnover_Currency
                        {
                            CurrencyId = reader.ToInt32("currency"),
                            Currency = reader.ToString("currencyName"),
                            Margin = reader.ToDecimal("margin"),
                            Turnover = reader.ToDecimal("turnover")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        #endregion By Turnover

        #region By Currency

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event> RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);
                    db.AddInParameter("currency", DbType.Int32, currency);
                    db.AddInParameter("topcurrency", DbType.String, topCurrency);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionbyTicket_Currency_Event
                            {
                                Awayid = reader.ToInt32("awayid"),
                                Homeid = reader.ToInt32("homeid"),
                                Leagueid = reader.ToInt32("leagueid"),
                                Matchid = reader.ToInt32("matchid"),
                                Tickets = reader.ToInt32("tickets")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="currency"></param>
        /// <param name="topCurrency"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event> RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event(int selection, int sportType, int currency, string topCurrency, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);
                    db.AddInParameter("currency", DbType.Int32, currency);
                    db.AddInParameter("topcurrency", DbType.String, topCurrency);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTurnover_Currency_Event
                            {
                                Awayid = reader.ToInt32("awayid"),
                                Homeid = reader.ToInt32("homeid"),
                                Leagueid = reader.ToInt32("leagueid"),
                                Matchid = reader.ToInt32("matchid"),
                                Turnover = reader.ToDecimal("turnover")
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

        #endregion By Currency

        #region By League

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_League_Event> RMS_Monitor_Parlay_SelectionByTicket_League_Event(int selection, int sportType, int leagueId, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTicket_League_Event>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTicket_League_Event"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);
                    db.AddInParameter("leagueid", DbType.Int32, leagueId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTicket_League_Event
                            {
                                Awayid = reader.ToInt32("awayid"),
                                Homeid = reader.ToInt32("homeid"),
                                Leagueid = reader.ToInt32("leagueid"),
                                Matchid = reader.ToInt32("matchid"),
                                Tickets = reader.ToInt32("tickets")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="leagueId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_League_Event> RMS_Monitor_Parlay_SelectionByTurnover_League_Event(int selection, int sportType, int leagueId, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTurnover_League_Event>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTurnover_League_Event"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int64, 9);
                    db.AddInParameter("sporttype", DbType.Int64, sportType);
                    db.AddInParameter("selection", DbType.Int64, selection);
                    db.AddInParameter("leagueid", DbType.Int64, leagueId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTurnover_League_Event
                            {
                                Awayid = reader.ToInt32("awayid"),
                                Homeid = reader.ToInt32("homeid"),
                                Leagueid = reader.ToInt32("leagueid"),
                                Matchid = reader.ToInt32("matchid"),
                                Turnover = reader.ToDecimal("turnover")
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

        #endregion By League

        #region Event

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event> RMS_Monitor_Parlay_SelectionByTurnover_Event(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTurnover_Event>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTurnover_Event"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTurnover_Event
                            {
                                Awayid = reader.ToInt32("awayid"),
                                Homeid = reader.ToInt32("homeid"),
                                Leagueid = reader.ToInt32("leagueid"),
                                Matchid = reader.ToInt32("matchid"),
                                Turnover = reader.ToDecimal("turnover")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype> RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype(int selection, int sportType, int matchId, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);
                    db.AddInParameter("matchid", DbType.Int32, matchId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype
                            {
                                BetType = reader.ToInt32("bettype"),
                                Margin = reader.ToDecimal("margin"),
                                m = reader.ToInt32("rn"),
                                Turnover = reader.ToDecimal("turnover")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice> RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);
                    db.AddInParameter("matchid", DbType.Int32, matchId);
                    db.AddInParameter("bettype2", DbType.Int32, betType2);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTurnover_Event_Bettype_Choice
                            {
                                BetTeam = reader.ToString("betteam"),
                                Turnover = reader.ToDecimal("turnover")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event> RMS_Monitor_Parlay_SelectionByTicket_Event(int selection, int sportType, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTicket_Event>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTicket_Event"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTicket_Event
                            {
                                Awayid = reader.ToInt32("awayid"),
                                Homeid = reader.ToInt32("homeid"),
                                Leagueid = reader.ToInt32("leagueid"),
                                Matchid = reader.ToInt32("matchid"),
                                Tickets = reader.ToInt32("tickets")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype> RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype(int selection, int sportType, int matchId, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);
                    db.AddInParameter("matchid", DbType.Int32, matchId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype
                            {
                                BetType = reader.ToInt32("bettype"),
                                Margin = reader.ToDecimal("margin"),
                                m = reader.ToInt32("rn"),
                                Tickets = reader.ToInt32("tickets")
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="sportType"></param>
        /// <param name="matchId"></param>
        /// <param name="betType2"></param>
        /// <param name="winlostDate"></param>
        /// <param name="maxTransId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice> RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice(int selection, int sportType, int matchId, int betType2, DateTime winlostDate, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice"))
                {
                    db.AddInParameter("winlostdate", DbType.DateTime, winlostDate);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("bettype", DbType.Int32, 9);
                    db.AddInParameter("sporttype", DbType.Int32, sportType);
                    db.AddInParameter("selection", DbType.Int32, selection);
                    db.AddInParameter("matchid", DbType.Int32, matchId);
                    db.AddInParameter("bettype2", DbType.Int32, betType2);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype_Choice
                            {
                                BetTeam = reader.ToString("betteam"),
                                Tickets = reader.ToInt32("tickets")
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

        #endregion Event
    }
}