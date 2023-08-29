using Fanex.Data;
using Fanex.Data.Repository;
using System.Collections.Generic;

namespace ParlayRMS.MixParlay.TotalOutstanding
{
    public class Service : IService
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<AgentTicket> GetAgentTickets(AgentCriteria criteria)
        {
            return repository.Fetch<AgentTicket>(criteria);
        }

        public IEnumerable<MemberTicket> GetMemberTickets(MemberCriteria criteria)
        {
            return repository.Fetch<MemberTicket>(criteria);
        }
    }
}
