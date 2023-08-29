using System.Collections.Generic;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public interface IService
    {
        IEnumerable<Ticket> GetTickets(ViewModel viewModel);

        IEnumerable<TicketDetail> GetTicketDetail(DetailViewModel viewModel);
    }
}
