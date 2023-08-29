using Fanex.BetList.Core.Entities;
using System.Collections.Generic;

namespace ParlayRMS.MixParlay.BetList
{
    public interface IService
    {
        IEnumerable<ITicket> CreateChoice(long refNo);
    }
}
