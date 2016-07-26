// --------------------------------------------------------------------------
// <copyright file="ICommonDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Common
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.Common;

    public interface ICommonDataAccess
    {
        /// <summary>
        /// Gets balance up day.
        /// </summary>
        /// <returns>Balance up day.</returns>
        RMS_SelBalanceUpDay RMS_SelBalanceUpDay();

        /// <summary>
        /// Gets min TRANSDATE from RMBETTRANS table.
        /// </summary>
        /// <returns>Min TRANSDATE.</returns>
        RMS_Rpt_Parlay_SelMinTransdate RMS_Rpt_Parlay_SelMinTransdate();

        /// <summary>
        /// Get the list of bet type belong Bet radar.
        /// </summary>
        /// <returns>Return the list bet type.</returns>
        List<int> GetBettypeBelongsBetRadar();
    }
}