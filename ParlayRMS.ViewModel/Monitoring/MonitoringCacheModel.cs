namespace ParlayRMS.ViewModel.Monitoring
{
    using System;

    public class MonitoringCacheModel
    {
        public MonitoringCacheModel()
        {
            WinlostDate = Config.WinlostDate;
        }

        public int BetType { get; set; }

        public DateTime WinlostDate { get; set; }

        public int Selection { get; set; }

        public string SportType { get; set; }

        public bool IsTicket { get; set; }

        public int Currency { get; set; }

        public string TopCurrency { get; set; }

        public int LeagueId { get; set; }

        public bool IsSelectDetail { get; set; }

        public bool IsBettype { get; set; }

        public bool IsChoice { get; set; }

        public bool IsEvent { get; set; }

        public int MatchId { get; set; }

        public int Bettype2 { get; set; }

        public bool IsValid()
        {
            return Selection == 0;
        }
    }
}
