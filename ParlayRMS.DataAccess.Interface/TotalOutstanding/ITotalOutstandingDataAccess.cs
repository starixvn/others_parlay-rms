// -----------------------------------------------------------------------
// <copyright file="ITotalOutstandingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Interface.TotalOutstanding
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.TotalOutstanding;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITotalOutstandingDataAccess
    {
        List<RMS_Rpt_Parlay_Outstanding_Mem> RMS_Rpt_Parlay_Outstanding_Mem(long custId, string combo, string sportsType);

        List<RMS_Rpt_Parlay_Outstanding_Ag> RMS_Rpt_Parlay_Outstanding_Ag(long custId, string combo, string sportsType);
    }
}