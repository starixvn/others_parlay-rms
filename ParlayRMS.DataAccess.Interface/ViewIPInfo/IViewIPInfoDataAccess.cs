// --------------------------------------------------------------------------
// <copyright file="IViewIPInfoDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.DataAccess.Interface.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DTO.ViewIPInfo;

    /// <summary>
    /// Main data-access-interface for view-IP-info report and other related reports.
    /// </summary>
    public interface IViewIPInfoDataAccess
    {
        /// <summary>
        /// Get bets statistics by IP and WINLOST date.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <returns>List of bets statistics objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> RMS_Rpt_Parlay_BetsStatisticsByIP(string ip, DateTime date);

        /// <summary>
        /// Get bets statistics by IP, WINLOST date, user id and sport type.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <param name="userId">Input user ID value.</param>
        /// <param name="sportType">Input sport type value.</param>
        /// <returns>List of bets statistics objects.</returns>
        IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> RMS_Rpt_Parlay_BetsStatisticsByIP_Sporttype(string ip, DateTime date, int userId, int sportType);

        /// <summary>
        /// Get Parlay combination details.
        /// </summary>
        /// <param name="refNo">REFNO value.</param>
        /// <returns>Parlay bet combination details.</returns>
        RMS_SystemParlay_CombinationDetails RMS_SystemParlay_CombinationDetails(long refNo);
    }
}