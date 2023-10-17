namespace ParlayRMS.Criteria.ViewIPInfo
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_Parlay_UsersStatisticsByIP_Criteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public long Max_TransId { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_Parlay_UsersStatisticsByIP";

        public override bool IsValid() => true;
    }
}
