namespace ParlayRMS.DTO.Charts
{
    public class RMS_Monitor_Parlay_SelectionByTicket_Event_Bettype
    {
        public int BetType { get; set; }

        public int Tickets { get; set; }

        public decimal Margin { get; set; }

        public int RowNumber { get; set; }
    }
}