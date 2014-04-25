// --------------------------------------------------------------------------
// <copyright file="IViewIPInfoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.Services.Interface.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Models.ViewIpInfo;

    /// <summary>
    /// Main service interface for view-IP-info report and other related reports.
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

        /// <summary>
        /// Get bets statistics base on specific IP, date, user id and sport type.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <param name="userId">Input user id value.</param>
        /// <param name="sportType">Input sport type value.</param>
        /// <returns>List of bets statistics.</returns>
        IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> GetIPInfoByUser(string ip, DateTime date, int userId, int sportType);

        /// <summary>
        /// Get Parlay combination details.
        /// </summary>
        /// <param name="refNo">REFNO value.</param>
        /// <returns>List of combination details view model, group by date.</returns>
        CombinationDetailsByDateViews GetBetCombinationDetails(long refNo);
    }
}