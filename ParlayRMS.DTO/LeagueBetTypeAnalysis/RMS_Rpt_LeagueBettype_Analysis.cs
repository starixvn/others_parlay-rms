
namespace ParlayRMS.DTO.LeagueBetTypeAnalysis
{
    public class RMS_Rpt_LeagueBettype_Analysis
    {
        public int No { get; set; }

        public string Bettype { get; set; }

        public string GroupName { get; set; }

        public string League { get; set; }

        public int TotalTickets { get; set; }

        public int DrawTickets { get; set; }

        public string Ratio { get; set; }

        public int WonTickets { get; set; }

        public int ParlayWonTickets { get; set; }

        public decimal TurnOver { get; set; }

        public decimal MemberWinloss { get; set; }

        public decimal MainHouseWinloss { get; set; }
    }
}
