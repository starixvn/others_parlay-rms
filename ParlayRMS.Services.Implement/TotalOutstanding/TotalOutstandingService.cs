// -----------------------------------------------------------------------
// <copyright file="TotalOutstandingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.TotalOutstanding
{
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.TotalOutstanding;
    using ParlayRMS.DataAccess.Interface.TotalOutstanding;
    using ParlayRMS.DTO.TotalOutstanding;
    using ParlayRMS.Services.Interface.TotalOutstanding;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalOutstandingService : ITotalOutstandingService
    {
        private readonly ITotalOutstandingDataAccess dataaccess = new TotalOutstandingDataAccess();

        public List<RMS_Rpt_Parlay_Outstanding_Mem> GetTotalOutstandingForMember(long custId, string combo, string sportsType)
        {
            return dataaccess.RMS_Rpt_Parlay_Outstanding_Mem(custId, combo, sportsType);
        }

        public List<RMS_Rpt_Parlay_Outstanding_Ag> GetTotalOutstandingForAgent(long custId, string combo, string sportsType)
        {
            return dataaccess.RMS_Rpt_Parlay_Outstanding_Ag(custId, combo, sportsType);
        }
    }
}