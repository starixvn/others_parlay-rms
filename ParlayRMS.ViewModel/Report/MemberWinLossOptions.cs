using ParlayRMS.Models;
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

        [AllowHtml]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        public static DateTime GetLimitDate()
        {
            string memberWinLossLimitDate = AppSetting.MemberWinLossLimitDate;
            DateTime limitDate = Convert.ToDateTime(memberWinLossLimitDate);
            DateTime minDate = DateTime.Now.AddYears(-1);
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

            var title = string.Format(
                                "{0} of {1} - from {2} to {3} - {4} [{5}] - {6} [{7}] - {8} [{9}] - {10}[{11}]",
                                ViewLang.AgentMemberWinLoss,
                                Username,
                                FromDate.ToString("MMM dd, yyyy"),
                                ToDate.ToString("MMM dd, yyyy"),
                                ViewLang.ParlayType,
                                ModelsHelper.GetParlayTitle(ParlayTypes),
                                ViewLang.Combo,
                                combosTitle,
                                ViewLang.SportsType,
                                ModelsHelper.GetSportType(SportsType),
                                ViewLang.LiveType,
                                ModelsHelper.GetLiveTypeTitle(LiveType));
            return title;
        }
    }
}