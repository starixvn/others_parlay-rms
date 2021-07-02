namespace ParlayRMS.ViewModel.LeagueWinningMonitor
{
    public class LeagueWinningMonitorModel
    {
        public string Combos { get; set; }

        public string SportsTypes { get; set; }

        public string ParlayTypes { get; set; }

        public string Levels { get; set; }

        public decimal WinningAmount { get; set; }

        public int SelectTop { get; set; }

        public byte Group { get; set; }

        public byte ReportBy { get; set; }
    }
}