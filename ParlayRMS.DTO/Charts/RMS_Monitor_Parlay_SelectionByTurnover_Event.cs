namespace ParlayRMS.DTO.Charts
{
    public class RMS_Monitor_Parlay_SelectionByTurnover_Event
    {
        public int Awayid { get; set; }

        public int Homeid { get; set; }

        public int Leagueid { get; set; }

        public int Matchid { get; set; }

        public decimal Turnover { get; set; }
    }
}