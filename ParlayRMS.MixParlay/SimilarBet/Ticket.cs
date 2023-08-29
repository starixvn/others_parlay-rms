using System;
using System.Collections.Generic;

namespace ParlayRMS.MixParlay.SimilarBet
{
    public class Ticket
    {
        public int Cnt_bets { get; set; }

        public int Combo { get; set; }

        public int Level { get; set; }

        public decimal Net_Win { get; set; }

        public decimal Odds { get; set; }

        public long Refno { get; set; }

        public decimal Stake { get; set; }

        public DateTime Transdate { get; set; }

        public long Transid { get; set; }

        public string Username { get; set; }

        public IEnumerable<TicketDetail> Details { get; set; }
    }
}
