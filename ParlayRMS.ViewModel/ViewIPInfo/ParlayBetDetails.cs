namespace ParlayRMS.ViewModel.ViewIPInfo
{
    using System;

    public class ParlayBetDetails
    {
        public DateTime Date { get; set; }

        public object Event { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public int Status { get; set; }
    }
}