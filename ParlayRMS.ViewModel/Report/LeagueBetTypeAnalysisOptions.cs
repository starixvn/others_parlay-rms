namespace ParlayRMS.ViewModel.Report
{
    using System;
    using Utility;
    using Utility.Data;

    public class LeagueBetTypeAnalysisOptions
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ParlayType { get; set; }

        public string Leagues { get; set; }

        public string LeagueGroups { get; set; }

        public int Site { get; set; }

        public double Ratio { get; set; }

        public int ReportBy { get; set; }

        public int SportType { get; set; }

        public byte? SortById { get; set; }

        public bool IsSortAsc { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string GetTitle()
        {
            var title = string.Format(
                                "{0} from {1} to {2} - [{3}] {4} - [{5}] - {6} - [{7}] {8}",
                                ViewLang.LeagueReport,
                                FromDate.ToString("MMM dd, yyyy"),
                                ToDate.ToString("MMM dd, yyyy"),
                                ViewLang.ParlayType,
                                ModelsHelper.GetSimpleParlayTitle(ParlayType),
                                ViewLang.Group,
                                ModelsHelper.GetLeagueGroupTitle(LeagueGroups),
                                ViewLang.Site,
                                ModelsHelper.GetSiteTitle(Site));
            return title;
        }
    }
}
