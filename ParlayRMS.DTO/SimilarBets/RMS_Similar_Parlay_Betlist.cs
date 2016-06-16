using System;
using System.Collections.Generic;

namespace ParlayRMS.DTO.SimilarBets
{
    public class RMS_Similar_Parlay_Betlist
    {
        public int Cnt_bets { get; set; }

        public int TotalBet { get; set; }

        public int TotalSelection { get; set; }

        public int Level { get; set; }

        public int SportType { get; set; }

        public string SportTypeName { get; set; }

        public decimal Max_payout { get; set; }

        public decimal Odds { get; set; }

        public long Refno { get; set; }

        public decimal Stake { get; set; }

        public DateTime Transdate { get; set; }

        public long Transid { get; set; }

        public string Username { get; set; }

        public IEnumerable<RMS_Similar_Parlay_Betlist_Detail> Details { get; set; }
    }
}