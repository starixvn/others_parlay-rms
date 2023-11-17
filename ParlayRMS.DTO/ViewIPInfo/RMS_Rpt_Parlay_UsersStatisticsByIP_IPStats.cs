namespace ParlayRMS.DTO.ViewIPInfo
{
    public class RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats
    {
        public string IP { get; set; }

        public int? CntUsers { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal HouseLoseAmount { get; set; }
	}
}
