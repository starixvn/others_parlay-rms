using System;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public class TicketDetail
    {
        public DateTime TransDate { get; set; }

        public long TransId { get; set; }

        public decimal Odds { get; set; }

        public long RefNo { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public string Ip { get; set; }

        public decimal HousePt { get; set; }

        public int Combo { get; set; }
    }
}