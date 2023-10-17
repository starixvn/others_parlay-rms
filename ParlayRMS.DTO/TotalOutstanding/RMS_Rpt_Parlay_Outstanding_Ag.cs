namespace ParlayRMS.DTO.TotalOutstanding
{
    public class RMS_Rpt_Parlay_Outstanding_Ag
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public int BetCount { get; set; }

        public string CustomerClass { get; set; }
    }
}
