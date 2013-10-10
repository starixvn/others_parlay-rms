// --------------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface
{
    using ParlayRMS.DTO.Common;

    public interface ICommonService
    {
        /// <summary>
        /// Gets balance up day information.
        /// </summary>
        /// <returns>Balance up day information.</returns>
        RMS_SelBalanceUpDay GetBalanceUpDayInfo();
    }
}
