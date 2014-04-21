// --------------------------------------------------------------------------
// <copyright file="IViewIPInfoDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.ViewIPInfo;

    public interface IViewIPInfoDataAccess
    {
        /// <summary>
        /// Get bets statistics by IP and WINLOST date.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <returns>List of view IP info report objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> RMS_Rpt_Parlay_BetsStatisticsByIP(string ip, DateTime date);        
    }
}