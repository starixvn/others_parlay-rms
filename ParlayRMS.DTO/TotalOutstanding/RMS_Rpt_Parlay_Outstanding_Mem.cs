// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_Outstanding_Mem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.TotalOutstanding
{
    using System;

    /// <summary>
    /// Parlay Outstanding model.
    /// </summary>
    public class RMS_Rpt_Parlay_Outstanding_Mem
    {
        public long TransId { get; set; }

        public long Refno { get; set; }

        public DateTime TransDate { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public int Combo { get; set; }
    }
}