// --------------------------------------------------------------------------
// <copyright file="DangerMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.Services.Implement.DangerMonitor
{
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.DangerMonitor;
    using ParlayRMS.DataAccess.Interface.DangerMonitor;
    using ParlayRMS.DTO.DangerMonitor;
    using ParlayRMS.Services.Interface.DangerMonitor;
    using ParlayRMS.Utility;

    /// <summary>
    /// Danger monitor service.
    /// </summary>
    public class DangerMonitorService : BaseService, IDangerMonitorService
    {
        private readonly IDangerMonitorDataAccess _topwinnersDataAccess = new DangerMonitorDataAccess();

        /// <summary>
        /// Get summary betting data of a dangerous agent.
        /// </summary>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor objects.</returns>
        public IEnumerable<RMS_Rpt_Parlay_DangerMonitor> GetDangerMonitorData(
                                                            string agentIdList,
                                                            string selection,
                                                            out long maxTransId)
        {
            var records = this._topwinnersDataAccess.RMS_Rpt_Parlay_DangerMonitor(agentIdList, selection, out maxTransId);
            string homeName;
            string awayName;

            records.ToList().ForEach(obj =>
            {
                homeName = CacheRef.GetTeamName(obj.HomeId);
                awayName = CacheRef.GetTeamName(obj.AwayId);
                obj.LeagueName = CacheRef.GetLeagueName(obj.LeagueId);
                obj.MatchName = string.Format(ViewLang.MatchNameFormat, homeName, awayName);
            });

            return records;
        }

        /// <summary>
        /// Get betting details of a specific match of a dangerous agent.
        /// </summary>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor_Details objects.</returns>
        public IEnumerable<RMS_Rpt_Parlay_DangerMonitor_Details> GetDangerMonitorDetails(
                                                                     int matchId,
                                                                     string agentIdList,
                                                                     string selection,
                                                                     long maxTransId)
        {
            return this._topwinnersDataAccess.RMS_Rpt_Parlay_DangerMonitor_Details(matchId, agentIdList, selection, maxTransId);
        }
    }
}
