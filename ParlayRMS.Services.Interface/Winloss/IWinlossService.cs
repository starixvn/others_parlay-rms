using ParlayRMS.Models;
using ParlayRMS.Models.Winloss;
using System;

namespace ParlayRMS.Services.Interface.Winloss
{
    public interface IWinlossService
    {
        WinlossData GetWinloss(DateTime fromDate, DateTime toDate, string parlayTypes, SportsType sportsType, string combos);
    }
}