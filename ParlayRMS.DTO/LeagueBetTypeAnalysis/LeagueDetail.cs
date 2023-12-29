namespace ParlayRMS.DTO.LeagueBetTypeAnalysis
{
	public class LeagueDetail: ModelBase
	{
		public int No { get; set; }

		public string Bettype { get; set; }

		public string Event { get; set; }

		public string BetChoice { get; set; }

		public int DrawTickets { get; set; }

		public int WonTickets { get; set; }

		public int ParlayWonTickets { get; set; }

		public decimal TurnOver { get; set; }

		public decimal MemberWinloss { get; set; }
	}
}
