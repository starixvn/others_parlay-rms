using ParlayRMS.DTO.BetList;
using System.Collections.Generic;

namespace ParlayRMS.DataAccess.Interface.BetList
{
    public interface IBetListDataAccess
    {
        IEnumerable<RMS_Parlay_TransDetails> RMS_Parlay_TransDetails(long refNo);

        RMS_Monitor_Parlay_BetInfo RMS_Monitor_Parlay_BetInfo(long transId);
    }
}