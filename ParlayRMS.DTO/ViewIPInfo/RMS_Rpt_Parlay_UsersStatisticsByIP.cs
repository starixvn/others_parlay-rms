namespace ParlayRMS.DTO.ViewIPInfo
{
    using System.Collections.Generic;

    public class RMS_Rpt_Parlay_UsersStatisticsByIP
    {
        public int TotalItems { get; set; }

        public long MaxTransId { get; set; }

        public IEnumerable<RMS_Rpt_Parlay_UsersStatisticsByIP_IPStats> IPStats { get; set; }
    }
}
