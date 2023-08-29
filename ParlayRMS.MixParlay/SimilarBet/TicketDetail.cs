namespace ParlayRMS.MixParlay.SimilarBet
{
    public class TicketDetail
    {
        public long TransId { get; set; }

        public long Refno { get; set; }

        public string Username { get; set; }

        public int Level { get; set; }

        public decimal Net_Win { get; set; }

        public int Similar_Combo { get; set; }
    }
}
