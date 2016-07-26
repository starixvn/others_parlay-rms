// --------------------------------------------------------------------------
// <copyright file="IDangerMonitorService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.Services.Interface.DangerMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.DangerMonitor;

    public interface IDangerMonitorService
    {
        /// <summary>
        /// Get summary betting data of a dangerous agent.
        /// </summary>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="maxPayout">Max payout after PT.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_DangerMonitor> GetDangerMonitorData(
                                                      string agentIdList,
                                                      decimal maxPayout,
                                                      string selection,
                                                      out long maxTransId);

        /// <summary>
        /// Get betting details of a specific match of a dangerous agent.
        /// </summary>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="maxPayout">Max payout after PT.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor_Details objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_DangerMonitor_Details> GetDangerMonitorDetails(
                                                              int matchId,
                                                              string agentIdList,
                                                              decimal maxPayout,
                                                              string selection,
                                                              long maxTransId);
    }
}