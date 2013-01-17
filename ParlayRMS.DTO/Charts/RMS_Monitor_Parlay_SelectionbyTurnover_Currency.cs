namespace ParlayRMS.DTO.Charts
{
    public class RMS_Monitor_Parlay_SelectionbyTurnover_Currency
    {
        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public decimal Margin { get; set; }

        public decimal Turnover { get; set; }
    }
}