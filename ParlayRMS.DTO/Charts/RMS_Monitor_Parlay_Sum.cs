namespace ParlayRMS.DTO.Charts
{
    public class RMS_Monitor_Parlay_Sum
    {
        public int Events { get; set; }

        public string Status { get; set; }

        public int Tickets { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal CompanyWL { get; set; }
    }
}