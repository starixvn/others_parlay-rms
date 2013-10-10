// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_TopWinner.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.TopWinners
{
    /// <summary>
    /// Parlay Top Winner model.
    /// </summary>
    public class RMS_Rpt_Parlay_TopWinner
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }

        public decimal Company_Winlost { get; set; }

        public decimal Stake_BaseRM { get; set; }

        public decimal Winlost_BaseRM { get; set; }

        public decimal Company_Winlost_BaseRM { get; set; }

        public int BetCount { get; set; }

        public decimal Avg_Odds { get; set; }

        public int Level { get; set; }
    }
}