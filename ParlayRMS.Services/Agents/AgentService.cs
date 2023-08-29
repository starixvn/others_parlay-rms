namespace ParlayRMS.Services.Agents
{
    using System.Collections.Generic;
    using DataAccess.Agents;
    using DTO;
    using ParlayRMS.DTO.Agents;

    public class AgentService : BaseService
    {
        private readonly AgentsDataAccess _agentDataAccess = new AgentsDataAccess();

        public IEnumerable<RMS_DangerousAgentSel> GetDangerousAgents(string username)
        {
            return _agentDataAccess.RMS_DangerousAgentSel(username);
        }

        public Result AddDangerousAgent(string userName, int adminId, string ip)
        {
            return _agentDataAccess.RMS_DangerousAgentIns(userName, adminId, ip);
        }

        public Result DeleteDangerousAgent(int agentId, string userName, int adminId, string ip)
        {
            return _agentDataAccess.RMS_DangerousAgentDel(agentId, userName, adminId, ip);
        }
    }
}
