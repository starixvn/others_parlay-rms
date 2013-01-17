// -----------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_TopWinner_Detail.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.TopWinners
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_Rpt_Parlay_TopWinner_Detail
    {
        public long TransId { get; set; }

        public int SportType { get; set; }

        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }

        public decimal Company_Winlost { get; set; }

        public decimal Stake_BaseRM { get; set; }

        public decimal Winlost_BaseRM { get; set; }

        public decimal Company_Winlost_BaseRM { get; set; }

        public int Combo { get; set; }

        public decimal Odds { get; set; }
    }
}