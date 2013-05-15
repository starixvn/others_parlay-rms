// -----------------------------------------------------------------------
// <copyright file="TotalOutstandingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.TotalOutstanding
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.TotalOutstanding;
    using ParlayRMS.DataAccess.Interface.TotalOutstanding;
    using ParlayRMS.Services.Interface.TotalOutstanding;
    using ParlayRMS.DTO.TotalOutstanding;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalOutstandingService : ITotalOutstandingService
    {
        private readonly ITotalOutstandingDataAccess dataaccess = new TotalOutstandingDataAccess();

        public List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(DateTime date, string combo, string sportsType, long custId)
        {
            return dataaccess.RMS_Rpt_Parlay_Outstanding_Mem(custId, date, combo, sportsType);
        }

        public List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(DateTime date, string combo, string sportsType, long custId)
        {
            return dataaccess.RMS_Rpt_Parlay_Outstanding_Ag(custId, date, combo, sportsType);
        }
    }
}