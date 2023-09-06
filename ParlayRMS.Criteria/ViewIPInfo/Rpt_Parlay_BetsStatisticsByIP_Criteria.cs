namespace ParlayRMS.Criteria.ViewIPInfo
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_Parlay_BetsStatisticsByIP_Criteria : CriteriaBase
    {
        public string Ip { get; set; }
        public DateTime FromDate { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_Parlay_BetsStatisticsByIP";

        public override bool IsValid() => true;
    }
}
