// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.MixParlayMonitor
{
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.MixParlayMonitor;
    using ParlayRMS.DTO.MixParlayMonitor;
    using ParlayRMS.Utility.Data;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Mix Parlay Monitor Data Access.
    /// </summary>
    public class MixParlayMonitorDataAccess : BaseDataAccess, IMixParlayMonitorDataAccess
    {
        /// <summary>
        /// Gets Monitor Parlay Bet List.
        /// </summary>
        /// <param name="combos">Combo values.</param>
        /// <param name="sportsTypes">Sport types.</param>
        /// <param name="levels">Level values.</param>
        /// <param name="stakeAfterPT">Stake after PT.</param>
        /// <param name="maxPayout">The max payout.</param>
        /// <param name="reportType">The report type.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <param name="markets">The markets.</param>
        /// <returns>
        /// List of RMS_Monitor_Parlay_BetList objects.
        /// </returns>
        public IEnumerable<RMS_Monitor_ComboParlay_Betlist> RMS_Monitor_ComboParlay_Betlist(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal stakeAfterPT,
            decimal maxPayout,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets)
        {
            var result = new List<RMS_Monitor_ComboParlay_Betlist>();

            using (var db = new ObjectDb("RMS_Monitor_ComboParlay_Betlist"))
            {
                db.AddInParameter("selection", DbType.String, combos);
                db.AddInParameter("parlaytype", DbType.String, parlayTypes);
                db.AddInParameter("sporttype", DbType.String, sportsTypes);
                db.AddInParameter("level", DbType.String, levels);
                db.AddInParameter("stake", DbType.Decimal, stakeAfterPT);
                db.AddInParameter("maxpayout", DbType.Decimal, maxPayout);
                db.AddInParameter("reporttype", DbType.Int32, reportType);
                db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                db.AddInParameter("limitRowCount", DbType.Int32, selectTop);
                db.AddInParameter("market", DbType.String, markets);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Monitor_ComboParlay_Betlist
                        {
                            TransDate = reader.ToDateTime("transdate"),
                            TransDateString = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                            TransId = reader.ToInt64("transid"),
                            RefNo = reader.ToInt64("refno"),
                            Username = reader.ToString("username"),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("stake"),
                            NetWinAfterPT = reader.ToDecimal("maxpayout"),
                            Combo = reader.ToInt32("combo"),
                            IP = reader.ToString("ip"),
                            Level = reader.ToInt32("level"),
                            IsBetRadar = reader.ToBoolean("IsBetRadar")
                        });
                    }
                }
            }

            return result;
        }
    }
}