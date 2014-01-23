// --------------------------------------------------------------------------
// <copyright file="ISystemParlayMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.21">Created</change>
// </history>

namespace ParlayRMS.Services.Interface.SystemParlayMonitor
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.SystemParlayMonitor;

    public interface ISystemParlayMonitorService
    {
        /// <summary>
        /// Gets data for system parlay monitor.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="sportType">Type of the sports.</param>
        /// <param name="level">The level.</param>
        /// <param name="stake">The stake.</param>
        /// <param name="maxTransId">The maximum trans identifier.</param>
        /// <param name="limitRowCount">The limit row count.</param>
        /// <returns>Data of system parlay monitor.</returns>
        IEnumerable<RMS_Monitor_SystemParlay_Betlist> GetSystemParlayMonitorData(
                                                          string selection,
                                                          string sportType,
                                                          string level,
                                                          decimal stake,
                                                          long maxTransId,
                                                          int limitRowCount);
    }
}