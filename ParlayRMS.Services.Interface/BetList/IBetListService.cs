using Fanex.BetList.Core.Entities;
using ParlayRMS.DTO.BetList;
using System.Collections.Generic;

namespace ParlayRMS.Services.Interface.BetList
{
    public interface IBetListService
    {
        IEnumerable<ITicket> CreateChoice(long refNo);

        RMS_Monitor_Parlay_BetInfo GetBetInfo(long transId);
    }
}