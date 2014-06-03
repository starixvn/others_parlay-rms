// --------------------------------------------------------------------------
// <copyright file="ICommonDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Common
{
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
    }
}