// --------------------------------------------------------------------------
// <copyright file="RMS_SelBalanceUpDay.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Common
{
    using System;

    /// <summary>
    /// Balance Up Day model.
    /// </summary>
    public class RMS_SelBalanceUpDay
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}