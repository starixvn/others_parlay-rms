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

    public interface IViewIPInfoService
    {
        IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> GetIPInfo(string ip, DateTime date);
    }
}