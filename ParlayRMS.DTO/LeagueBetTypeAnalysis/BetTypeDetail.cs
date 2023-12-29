namespace ParlayRMS.DTO.LeagueBetTypeAnalysis
{
	public class BetTypeDetail: ModelBase
	{
		public int No { get; set; }

		public string GroupName { get; set; }

		public string League { get; set; }

		public string Event { get; set; }

		public int DrawTickets { get; set; }

		public int WonTickets { get; set; }

		public int ParlayWonTickets { get; set; }

		public decimal TurnOver { get; set; }

		public decimal MemberWinloss { get; set; }
	}
}
