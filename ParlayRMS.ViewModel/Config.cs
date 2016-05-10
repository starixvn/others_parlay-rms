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
        private static List<SelectListItem> _sportsTypes;
        private static List<SelectListItem> _selectTops;
        private static List<SelectListItem> _dateRanges;
        private static List<SelectListItem> _combos;
        private static List<SelectListItem> _parlayTypes;

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
                new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() }
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
    }
}