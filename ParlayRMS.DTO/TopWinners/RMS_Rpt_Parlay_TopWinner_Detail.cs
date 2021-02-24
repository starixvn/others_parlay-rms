namespace ParlayRMS.DTO.TopWinners
{
    public class RMS_Rpt_Parlay_TopWinner_Detail
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }

        public decimal Company_Winlost { get; set; }

        public decimal Stake_BaseRM { get; set; }

        public decimal Winlost_BaseRM { get; set; }

        public decimal Company_Winlost_BaseRM { get; set; }

        public int TotalCombo { get; set; }

        public int Selection { get; set; }

        public bool IsSingleMatchParlay { get; set; }

        public int ParlayType { get; set; }

        public bool IsGOF { get; set; }
    }
}