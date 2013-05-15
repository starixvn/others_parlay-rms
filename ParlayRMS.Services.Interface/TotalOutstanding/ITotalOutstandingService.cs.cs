// -----------------------------------------------------------------------
// <copyright file="ITotalOutstandingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Interface.TotalOutstanding
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.TotalOutstanding;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITotalOutstandingService
    {
        List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(DateTime date, string combo, string sportsType, long custId);

        List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(DateTime date, string combo, string sportsType, long custId);
    }
}