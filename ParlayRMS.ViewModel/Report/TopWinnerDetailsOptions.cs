// --------------------------------------------------------------------------
// <copyright file="TopWinnerDetailsOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Report
{
    using System;

    /// <summary>
    /// Top Winner Details Options.
    /// </summary>
    public class TopWinnerDetailsOptions
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SportsType { get; set; }

        public string Combos { get; set; }
    }
}