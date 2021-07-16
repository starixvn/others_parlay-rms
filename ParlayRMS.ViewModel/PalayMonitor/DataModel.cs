using ParlayRMS.Web.Mvc.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ParlayRMS.ViewModel.PalayMonitor
{
    public class DataModel
    {
        public string Combos { get; set; }

        public string ParlayTypes { get; set; }

        public string SportsTypes { get; set; }

        public string Levels { get; set; }

        public string Markets { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal StakeAfterPT { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal NetWinAfterPT { get; set; }

        public int ReportType { get; set; }

        [Required, Values(10, 20, 50, 100, 200, 1000, 2000, 3000)]
        public int SelectTop { get; set; }

        [Required, Range(0, long.MaxValue)]
        public long MaxTransId { get; set; }

        public bool IsCornerParlay { get; set; }

        public decimal? Odds { get; set; }
    }
}