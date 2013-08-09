// -----------------------------------------------------------------------
// <copyright file="IWinlossService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Interface.Winloss
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.Models.Winloss;
    using ParlayRMS.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinlossService
    {
        WinlossData GetWinloss(DateTime fromDate, DateTime toDate, BetType betType, SportsType sportsType, string combos);
    }
}