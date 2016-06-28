// --------------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.Common;

    public interface ICommonService
    {
        /// <summary>
        /// Gets balance up day information.
        /// </summary>
        /// <returns>Balance up day information.</returns>
        RMS_SelBalanceUpDay GetBalanceUpDayInfo();

        /// <summary>
        /// Get min TRANSDATE from RMBETTRANS table.
        /// </summary>
        /// <returns>Min TRANSDATE.</returns>
        RMS_Rpt_Parlay_SelMinTransdate GetMinTransDate();

        /// <summary>
        /// Get the list of bet type belong Bet radar.
        /// </summary>
        /// <returns>Return the list bet type.</returns>
        List<int> GetBettypeBelongsBetRadar();
    }
}
