// --------------------------------------------------------------------------
// <copyright file="RMS_Monitor_SystemParlay_Betlist.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.21">Created</change>
// </history>

namespace ParlayRMS.DTO.SystemParlayMonitor
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Model for system parlay monitoring function.
    /// </summary>
    public class RMS_Monitor_SystemParlay_Betlist
    {
        public DateTime TransDate { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public string Username { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal NetWinAfterPT { get; set; }

        public int Combo { get; set; }

        public string ComboName { get; set; }

        public byte Level { get; set; }

        public List<decimal> SubOdds { get; set; }
    }
}