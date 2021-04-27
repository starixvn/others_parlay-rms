using ParlayRMS.Models;
using ParlayRMS.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace ParlayRMS.ViewModel
{
    public static class Config
    {
        private static List<SelectListItem> _sportsTypes;
        private static List<SelectListItem> _selectTops;
        private static List<SelectListItem> _dateRanges;
        private static List<SelectListItem> _combos;
        private static List<SelectListItem> _parlayTypes;
        private static List<SelectListItem> _reportTypes;
        private static List<SelectListItem> _leagueGroups;
        private static List<SelectListItem> _sites;
        private static List<SelectListItem> _reportBy;
        private static List<SelectListItem> _simpleParlayTypes;
        private static List<SelectListItem> _ticketSelections;

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        static Config()
        {
            _sportsTypes = new List<SelectListItem>()
            {
                new SelectListItem { Text = ViewLang.Allsports, Value = ((int)SportsType.AllSports).ToString(), Selected = true },
                new SelectListItem { Text = ViewLang.Socceronly, Value = ((int)SportsType.SoccerOnly).ToString() },
                new SelectListItem { Text = ViewLang.Combinedsports, Value = ((int)SportsType.CombinedSports).ToString() },
                new SelectListItem { Text = ViewLang.Othersports, Value = ((int)SportsType.OtherSports).ToString() }
            };

            var values = new List<int> { 10, 20, 50, 100 };
            _selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                _selectTops.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString(),
                    Selected = item == 100 ? true : false
                });
            }

            _dateRanges = new List<SelectListItem>()
            {
                new SelectListItem { Text = ViewLang.Yesterday, Value = ((int)DateRange.Yesterday).ToString() },
                new SelectListItem { Text = ViewLang.CurrentWeek, Value = ((int)DateRange.CurrentWeek).ToString() },
                new SelectListItem { Text = ViewLang.CurrentMonth, Value = ((int)DateRange.CurrentMonth).ToString() },
                new SelectListItem { Text = ViewLang.LastWeek, Value = ((int)DateRange.LastWeek).ToString() },
                new SelectListItem { Text = ViewLang.LastMonth, Value = ((int)DateRange.LastMonth).ToString() },
                new SelectListItem { Text = ViewLang.SinceLastMonth, Value = ((int)DateRange.SinceLastMonth).ToString() },
                new SelectListItem { Text = ViewLang.SpecificDate, Value = ((int)DateRange.SpecificDate).ToString() }
            };

            _combos = new List<SelectListItem>();
            for (int i = 2; i <= 9; i++)
            {
                _combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            _combos.Add(new SelectListItem { Text = ViewLang.TenUp, Value = "10" });

            _parlayTypes = new List<SelectListItem>()
            {
                new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() },
                new SelectListItem { Text = ViewLang.GodOfFortune, Value = ((int)ParlayType.GodOfFortune).ToString() },
                new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() },
                new SelectListItem { Text = ViewLang.PenaltyShootoutCombo, Value = ((int)ParlayType.PenaltyShootoutCombo).ToString() },
                new SelectListItem { Text = ViewLang.SingleMatchParlay, Value = ((int)ParlayType.SingleMatchParlay).ToString() }
            };

            _simpleParlayTypes = new List<SelectListItem>()
            {
                new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() },
                new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() }
            };

            _reportTypes = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Combo", Value = ((int)ReportType.Combo).ToString(), Selected = true },
                new SelectListItem { Text = "Sport type", Value = ((int)ReportType.SportType).ToString() }
            };

            _leagueGroups = new List<SelectListItem> {
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
                new SelectListItem { Text = "Blacklist", Value = "8" },

            };

            _reportBy = new List<SelectListItem> {
                new SelectListItem { Text = "League", Value = "1", Selected = true },
                new SelectListItem { Text = "BetType", Value = "2" },

            };

            _sites = new List<SelectListItem> {
                new SelectListItem { Text = "All", Value = "2" },
                new SelectListItem { Text = "Credit", Value = "0" },
                new SelectListItem { Text = "Deposit", Value = "1" },

            };

            _ticketSelections = new List<SelectListItem> {
                new SelectListItem { Text = "All", Value = "All" },
                new SelectListItem { Text = "Live", Value = "Live" },
                new SelectListItem { Text = "Non Live", Value = "Non Live" },
                new SelectListItem { Text = "Mix", Value = "Mix" }

            };
        }

        public static List<SelectListItem> Combos
        {
            get
            {
                return _combos;
            }
        }

        public static List<SelectListItem> DateRanges
        {
            get
            {
                return _dateRanges;
            }
        }

        public static List<SelectListItem> SelectTops
        {
            get
            {
                return _selectTops;
            }
        }

        public static List<SelectListItem> SportsTypes
        {
            get
            {
                return _sportsTypes;
            }
        }

        public static List<SelectListItem> ParlayTypes
        {
            get
            {
                return _parlayTypes;
            }
        }

        public static List<SelectListItem> SimpleParlayTypes
        {
            get
            {
                return _simpleParlayTypes;
            }
        }

        public static List<SelectListItem> ReportTypes
        {
            get
            {
                return _reportTypes;
            }
        }

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

        public static List<SelectListItem> LeagueGroups
        {
            get
            {
                return _leagueGroups;
            }
        }

        public static List<SelectListItem> Sites
        {
            get
            {
                return _sites;
            }
        }

        public static List<SelectListItem> ReportBy
        {
            get
            {
                return _reportBy;
            }
        }

        public static List<SelectListItem> TicketSelections
        {
            get
            {
                return _ticketSelections;
            }
        }
    }
}