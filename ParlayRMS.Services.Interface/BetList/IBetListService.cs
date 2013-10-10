// --------------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.BetList
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    public interface IBetListService
    {
        /// <summary>
        /// Creates choice.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>List of tickets.</returns>
        IEnumerable<ITicket> CreateChoice(long refNo);
    }
}