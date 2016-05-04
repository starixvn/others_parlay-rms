using System;

namespace ParlayRMS.MixParlay.TotalOutstanding
{
    public class MemberTicket
    {
        public long TransId { get; set; }

        public long Refno { get; set; }

        public DateTime TransDate { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public string Combo { get; set; }
    }
}