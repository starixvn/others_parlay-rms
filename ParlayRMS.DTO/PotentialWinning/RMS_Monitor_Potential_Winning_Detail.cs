using System;

namespace ParlayRMS.DTO.PotentialWinning
{
    public class RMS_Monitor_Potential_Winning_Detail
    {
        public DateTime TransDate { get; set; }

        public string TransDateString { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }


        public decimal WinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public string Ip { get; set; }

        public decimal HousePT { get; set; }

        public int TotalBet { get; set; }

        public int TotalSelection { get; set; }

        public int NumberWon { get; set; }

        public string SportTypeName { get; set; }
    }
}