// -----------------------------------------------------------------------
// <copyright file="MixParlayDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Implement.MixParlayMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.MixParlayMonitor;
    using ParlayRMS.DTO.MixParlayMonitor;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MixParlayMonitorDataAccess : BaseDataAccess, IMixParlayMonitorDataAccess
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="combos"></param>
        /// <param name="sportsTypes"></param>
        /// <param name="levels"></param>
        /// <param name="maxPayout"></param>
        /// <param name="maxTransId"></param>
        /// <param name="selectTop"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_Betlist> RMS_Monitor_Parlay_Betlist(string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop, long maxTransId)
        {
            var result = new List<RMS_Monitor_Parlay_Betlist>();

            try
            {
                using (var db = new ObjectDb("RMS_Monitor_Parlay_Betlist"))
                {
                    db.AddInParameter("selection", DbType.String, combos);
                    db.AddInParameter("sporttype", DbType.String, sportsTypes);
                    db.AddInParameter("level", DbType.String, levels);
                    db.AddInParameter("maxpayout", DbType.Decimal, maxPayout);
                    db.AddInParameter("max_transid", DbType.Int64, maxTransId);
                    db.AddInParameter("limitRowCount", DbType.Int32, selectTop);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Monitor_Parlay_Betlist
                            {
                                TransDate = reader.ToDateTime("transdate"),
                                TransId = reader.ToInt64("transid"),
                                RefNo = reader.ToInt64("refno"),
                                Username = reader.ToString("username"),
                                Odds = reader.ToDecimal("odds"),
                                Stake = reader.ToDecimal("stake"),
                                MaxPayout = reader.ToDecimal("maxpayout"),
                                Combo = reader.ToInt32("combo"),
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
    }
}