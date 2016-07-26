using Fanex.Data.Repository;
using System.Collections.Generic;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public class Service : IService
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<Ticket> GetTickets(ViewModel viewModel)
        {
            var criteria = new Criteria
            {
                Combos = viewModel.Combos,
                SportTypes = viewModel.SportTypes,
                Levels = viewModel.Levels,
                WinningAmount = viewModel.WinningAmount,
                SelectTop = viewModel.SelectTop
            };

            return repository.Fetch<Ticket>(criteria);
        }

        public IEnumerable<TicketDetail> GetTicketDetail(DetailViewModel viewModel)
        {
            var criteria = new DetailCriteria
            {
                CustId = viewModel.CustId,
                Combos = viewModel.Combos,
                SportTypes = viewModel.SportTypes
            };

            return repository.Fetch<TicketDetail>(criteria);
        }
    }
}