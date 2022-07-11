namespace ParlayRMS.ViewModel.Report
{
    using System;

    public class TopWinnerDetailsOptions
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SportsType { get; set; }

        public string Combos { get; set; }

        public string ParlayTypes { get; set; }

        public byte LiveType { get; set; }

        public string Currencies { get; set; }
    }
}