// -----------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_Outstanding_Mem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.TotalOutstanding
{
    using System;

    /// <summary>
    /// TODO: Update summary.
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