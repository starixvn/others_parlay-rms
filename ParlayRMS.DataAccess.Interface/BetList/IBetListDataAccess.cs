namespace ParlayRMS.DataAccess.Interface.BetList
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.BetList;

    public interface IBetListDataAccess
    {
        /// <summary>
        /// RMs the s_ parlay_ trans details.
        /// </summary>
        /// <param name="refNo">The ref no.</param>
        /// <returns></returns>
        IEnumerable<RMS_Parlay_TransDetails> RMS_Parlay_TransDetails(long refNo);
    }
}