using System;

namespace ParlayRMS.MixParlay.MixParlayMonitor
{
    public class Ticket
    {
        public DateTime TransDate { get; set; }

        public string TransDateString { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public string Username { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal MaxPayout { get; set; }

        public int Combo { get; set; }

        public string IP { get; set; }

        public int Level { get; set; }

        public bool IsBetRadar { get; set; }
    }
}