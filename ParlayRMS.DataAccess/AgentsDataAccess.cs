namespace ParlayRMS.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria;
    using Criteria.Agents;
    using DTO.Agents;

    public class AgentsDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();
        
        public IEnumerable<RMS_DangerousAgentSel> RMS_DangerousAgentSel(string username)
        {
            username = string.IsNullOrEmpty(username) ? null : username;
            var criteria = new DangerousAgentSel_Criteria { Username = username };

            var agents = repository.Fetch<RMS_DangerousAgentSel>(criteria).ToList();

            var counter = 1;
            agents.ForEach(agent => { agent.No = counter++; });

            return agents;
        }

        public Result RMS_DangerousAgentIns(string userName, int adminId, string ip)
        {
            var command = new DangerousAgentIns_Command
            {
                Username = userName,
                UserId = adminId,
                Ip = ip
            };

            repository.Execute(command);

            return ResultFactory.From(command);
        }

        public Result RMS_DangerousAgentDel(int agentId, string userName, int adminId, string ip)
        {
            var command = new DangerousAgentDel_Command
            {
                UserId = adminId,
                AgentId = agentId,
                Ip = ip,
                Username = userName
            };

            repository.Execute(command);

            return ResultFactory.From(command);
        }
    }
}
