namespace ParlayRMS.MixParlay.TotalOutstanding
{
    public class AgentTicket
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public int BetCount { get; set; }
    }
}