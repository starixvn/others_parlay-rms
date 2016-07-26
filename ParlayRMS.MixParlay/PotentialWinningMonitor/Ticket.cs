using System;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public class Ticket
    {
        public DateTime TransDate { get; set; }

        public long CustId { get; set; }

        public string Username { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public int Level { get; set; }

        public int TotalBets { get; set; }
    }
}