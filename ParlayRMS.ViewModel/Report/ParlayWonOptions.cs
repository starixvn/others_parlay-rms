using ParlayRMS.Utility;
using ParlayRMS.Web.Mvc.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ParlayRMS.ViewModel.Report
{
    using Utility.Data;

    public class ParlayWonOptions
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

        public string Levels { get; set; }

        public decimal HouseAmountFrom { get; set; }

        public decimal HouseAmountTo { get; set; }

        public string Currencies { get; set; }

        public byte Site { get; set; }

        public byte LiveType { get; set; }

        public int? CustomerClass { get; set; }

        public string Ip { get; set; }

        public int SortById { get; set; }

        public bool IsSortAsc { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string GetTitle()
        {
            var combosTitle = ViewLang.All;
            var combos = Combos.Split(',');
            if (combos.Length < 9)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "10" ? ViewLang.TenUp : x));
            }

            var levelTitle = ViewLang.All;
            var levels = Levels.Split(',');
            if (levels.Length < 11)
            {
                levelTitle = string.Join(",", levels.Select(x => x));
            }

            if (levelTitle == "0")
            {
                levelTitle = levelTitle.Replace("0", ViewLang.None);
            }

            levelTitle = levelTitle.Replace("0,", "None,");

            var title =
                $"{ViewLang.ParlayWon} from {FromDate:MMM dd, yyyy} to {ToDate:MMM dd, yyyy} - [{ViewLang.ParlayType}] {ModelsHelper.GetSpecialParlayTitle(ParlayTypes)} - [{ViewLang.Combo}] - {combosTitle} [{ViewLang.SportsType}] - {ModelsHelper.GetSportType(SportsType)} - [{ViewLang.Level}] {levelTitle} - [{ViewLang.Site}] {ModelsHelper.GetSiteTitle(Site)} - [{ViewLang.LiveType}] {ModelsHelper.GetLiveTypeTitle(LiveType)} - {ViewLang.CustomerClass} [{ModelsHelper.GetCustomerClassTitle(CustomerClass)}]";
            return title;
        }
    }
}
