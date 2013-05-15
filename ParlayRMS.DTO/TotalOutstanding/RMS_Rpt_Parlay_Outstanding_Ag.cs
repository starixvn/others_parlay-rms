// -----------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_Outstanding_Ag.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.TotalOutstanding
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_Rpt_Parlay_Outstanding_Ag
    {
        public long CustId { get; set; }

        public string Username { get; set; }

        public decimal StakePt { get; set; }

        public decimal MaxPayoutPt { get; set; }

        public int BetCount { get; set; }
    }
}