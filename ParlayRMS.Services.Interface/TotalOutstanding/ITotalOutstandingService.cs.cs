// -----------------------------------------------------------------------
// <copyright file="ITotalOutstandingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Interface.TotalOutstanding
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.TotalOutstanding;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITotalOutstandingService
    {
        List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(long custId, string combo, string sportsType);

        List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(long custId, string combo, string sportsType);
    }
}