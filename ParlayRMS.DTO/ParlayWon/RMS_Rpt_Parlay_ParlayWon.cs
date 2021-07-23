using System;

namespace ParlayRMS.DTO.ParlayWon
{
    public class RMS_Rpt_Parlay_ParlayWon
    {
        public DateTime TransDate { get; set; }

        public string TransId { get; set; }

        public string RefNo { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string CustomerClass { get; set; }

        public string Currency { get; set; }

        public int BetType { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal StakeAfterPT { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal HouseLoseAmount { get; set; }

        public int TotalBet { get; set; }

        public int TotalSelection { get; set; }

        public int SportType { get; set; }

        public string IP { get; set; }
    }
}