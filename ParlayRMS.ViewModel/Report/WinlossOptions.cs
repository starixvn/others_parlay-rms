using ParlayRMS.Models;
using ParlayRMS.Utility;
using ParlayRMS.Web.Mvc.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ParlayRMS.ViewModel.Report
{
    using Utility.Data;

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

        public byte Site { get; set; }

        public byte LiveType { get; set; }

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

            var title =
                $"{ViewLang.WinLoss} - from {FromDate:MMM dd, yyyy} to {ToDate:MMM dd, yyyy} - {ViewLang.ParlayType} [{ModelsHelper.GetParlayTitle(ParlayTypes)}] - {ViewLang.Combo} [{combosTitle}] - {ViewLang.SportsType} [{ModelsHelper.GetSportType(SportsType)}] - Report by [{ModelsHelper.GetReportType(ReportType)}] - {ViewLang.Site} [{ModelsHelper.GetSiteTitle(Site)}] - {ViewLang.LiveType}[{ModelsHelper.GetLiveTypeTitle(LiveType)}]";
            return title;
        }
    }
}
