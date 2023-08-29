using Fanex.Data.Repository;
using System.Collections.Generic;

namespace ParlayRMS.MixParlay.MixParlayMonitor
{
    public class Service : IService
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<Ticket> GetTickets(ViewModel model)
        {
            var criteria = new Criteria
            {
                Combos = model.Combos,
                SportTypes = model.SportTypes,
                Levels = model.Levels,
                Markets = model.Markets,
                ReportType = model.ReportType,
                MaxTransId = model.MaxTransId,
                SelectTop = model.SelectTop,
                Stake = model.StakeAfterPT
            };

            return repository.Fetch<Ticket>(criteria);
        }
    }
}
