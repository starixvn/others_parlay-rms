using ParlayRMS.Common.Models;
using ParlayRMS.Models;
using ParlayRMS.Utility;
using ParlayRMS.Web.Mvc.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ParlayRMS.ViewModel.Report
{
    public class WinlossOptions
    {
        [Required(ErrorMessage = "FromDate is required")]
        [DateRange("1900/01/01", "9999/12/31")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "ToDate is required")]
        [DateRange("1900/01/01", "9999/12/31")]
        public DateTime ToDate { get; set; }

        public string ParlayTypes { get; set; }

        public string SportsType { get; set; }

        public string Combos { get; set; }

        public ReportType ReportType { get; set; }

        public string GetTitle()
        {
            var combosTitle = ViewLang.All;
            var combos = Combos.Split(',');
            if (combos.Length < 9)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "10" ? ViewLang.TenUp : x));
            }

            var title = string.Format(
                                   "{0} - from {1} to {2} - {3} [{4}] - {5} [{6}] - {7} [{8}] - Report by [{9}]",
                                   ViewLang.WinLoss,
                                   FromDate.ToString("MMMM dd, yyyy"),
                                   ToDate.ToString("MMMM dd, yyyy"),
                                   ViewLang.ParlayType,
                                   ModelsHelper.GetParlayTitle(ParlayTypes),
                                   ViewLang.Combo,
                                   combosTitle,
                                   ViewLang.SportsType,
                                   ModelsHelper.GetSportType(SportsType),
                                   ModelsHelper.GetReportType(ReportType));
            return title;
        }
    }
}