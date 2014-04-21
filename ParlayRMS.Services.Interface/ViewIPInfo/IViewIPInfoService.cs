// --------------------------------------------------------------------------
// <copyright file="IViewIPInfoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.ViewIPInfo;

    /// <summary>
    /// IViewIPInfoService interface.
    /// </summary>
    public interface IViewIPInfoService
    {
        /// <summary>
        /// Get bets statistics base on specific IP and date.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <returns>List of bets statistics.</returns>
        IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> GetIPInfo(string ip, DateTime date);
    }
}