using System.Collections.Generic;

namespace ParlayRMS.MixParlay.TotalOutstanding
{
    public interface IService
    {
        IEnumerable<MemberTicket> GetMemberTickets(MemberCriteria criteria);

        IEnumerable<AgentTicket> GetAgentTickets(AgentCriteria criteria);
    }
}
