using ParlayRMS.Utility;
using System;
using System.Linq;

namespace ParlayRMS.ViewModel.Report
{
    using Utility.Data;

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

        public int? CustomerClass { get; set; }

        public string GetTile()
        {
            var combosTitle = ViewLang.All;
            var combos = Combos.Split(',');
            if (combos.Length < 9)
            {
                combosTitle = string.Join(",", combos.Select(x => x == "10" ? ViewLang.TenUp : x));
            }

            var title =
                $"Top {SelectTop} winners - from {FromDate:MMM dd, yyyy} to {ToDate:MMM dd, yyyy} - {ViewLang.Combo} [{combosTitle}] - {ViewLang.ParlayType} [{ModelsHelper.GetParlayTitle(ParlayTypes)}] - {ViewLang.SportsType} [{ModelsHelper.GetSportType(SportsType)}] - {ViewLang.Site} [{ModelsHelper.GetSiteTitle(Site)}] - {ViewLang.CustomerClass} [{ModelsHelper.GetCustomerClassTitle(CustomerClass)}] - {ViewLang.LiveType} [{ModelsHelper.GetLiveTypeTitle(LiveType)}]";
            return title;
        }
    }
}
