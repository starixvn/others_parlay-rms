// --------------------------------------------------------------------------
// <copyright file="DangerMonitorDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Implement.DangerMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.DangerMonitor;
    using ParlayRMS.DTO.DangerMonitor;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Danger monitor data access layer.
    /// </summary>
    public class DangerMonitorDataAccess : BaseDataAccess, IDangerMonitorDataAccess
    {
        /// <summary>
        /// Get dangerous members' bet data.
        /// </summary>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxPayout">Max payout after PT.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor objects.</returns>
        public IEnumerable<RMS_Rpt_Parlay_DangerMonitor> RMS_Rpt_Parlay_DangerMonitor(
                                                            string agentIdList,
                                                            string selection,
                                                            decimal maxPayout,
                                                            out long maxTransId)
        {
            var result = new List<RMS_Rpt_Parlay_DangerMonitor>();
            maxTransId = 0;

            using (var db = new ObjectDb("RMS_Rpt_Parlay_DangerMonitor"))
            {
                db.AddInParameter("AgentIDList", DbType.String, agentIdList);
                db.AddInParameter("maxpayout", DbType.Decimal, maxPayout);
                db.AddInParameter("selection", DbType.String, selection);
                db.AddOutParameter("max_transid", DbType.Int64, 8);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_DangerMonitor
                        {
                            LeagueId = reader.ToInt32("leagueid"),
                            MatchId = reader.ToInt32("matchid"),
                            HomeId = reader.ToInt32("homeid"),
                            AwayId = reader.ToInt32("awayid"),
                            HdpHome = reader.ToInt32("hdp_home"),
                            HdpAway = reader.ToInt32("hdp_away"),
                            Hdp1stHome = reader.ToInt32("hdp_1st_home"),
                            Hdp1stAway = reader.ToInt32("hdp_1st_away"),
                            OUHome = reader.ToInt32("ou_home"),
                            OUAway = reader.ToInt32("ou_away"),
                            OU1stHome = reader.ToInt32("ou_1st_home"),
                            OU1stAway = reader.ToInt32("ou_1st_away")
                        });
                    }
                }

                maxTransId = Convert.ToInt64(db.GetParameterValue("max_transid"));
            }

            return result;
        }

        /// <summary>
        /// Get dangerous members' bet details data.
        /// </summary>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxPayout">Max payout after PT.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor_Details objects.</returns>
        public IEnumerable<RMS_Rpt_Parlay_DangerMonitor_Details> RMS_Rpt_Parlay_DangerMonitor_Details(
                                                                     int matchId,
                                                                     string agentIdList,
                                                                     string selection,
                                                                     decimal maxPayout,
                                                                     long maxTransId)
        {
            var result = new List<RMS_Rpt_Parlay_DangerMonitor_Details>();

            using (var db = new ObjectDb("RMS_Rpt_Parlay_DangerMonitor_Details"))
            {
                db.AddInParameter("matchid", DbType.Int32, matchId);
                db.AddInParameter("AgentIDList", DbType.String, agentIdList);
                db.AddInParameter("maxpayout", DbType.Decimal, maxPayout);
                db.AddInParameter("selection", DbType.String, selection);
                db.AddInParameter("max_transid", DbType.Int64, maxTransId);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_Rpt_Parlay_DangerMonitor_Details
                        {
                            Hdp = reader.ToDecimal("hdp"),
                            Odds = reader.ToDecimal("odds"),
                            HdpHome = reader.ToInt32("hdp_home"),
                            HdpAway = reader.ToInt32("hdp_away"),
                            Hdp1stHome = reader.ToInt32("hdp_1st_home"),
                            Hdp1stAway = reader.ToInt32("hdp_1st_away"),
                            OUHome = reader.ToInt32("ou_home"),
                            OUAway = reader.ToInt32("ou_away"),
                            OU1stHome = reader.ToInt32("ou_1st_home"),
                            OU1stAway = reader.ToInt32("ou_1st_away")
                        });
                    }
                }
            }

            return result;
        }
    }
}