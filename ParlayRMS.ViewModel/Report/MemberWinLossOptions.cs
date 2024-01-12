using ParlayRMS.Utility;
using ParlayRMS.Web.Mvc.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.ViewModel.Report
{
    using Utility.Data;

    public class MemberWinLossOptions
    {
        [Required(ErrorMessage = "FromDate is required")]
        [DateRange("1900/01/01", "9999/12/31")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "ToDate is required")]
        [DateRange("1900/01/01", "9999/12/31")]
        public DateTime ToDate { get; set; }

        public string ParlayTypes { get; set; }

        public string SportsType { get; set; }

        public byte LiveType { get; set; }

        public string Combos { get; set; }

        public int? PageIndex { get; set; }

        public bool IsCached { get; set; }

        public byte? SortById { get; set; }

        public bool IsSortAsc { get; set; }

        public byte ReportBy { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        public static DateTime GetLimitDate()
        {
            var memberWinLossLimitDate = AppSetting.MemberWinLossLimitDate;
            var limitDate = Convert.ToDateTime(memberWinLossLimitDate);
            var minDate = DateTime.Now.AddYears(-1);
            return minDate > limitDate ? minDate : limitDate;
        }

        public string GetTitle()
        {
            var combosTitle = ViewLang.All;
            var combos = Combos.Split(',');
            if (combos.Length < 9)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "10" ? ViewLang.TenUp : x));
            }

            var title =
                $"{ViewLang.AgentMemberWinLoss} of {Username} - from {FromDate:MMM dd, yyyy} to {ToDate:MMM dd, yyyy} - {ViewLang.ParlayType} [{ModelsHelper.GetSpecialParlayTitle(ParlayTypes)}] - {ViewLang.Combo} [{combosTitle}] - {ViewLang.SportsType} [{ModelsHelper.GetSportType(SportsType)}] - {ViewLang.LiveType}[{ModelsHelper.GetLiveTypeTitle(LiveType)}]";
            return title;
        }
    }
}
