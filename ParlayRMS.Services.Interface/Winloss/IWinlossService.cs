using ParlayRMS.Models.Winloss;
using ParlayRMS.ViewModel.Report;

namespace ParlayRMS.Services.Interface.Winloss
{
    public interface IWinlossService
    {
        WinlossData GetWinloss(WinlossOptions options);
    }
}