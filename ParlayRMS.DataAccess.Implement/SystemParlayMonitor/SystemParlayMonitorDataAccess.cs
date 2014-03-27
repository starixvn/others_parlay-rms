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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public IEnumerable<RMS_Monitor_SystemParlay_Betlist> RMS_Monitor_SystemParlay_Betlist(
                                                                 string selection,
                                                                 string sportType,
                                                                 string level,
                                                                 decimal stake,
                                                                 long maxTransId,
                                                                 int limitRowCount)
        {
            var result = new List<RMS_Monitor_SystemParlay_Betlist>();

            using (var db = new ObjectDb("RMS_Monitor_SystemParlay_Betlist"))
            {
                db.AddInParameter("selection", DbType.String, selection);
                db.AddInParameter("sporttype", DbType.String, sportType);
                db.AddInParameter("level", DbType.String, level);
                db.AddInParameter("stake", DbType.Decimal, stake);
                db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                db.AddInParameter("limitRowCount", DbType.Int32, limitRowCount);

                using (var dataSet = db.ExecuteDataSet())
                {
                    if (dataSet.Tables.Count == 2)
                    {
                        DataTable ticketList = dataSet.Tables[0];
                        EnumerableRowCollection<DataRow> oddsList = dataSet.Tables[1].AsEnumerable();
                        RMS_Monitor_SystemParlay_Betlist item = null;

                        if (ticketList.Rows.Count > 0)
                        {
                            foreach (DataRow row in ticketList.Rows)
                            {
                                item = new RMS_Monitor_SystemParlay_Betlist
                                {
                                    TransDate = row.ToDateTime("transdate"),
                                    TransId = row.ToInt64("transid"),
                                    RefNo = row.ToInt64("refno"),
                                    Username = row.ToString("username"),
                                    ////Odds = row.ToDecimal("odds"),
                                    Stake = row.ToDecimal("stake"),
                                    ////MaxPayout = row.ToDecimal("maxpayout"),
                                    Combo = row.ToInt32("combo"),
                                    Level = row.ToByte("level")
                                };

                                item.SubOdds = oddsList.Where(oddsRow => oddsRow.ToInt64("refno") == item.RefNo)
                                                       .Select(oddsRow => oddsRow.ToDecimal("odds")).ToList();
                                result.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}