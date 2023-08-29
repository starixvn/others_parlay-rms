namespace ParlayRMS.ViewModel.TotalOutstanding
{
    using System;
    using System.Web.Mvc;

    public class DataModel
    {
        public DateTime Date { get; set; }

        public string Combo { get; set; }

        public string ParlaysType { get; set; }

        public string SportsType { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public int SearchBy { get; set; }

        public bool IsCornerParlay { get; set; }
    }
}
