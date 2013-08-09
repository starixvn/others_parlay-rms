// -----------------------------------------------------------------------
// <copyright file="IWinlossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Interface.Winloss
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.Winloss;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinlossDataAccess
    {
        IEnumerable<RMS_Rpt_Parlay_Winloss> RMS_Rpt_Parlay_Winloss(DateTime fromDate, DateTime toDate, DateTime moveBetsDay, DateTime balanceUpDay, int betType, int sportsType, string combos);
    }
}