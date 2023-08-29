namespace ParlayRMS.Models.MemberWinLoss
{
    using System.Collections.Generic;
    using ParlayRMS.DTO.MemberWinLoss;

    public class MemberWinLossData
    {
        public MemberWinLossData()
        {
            Total = new RMS_Rpt_Parlay_MemberWinLoss();
            Records = new List<RMS_Rpt_Parlay_MemberWinLoss>();
        }

        public IEnumerable<RMS_Rpt_Parlay_MemberWinLoss> Records { get; set; }

        public RMS_Rpt_Parlay_MemberWinLoss Total { get; set; }
    }
}
