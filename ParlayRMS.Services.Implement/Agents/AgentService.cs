// --------------------------------------------------------------------------
// <copyright file="AgentService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.12">Created</change>
// </history>

namespace ParlayRMS.Services.Implement.Agents
{
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.Agents;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Agents;

    /// <summary>
    /// Agent service.
    /// </summary>
    public class AgentService : BaseService
    {
        private readonly AgentsDataAccess _agentDataAccess = new AgentsDataAccess();

        /// <summary>
        /// Gets dangerous agent list.
        /// </summary>
        /// <param name="username">The user name that is used to filter agent.</param>
        /// <returns>Dangerous agent list.</returns>
        public IEnumerable<RMS_DangerousAgentSel> GetDangerousAgents(string username)
        {
            return this._agentDataAccess.RMS_DangerousAgentSel(username);
        }

        /// <summary>
        /// Inserts a new dangerous agent.
        /// </summary>
        /// <param name="userName">The user name of the dangerous agent.</param>
        /// <param name="adminId">The ID of administrator who adds the agent.</param>
        /// <param name="ip">IP of machine which adds the agent.</param>
        /// <returns>Whether or not the agent is added successfully.</returns>
        public Result AddDangerousAgent(string userName, int adminId, string ip)
        {
            return this._agentDataAccess.RMS_DangerousAgentIns(userName, adminId, ip);
        }

        /// <summary>
        /// Deletes an existing dangerous agent.
        /// </summary>
        /// <param name="agentId">The ID of the dangerous agent.</param>
        /// <param name="userName">The user name of the dangerous agent.</param>
        /// <param name="adminId">The ID of administrator who deletes the agent.</param>
        /// <param name="ip">IP of machine which deletes the agent.</param>
        /// <returns>Whether or not the agent is deleted successfully.</returns>
        public Result DeleteDangerousAgent(int agentId, string userName, int adminId, string ip)
        {
            return this._agentDataAccess.RMS_DangerousAgentDel(agentId, userName, adminId, ip);
        }
    }
}