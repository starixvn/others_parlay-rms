using ParlayRMS.DTO.MemberWinLoss;

namespace ParlayRMS.DTO.ViewIPInfo
{
    public class RMS_Rpt_Parlay_ViewIPInfo
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public int Soccer { get; set; }

        public int Others { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal HouseLoseAmount { get; set; }

		public string SiteGroup { get; set; }

		public void Add(RMS_Rpt_Parlay_ViewIPInfo obj)
		{
			Stake += obj.Stake;
			WinningAmount += obj.WinningAmount;
			HouseLoseAmount += obj.HouseLoseAmount;
			Soccer += obj.Soccer;
			Others += obj.Others;
		}
	}
}
