using Fanex.BetList.Core.Entities;
using ParlayRMS.DTO.BetList;
using System.Collections.Generic;

namespace ParlayRMS.Services.Interface.BetList
{
    public interface IBetListService
    {
        BetListData GetBetListData(long refNo);

        IEnumerable<ITicket> CreateChoice(long refNo);

        RMS_Monitor_Parlay_BetInfo GetBetInfo(long transId);

        IEnumerable<ITicket> Map(long refNo, IEnumerable<RMS_Parlay_TransDetails> data);
    }
}