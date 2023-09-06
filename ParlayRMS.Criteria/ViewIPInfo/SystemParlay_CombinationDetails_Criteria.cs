namespace ParlayRMS.Criteria.ViewIPInfo
{
    using Fanex.Data.Repository;

    public class SystemParlay_CombinationDetails_Criteria : CriteriaBase
    {
        public long RefNo{ get; set; }

        public decimal Outstanding{ get; set; }

        public override string GetSettingKey() => "RMS_SystemParlay_CombinationDetails";

        public override bool IsValid() => true;
    }
}
