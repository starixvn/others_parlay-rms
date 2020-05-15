using ParlayRMS.Web.Mvc.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ParlayRMS.ViewModel.Report
{
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

        public string GetTitle()
        {
            return string.Empty;
        }
    }
}
