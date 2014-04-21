// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.ViewIPInfo
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.ViewIPInfo;
    using ParlayRMS.DataAccess.Interface.ViewIPInfo;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Services.Implement.Common;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.ViewIPInfo;

    /// <summary>
    /// ViewIPInfo Service.
    /// </summary>
    public class ViewIPInfoService : BaseService, IViewIPInfoService
    {
        private readonly IViewIPInfoDataAccess _viewIPInfoDataAccess = new ViewIPInfoDataAccess();

        /// <summary>
        /// Get bets statistics base on specific IP and date.
        /// </summary>
        /// <param name="ip">Input IP value.</param>
        /// <param name="date">Input date value.</param>
        /// <returns>List of bets statistics.</returns>
        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> GetIPInfo(string ip, DateTime date)
        {
            return this._viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP(ip, date);
        }        
    }
}