// --------------------------------------------------------------------------
// <copyright file="ParlayBetDetails.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.24">Created</change>
// </history>

namespace ParlayRMS.ViewModel.ViewIPInfo
{
    using System;

    /// <summary>
    /// Parlay Bet Details.
    /// </summary>
    public class ParlayBetDetails
    {
        public DateTime Date { get; set; }

        public object Event { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public int Status { get; set; }
    }
}