namespace ParlayRMS.DTO.PotentialWinning
{
    using System;

    public class RMS_Monitor_Potential_Winning
    {
        public DateTime TransDate { get; set; }

        public string TransDateString { get; set; }

        public long CustId { get; set; }

        public string UserName { get; set; }

        public string Currency { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal CompanyLoseAmount { get; set; }

        public int TotalTicket { get; set; }

        public int Level { get; set; }
    }
}