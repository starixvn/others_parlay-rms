// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Implement.ViewIPInfo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Main data-access-class for view-IP-info report and other related reports.
    /// </summary>
    public class ViewIPInfoDataAccess : BaseDataAccess
    {
        /// <summary>
        /// Get bets statistics by IP and WINLOST date.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <returns>List of view IP info objects.</returns>
        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> RMS_Rpt_Parlay_BetsStatisticsByIP(string ip, DateTime date)
        {
            var result = new List<RMS_Rpt_Parlay_ViewIPInfo>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_BetsStatisticsByIP"))
            {
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("fromdate", DbType.DateTime, date);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_ViewIPInfo
                        {
                            UserId = reader.ToInt32("custid"),
                            Username = reader.ToString("username"),
                            Soccer = reader.ToInt32("cnt_soccer"),
                            Others = reader.ToInt32("cnt_others"),
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get IP statistics by date.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="pageIndex">Page index value.</param>
        /// <param name="pageSize">Page size value.</param>
        /// <param name="totalItems">Total items value.</param>
        /// <param name="maxTransId">Maximum Transaction ID value.</param>
        /// <returns>User statistics by IP object.</returns>
        public RMS_Rpt_Parlay_UsersStatisticsByIP RMS_Rpt_Parlay_UsersStatisticsByIP(DateTime fromDate, int pageIndex, int pageSize, int totalItems, long maxTransId)
        {
            var result = new RMS_Rpt_Parlay_UsersStatisticsByIP();
            var ipStats = new List<RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_UsersStatisticsByIP"))
            {
                Hashtable param = new Hashtable();
                param.Add("fromdate", fromDate);
                param.Add("pageindex", pageIndex);
                param.Add("pagesize", pageSize);
                param.Add("totalitems", totalItems);
                param.Add("max_transid", maxTransId);

                using (var reader = db.ExecuteReader(param))
                {
                    while (reader.Read())
                    {
                        ipStats.Add(new RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats
                        {
                            IP = reader.ToString("IP"),
                            CntUsers = reader.ToInt32("cntUsers")
                        });
                    }
                }

                result.IPStats = ipStats;
                result.TotalItems = Convert.ToInt32(db.GetParameterValue("totalitems"));
                result.MaxTransId = Convert.ToInt64(db.GetParameterValue("max_transid"));
            }

            return result;
        }

        /// <summary>
        /// Get bets statistics by IP, WINLOST date, user id and sport type.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <param name="userId">Input user ID value.</param>
        /// <param name="sportType">Input sport type value.</param>
        /// <returns>List of view IP info by user objects.</returns>
        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> RMS_Rpt_Parlay_BetsStatisticsByIP_Sporttype(string ip, DateTime date, int userId, int sportType)
        {
            var result = new List<RMS_Rpt_Parlay_ViewIPInfoByUser>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_BetsStatisticsByIP_Sporttype"))
            {
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("fromdate", DbType.DateTime, date);
                db.AddInParameter("custid", DbType.Int32, userId);
                db.AddInParameter("sporttype", DbType.String, sportType);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_ViewIPInfoByUser
                        {
                            TransID = reader.ToInt64("transid"),
                            RefNo = reader.ToInt64("refno"),
                            TransDate = reader.ToDateTime("transdate"),
                            BetType = reader.ToInt32("bettype"),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("stake"),
                            StatusId = reader.ToInt32("statusid"),
                            Status = reader.ToString("status"),
                            Winlost = reader.ToDecimal("winlost"),
                            Hdp1 = Convert.ToInt32(reader.ToDecimal("hdp1")),
                            Hdp2 = Convert.ToInt32(reader.ToDecimal("hdp2")),
                            BetTeam = reader.ToString("betteam"),
                            BetCheck = reader.ToString("betcheck"),
                            HomeId = reader.ToInt32("homeid"),
                            AwayId = reader.ToInt32("awayid"),
                            SportTypeId = reader.ToInt32("sporttype"),
                            LeagueId = reader.ToInt32("leagueid"),
                            IsNeutral = reader.ToBoolean("IsNeutral"),
                            EventDate = reader.ToDateTime("eventdate")
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get Parlay combination details.
        /// </summary>
        /// <param name="refNo">REFNO value.</param>
        /// <returns>CombinationDetails model.</returns>
        public RMS_SystemParlay_CombinationDetails RMS_SystemParlay_CombinationDetails(long refNo)
        {
            var result = new RMS_SystemParlay_CombinationDetails();
            var combos = new List<RMS_SystemParlay_CombinationDetails_Combo>();
            var comboRefs = new List<RMS_SystemParlay_CombinationDetails_ComboRef>();
            decimal outStanding = -1;
            using (var db = new ObjectDb("RMS_SystemParlay_CombinationDetails"))
            {
                db.AddInParameter("refno", DbType.Int64, refNo);
                db.AddOutParameter("Outstanding", DbType.Decimal, 8);
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        combos.Add(new RMS_SystemParlay_CombinationDetails_Combo
                        {
                            WinLostDate = DbConverter.ToDateTime(reader["winlostdate"]),
                            TransID = DbConverter.ToInt64(reader["transid"]),
                            Serial = DbConverter.ToInt32(reader["serial"]),
                            StatusId = DbConverter.ToInt32(reader["statusid"]),
                            Status = DbConverter.ToString(reader["status"]),
                            Modds = DbConverter.ToDecimal(reader["modds"]),
                            Stake = DbConverter.ToDecimal(reader["stake"]),
                            Winlost = DbConverter.ToDecimal(reader["winlost"])
                        });
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            comboRefs.Add(new RMS_SystemParlay_CombinationDetails_ComboRef
                            {
                                Serial = DbConverter.ToInt32(reader["serial"]),
                                HomeId = DbConverter.ToInt32(reader["homeid"]),
                                AwayId = DbConverter.ToInt32(reader["awayid"])
                            });
                        }
                    }
                }

                outStanding = Convert.ToInt64(db.GetParameterValue("Outstanding"));
            }

            result.Outstanding = outStanding;
            result.Combo = combos;
            result.ComboRef = comboRefs;
            return result;
        }
    }

}