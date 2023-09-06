namespace ParlayRMS.Criteria.Common
{
    using Fanex.Data.Repository;

    public class Rpt_Parlay_SelMinTransdate_Criteria : CriteriaBase
    {
        public override string GetSettingKey() => "RMS_Rpt_Parlay_SelMinTransdate";

        public override bool IsValid() => true;
    }
}