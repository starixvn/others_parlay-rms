using System.Collections.Generic;

namespace ParlayRMS.MixParlay.MixParlayMonitor
{
    public interface IService
    {
        IEnumerable<Ticket> GetTickets(ViewModel model);
    }
}
