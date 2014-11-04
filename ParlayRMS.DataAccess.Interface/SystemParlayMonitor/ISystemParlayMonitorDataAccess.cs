// --------------------------------------------------------------------------
// <copyright file="ISystemParlayMonitorDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.21">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Interface.SystemParlayMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.SystemParlayMonitor;

    public interface ISystemParlayMonitorDataAccess
    {
        /// <summary>
        /// Gets data for system parlay monitor.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="sportType">Type of the sports.</param>
        /// <param name="level">The level.</param>
        /// <param name="stake">The stake.</param>
        /// <param name="maxPayout">The max payout.</param>
        /// <param name="reportType">The report type.</param>
        /// <param name="maxTransId">The maximum trans identifier.</param>
        /// <param name="limitRowCount">The limit row count.</param>
        /// <param name="markets">The markets.</param>
        /// <returns>
        /// Data of system parlay monitor.
        /// </returns>
        IEnumerable<RMS_Monitor_SystemParlay_Betlist> RMS_Monitor_SystemParlay_Betlist(
            string selection,
            string sportType,
            string level,
            decimal stake,
            decimal maxPayout,
            int reportType,
            long maxTransId,
            int limitRowCount,
            string markets);
    }
}