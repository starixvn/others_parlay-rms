// --------------------------------------------------------------------------
// <copyright file="IBetListDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.BetList
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.BetList;

    public interface IBetListDataAccess
    {
        /// <summary>
        /// Gets transaction details of a bet.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>List of transactions.</returns>
        IEnumerable<RMS_Parlay_TransDetails> RMS_Parlay_TransDetails(long refNo);
    }
}