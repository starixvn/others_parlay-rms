namespace ParlayRMS.DTO.Charts
{
    public class RMS_Monitor_Parlay_SelectionbyTicket_Currency
    {
        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public decimal Margin { get; set; }

        public int NoTickets { get; set; }
    }
}