namespace ParlayRMS.DTO.SimilarBets
{
    public class RMS_Similar_Parlay_Betlist_Detail
    {
        public long TransId { get; set; }

        public long Refno { get; set; }

        public string Username { get; set; }

        public int Level { get; set; }

        public decimal Max_Payout { get; set; }

        public int Similar_Selection { get; set; }
    }
}