namespace ParlayRMS.DTO.SimilarBets
{
    public class TotalDataModel
    {
        public int SimilarBetsTotal { get; set; }
        public decimal Stake { get; set; }

        public decimal MaxPayout { get; set; }

        public decimal PotentialWinningAmount { get; set; }

        public decimal HouseLoseAmount { get; set; }
    }
}
