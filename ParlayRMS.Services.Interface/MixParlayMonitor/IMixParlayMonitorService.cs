// -----------------------------------------------------------------------
// <copyright file="MixParlayService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Interface.MixParlayMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.MixParlayMonitor;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMixParlayMonitorService
    {
        /// <summary>
        /// Gets the mix parlay monitor tickets.
        /// </summary>
        /// <param name="combo">The combo.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="maxPayout">The max payout.</param>
        /// <param name="level">The level.</param>
        /// <param name="selectTop">The select top.</param>
        /// <returns></returns>
        IEnumerable<RMS_Monitor_Parlay_Betlist> GetMixParlayMonitorTickets(string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop, long maxTransId);
    }
}