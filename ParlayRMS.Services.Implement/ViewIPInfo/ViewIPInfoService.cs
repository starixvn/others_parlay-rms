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

    public class ViewIPInfoService : BaseService, IViewIPInfoService
    {
        private readonly IViewIPInfoDataAccess _viewIPInfoDataAccess = new ViewIPInfoDataAccess();
        private readonly ICommonService _commonService = new CommonService();

        public IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> GetIPInfo(string ip, DateTime date)
        {
            var info = _commonService.GetBalanceUpDayInfo();
            return this._viewIPInfoDataAccess.RMS_Rpt_Parlay_BetsStatisticsByIP(ip, date);
        }        
    }
}