namespace ParlayRMS.DTO.LeagueWinningMonitor
{
    public class RmsRptLeagueWinningMonitor
    {
        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal TotalStake { get; set; }

        public decimal TotalWinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public int TotalTickets { get; set; }
    }
}