// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_MemberWinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.DTO.MemberWinLoss
{
    using System;

    /// <summary>
    /// DTO for Member Win Loss report.
    /// </summary>
    public class RMS_Rpt_Parlay_MemberWinLoss
    {
        public string Month { get; set; }

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
