// --------------------------------------------------------------------------
// <copyright file="IAgentsDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.12">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Interface.Agents
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Agents;

    public interface IAgentsDataAccess
    {
        /// <summary>
        /// Gets dangerous agent list.
        /// </summary>
        /// <param name="username">The user name that is used to filter agent.</param>
        /// <returns>Dangerous agent list.</returns>
        IEnumerable<RMS_DangerousAgentSel> RMS_DangerousAgentSel(string username);

        /// <summary>
        /// Inserts a new dangerous agent.
        /// </summary>
        /// <param name="userName">The user name of the dangerous agent.</param>
        /// <param name="adminId">The ID of administrator who adds the agent.</param>
        /// <param name="ip">IP of machine which adds the agent.</param>
        /// <returns>Whether or not the agent is added successfully.</returns>
        Result RMS_DangerousAgentIns(string userName, int adminId, string ip);

        /// <summary>
        /// Deletes an existing dangerous agent.
        /// </summary>
        /// <param name="agentId">The ID of the dangerous agent.</param>
        /// <param name="userName">The user name of the dangerous agent.</param>
        /// <param name="adminId">The ID of administrator who deletes the agent.</param>
        /// <param name="ip">IP of machine which deletes the agent.</param>
        /// <returns>Whether or not the agent is deleted successfully.</returns>
        Result RMS_DangerousAgentDel(int agentId, string userName, int adminId, string ip);
    }
}