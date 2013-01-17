namespace ParlayRMS.Services.Interface.BetList
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    public interface IBetListService
    {
        IEnumerable<ITicket> CreateChoice(long refNo);
    }
}