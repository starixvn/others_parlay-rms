// -----------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_TopWinner.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.TopWinners
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_Rpt_Parlay_TopWinner
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal Stake { get; set; }

        public decimal Winlost { get; set; }

        public decimal Company_Winlost { get; set; }

        public decimal Stake_BaseRM { get; set; }

        public decimal Winlost_BaseRM { get; set; }

        public decimal Company_Winlost_BaseRM { get; set; }

        public int BetCount { get; set; }

        public decimal Avg_Odds { get; set; }

        public int Level { get; set; }
    }
}