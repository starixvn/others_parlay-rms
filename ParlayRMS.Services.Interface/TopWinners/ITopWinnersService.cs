using ParlayRMS.DTO.BetList;
using ParlayRMS.DTO.TopWinners;
using ParlayRMS.ViewModel.Report;
using System.Collections.Generic;

namespace ParlayRMS.Services.Interface.TopWinners
{
    public interface ITopWinnersService
    {
        IEnumerable<RMS_Rpt_Parlay_TopWinner> GetTopWinners(TopWinnersOptions options);

        IEnumerable<RMS_Rpt_Parlay_TopWinner_Detail> GetTopWinnerDetails(TopWinnerDetailsOptions options);

        IEnumerable<RMS_Parlay_TransDetails> GetBetDetail(long refNo);
    }
}