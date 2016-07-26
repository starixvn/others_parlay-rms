using System;
using System.Web.Mvc;

namespace ParlayRMS.MixParlay.TotalOutstanding
{
    public class ViewModel
    {
        public DateTime Date { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public int SearchBy { get; set; }

        public string Combo { get; set; }

        public string SportsType { get; set; }
    }
}