// --------------------------------------------------------------------------
// <copyright file="IDangerMonitorDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Interface.DangerMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.DangerMonitor;

    public interface IDangerMonitorDataAccess
    {
        /// <summary>
        /// Get dangerous members' bet data.
        /// </summary>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxPayout">Max payout after PT.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_DangerMonitor> RMS_Rpt_Parlay_DangerMonitor(
                                                      string agentIdList,
                                                      string selection,
                                                      decimal maxPayout,
                                                      out long maxTransId);

        /// <summary>
        /// Get dangerous members' bet details data.
        /// </summary>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="agentIdList">The agent identifier list.</param>
        /// <param name="selection">The combo selection.</param>
        /// <param name="maxPayout">Max payout after PT.</param>
        /// <param name="maxTransId">The maximum transaction identifier.</param>
        /// <returns>List of RMS_RPT_Parlay_DangerMonitor_Details objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_DangerMonitor_Details> RMS_Rpt_Parlay_DangerMonitor_Details(
                                                              int matchId,
                                                              string agentIdList,
                                                              string selection,
                                                              decimal maxPayout,
                                                              long maxTransId);
    }
}