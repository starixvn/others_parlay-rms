// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_Outstanding_Ag.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.TotalOutstanding
{
    /// <summary>
    /// Parlay Outstanding model for Agent.
    /// </summary>
    public class RMS_Rpt_Parlay_Outstanding_Ag
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public int BetCount { get; set; }
    }
}