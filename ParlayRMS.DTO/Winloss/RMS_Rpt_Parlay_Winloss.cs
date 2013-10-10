// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_Winloss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Winloss
{
    using System;

    /// <summary>
    /// Model for Parlay Win Loss Report.
    /// </summary>
    public class RMS_Rpt_Parlay_Winloss
    {
        public DateTime WinlostDate { get; set; }

        public string Combo { get; set; }

        public int Tickets_Won { get; set; }

        public int Tickets_Lose { get; set; }

        public decimal Stake_Won { get; set; }

        public decimal Stake_Lose { get; set; }

        public decimal Member_Won { get; set; }

        public decimal Member_Lose { get; set; }

        public decimal OurPT_Won { get; set; }

        public decimal OurPT_Lose { get; set; }

        public decimal OurPT_NetWinloss { get; set; }
    }
}