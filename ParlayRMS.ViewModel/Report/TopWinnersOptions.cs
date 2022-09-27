using ParlayRMS.Common.Models;
using ParlayRMS.Utility;
using System;
using System.Linq;

namespace ParlayRMS.ViewModel.Report
{
    public class TopWinnersOptions
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SportsType { get; set; }

        public int SelectTop { get; set; }

        public string Combos { get; set; }

        public byte Site { get; set; }

        public string ParlayTypes { get; set; }

        public byte LiveType { get; set; }

        public string Currencies { get; set; }

        public Int64? CustomerClass { get; set; }

        public string GetTile()
        {
            var combosTitle = ViewLang.All;
            var combos = Combos.Split(',');
            if (combos.Length < 9)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "10" ? ViewLang.TenUp : x));
            }

            string title = string.Format(
                                      "Top {0} winners - from {1} to {2} - {3} [{4}] - {5} [{6}] - {7} [{8}] - {9} [{10}] - {11} [{12} - {13} [{14}]",
                                      SelectTop,
                                      FromDate.ToString("MMM dd, yyyy"),
                                      ToDate.ToString("MMM dd, yyyy"),
                                      ViewLang.Combo,
                                      combosTitle,
                                      ViewLang.ParlayType,
                                      ModelsHelper.GetParlayTitle(ParlayTypes),
                                      ViewLang.SportsType,
                                      ModelsHelper.GetSportType(SportsType),
                                      ViewLang.Site,
                                      ModelsHelper.GetSiteTitle(Site),
                                      ViewLang.CustomerClass,
                                      ModelsHelper.GetCustomerClassTitle(CustomerClass),
                                      ViewLang.LiveType,
                                      ModelsHelper.GetLiveTypeTitle(LiveType));
            return title;
        }
    }
}