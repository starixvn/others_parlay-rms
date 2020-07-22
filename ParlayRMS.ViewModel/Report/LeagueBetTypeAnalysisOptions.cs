namespace ParlayRMS.ViewModel.Report
{
    using System;

    public class LeagueBetTypeAnalysisOptions
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ParlayType { get; set; }

        public string Leagues { get; set; }

        public string LeagueGroups { get; set; }

        public int Site { get; set; }

        public int ReportBy { get; set; }

        public int Page { get; set; }
    }
}
