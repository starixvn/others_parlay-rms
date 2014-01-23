// --------------------------------------------------------------------------
// <copyright file="SystemParlayMonitorDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.21">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Implement.SystemParlayMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.SystemParlayMonitor;
    using ParlayRMS.DTO.SystemParlayMonitor;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// System Parlay Monitor Data Access.
    /// </summary>
    public class SystemParlayMonitorDataAccess : BaseDataAccess, ISystemParlayMonitorDataAccess
    {
        /// <summary>
        /// Gets data for system parlay monitor.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="sportType">Type of the sports.</param>
        /// <param name="level">The level.</param>
        /// <param name="stake">The stake.</param>
        /// <param name="maxTransId">The maximum trans identifier.</param>
        /// <param name="limitRowCount">The limit row count.</param>
        /// <returns>Data of system parlay monitor.</returns>
        public IEnumerable<RMS_Monitor_SystemParlay_Betlist> RMS_Monitor_SystemParlay_Betlist(
                                                                 string selection,
                                                                 string sportType,
                                                                 string level,
                                                                 decimal stake,
                                                                 long maxTransId,
                                                                 int limitRowCount)
        {
            var result = new List<RMS_Monitor_SystemParlay_Betlist>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_SystemParlay_Betlist"))
                {
                    db.AddInParameter("selection", DbType.String, selection);
                    db.AddInParameter("sporttype", DbType.String, sportType);
                    db.AddInParameter("level", DbType.String, level);
                    db.AddInParameter("stake", DbType.Decimal, stake);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("limitRowCount", DbType.Int32, limitRowCount);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_SystemParlay_Betlist
                            {
                                TransDate = reader.ToDateTime("transdate"),
                                TransId = reader.ToInt64("transid"),
                                RefNo = reader.ToInt64("refno"),
                                Username = reader.ToString("username"),
                                Odds = reader.ToDecimal("odds"),
                                Stake = reader.ToDecimal("stake"),
                                MaxPayout = reader.ToDecimal("maxpayout"),
                                Combo = reader.ToInt32("combo"),
                                Level = reader.ToByte("level")
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