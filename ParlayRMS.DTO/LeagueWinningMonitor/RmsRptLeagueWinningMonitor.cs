namespace ParlayRMS.DTO.LeagueWinningMonitor
{
    public class RmsRptLeagueWinningMonitor
    {
        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public int TotalStake { get; set; }

        public int TotalWinningAmount { get; set; }

        public int CompanyLoseAmount { get; set; }

        public int TotalTickets { get; set; }
    }
}