using ParlayRMS.Models;
using ParlayRMS.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.ViewModel
{
    public static class Config
    {
        static Config()
        {
            SportsTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.Allsports, Value = ((int)SportsType.AllSports).ToString(), Selected = true },
                new SelectListItem { Text = ViewLang.Socceronly, Value = ((int)SportsType.SoccerOnly).ToString() },
                new SelectListItem { Text = ViewLang.Combinedsports, Value = ((int)SportsType.CombinedSports).ToString() },
                new SelectListItem { Text = ViewLang.Othersports, Value = ((int)SportsType.OtherSports).ToString() }
            };

            var values = new List<int> { 10, 20, 50, 100 };
            SelectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                SelectTops.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString(),
                    Selected = item == 100
                });
            }

            DateRanges = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.Yesterday, Value = ((int)DateRange.Yesterday).ToString() },
                new SelectListItem { Text = ViewLang.CurrentWeek, Value = ((int)DateRange.CurrentWeek).ToString() },
                new SelectListItem { Text = ViewLang.CurrentMonth, Value = ((int)DateRange.CurrentMonth).ToString() },
                new SelectListItem { Text = ViewLang.LastWeek, Value = ((int)DateRange.LastWeek).ToString() },
                new SelectListItem { Text = ViewLang.LastMonth, Value = ((int)DateRange.LastMonth).ToString() },
                new SelectListItem { Text = ViewLang.SinceLastMonth, Value = ((int)DateRange.SinceLastMonth).ToString() },
                new SelectListItem { Text = ViewLang.SpecificDate, Value = ((int)DateRange.SpecificDate).ToString() }
            };

            Combos = new List<SelectListItem>();
            for (var i = 2; i <= 9; i++)
            {
                Combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            Combos.Add(new SelectListItem { Text = ViewLang.TenUp, Value = "10" });

            ParlayTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() },
                new SelectListItem { Text = ViewLang.GodOfFortune, Value = ((int)ParlayType.GodOfFortune).ToString() },
                new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() },
                new SelectListItem { Text = ViewLang.PenaltyShootoutCombo, Value = ((int)ParlayType.PenaltyShootoutCombo).ToString() },
                new SelectListItem { Text = ViewLang.SingleMatchParlay, Value = ((int)ParlayType.SingleMatchParlay).ToString() }
            };

            SimpleParlayTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() },
                new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() }
            };

            ReportTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Combo", Value = ((int)ReportType.Combo).ToString(), Selected = true },
                new SelectListItem { Text = "Sport type", Value = ((int)ReportType.SportType).ToString() }
            };

            LeagueGroups = new List<SelectListItem> {
                new SelectListItem { Text = ViewLang.None, Value = "-1" },
                new SelectListItem { Text = "MJ", Value = "1" },
                new SelectListItem { Text = "M1", Value = "2" },
                new SelectListItem { Text = "M2", Value = "3" },
                new SelectListItem { Text = "M3", Value = "4" },
                new SelectListItem { Text = "M4 (a)", Value = "5" },
                new SelectListItem { Text = "M4 (b)", Value = "6" },
                new SelectListItem { Text = "M4 (x)", Value = "7" },
                new SelectListItem { Text = "Virtual", Value = "9" },
                new SelectListItem { Text = "CUP", Value = "10" },
                new SelectListItem { Text = "CORNER", Value = "11" },
                new SelectListItem { Text = "WORLD CUP", Value = "12" },
                new SelectListItem { Text = "Blacklist", Value = "8" },

            };

            ReportBy = new List<SelectListItem> {
                new SelectListItem { Text = "League", Value = "1", Selected = true },
                new SelectListItem { Text = "BetType", Value = "2" },

            };

            Sites = new List<SelectListItem> {
                new SelectListItem { Text = "All", Value = "2" },
                new SelectListItem { Text = "Credit", Value = "0" },
                new SelectListItem { Text = "Deposit", Value = "1" },

            };

            LiveTypes = new List<SelectListItem> {
                new SelectListItem { Text = "All", Value = "3" },
                new SelectListItem { Text = "Live", Value = "1" },
                new SelectListItem { Text = "Non Live", Value = "0" },
                new SelectListItem { Text = "Mix", Value = "2" }

            };
        }

        public static List<SelectListItem> Combos { get; }

        public static List<SelectListItem> DateRanges { get; }

        public static List<SelectListItem> SelectTops { get; }

        public static List<SelectListItem> SportsTypes { get; }

        public static List<SelectListItem> ParlayTypes { get; }

        public static List<SelectListItem> SimpleParlayTypes { get; }

        public static List<SelectListItem> ReportTypes { get; }

        public static DateTime WinlostDate
        {
            get
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebug"]))
                {
                    return new DateTime(2012, 11, 6).Date;
                }
                else
                {
                    return DateTime.Now.AddDays(-1).Date;
                }
            }
        }

        public static List<SelectListItem> LeagueGroups { get; }

        public static List<SelectListItem> Sites { get; }

        public static List<SelectListItem> ReportBy { get; }

        public static List<SelectListItem> LiveTypes { get; }
    }
}
