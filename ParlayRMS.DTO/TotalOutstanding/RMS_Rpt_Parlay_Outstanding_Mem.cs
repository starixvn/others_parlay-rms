using System;

namespace ParlayRMS.DTO.TotalOutstanding
{
    public class RMS_Rpt_Parlay_Outstanding_Mem
    {
        public long TransId { get; set; }

        public long Refno { get; set; }

        public DateTime TransDate { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public int TotalBet { get; set; }

        public int TotalSelection { get; set; }

        public int Combo { get; set; }

        public int SportType { get; set; }

        public string SportTypeName { get; set; }
    }
}