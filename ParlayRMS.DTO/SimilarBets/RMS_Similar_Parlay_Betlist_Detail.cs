namespace ParlayRMS.DTO.SimilarBets
{
    public class RMS_Similar_Parlay_Betlist_Detail
    {
        public long TransId { get; set; }

        public long Refno { get; set; }

        public string Username { get; set; }

        public int CustId { get; set; }

        public int Level { get; set; }

        public decimal Stake { get; set; }

        public decimal Max_Payout { get; set; }

        public decimal PotentialWinningAmount { get; set; }

        public decimal HouseLoseAmount { get; set; }

        public int Similar_Selection { get; set; }

        public string CustomerClass { get; set; }
    }
}