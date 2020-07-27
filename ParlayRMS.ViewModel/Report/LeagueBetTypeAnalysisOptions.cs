namespace ParlayRMS.ViewModel.Report
{
    using Common.Models;
    using System;
    using Utility;

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

        public int Page { get; set; }

        public string GetTitle()
        {
            var title = string.Format(
                                "{0} from {1} to {2} - [{3}] {4} - [{5}] - {6} - [{7}] {8}",
                                ViewLang.LeagueReport,
                                FromDate.ToString("MMMM dd, yyyy"),
                                ToDate.ToString("MMMM dd, yyyy"),
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
