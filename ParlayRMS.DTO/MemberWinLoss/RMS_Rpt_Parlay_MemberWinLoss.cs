// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_MemberWinLoss.cs" company="">
// Copyright (c) . All rights reserved.
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

        public string Username { get; set; }

        public bool IsSutotalRow { get; set; }

        public int NumOfSubrows { get; set; }

        /// <summary>
        /// Add other member win loss data to current object.
        /// </summary>
        /// <param name="obj">Other member win loss data object.</param>
        public void Add(RMS_Rpt_Parlay_MemberWinLoss obj)
        {
            this.Tickets_Won += obj.Tickets_Won;
            this.Tickets_Lose += obj.Tickets_Lose;
            this.Stake_Won += obj.Stake_Won;
            this.Stake_Lose += obj.Stake_Lose;
            this.Member_Won += obj.Member_Won;
            this.Member_Lose += obj.Member_Lose;
            this.OurPT_Won += obj.OurPT_Won;
            this.OurPT_Lose += obj.OurPT_Lose;
            this.OurPT_NetWinloss += obj.OurPT_NetWinloss;
            this.NumOfSubrows += 1;
        }
    }
}
