// -----------------------------------------------------------------------
// <copyright file="IMixParlayDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DataAccess.Interface.MixParlayMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.MixParlayMonitor;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMixParlayMonitorDataAccess
    {
        /// <summary>
        /// Gets the mix parlay tickets.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="sportsType">Type of the sports.</param>
        /// <param name="level">The level.</param>
        /// <param name="maxPayout">The max payout.</param>
        /// <param name="maxTransId">The max trans id.</param>
        /// <param name="selectTop">The select top.</param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_Betlist> RMS_Monitor_Parlay_Betlist(string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop, long maxTransId);
    }
}