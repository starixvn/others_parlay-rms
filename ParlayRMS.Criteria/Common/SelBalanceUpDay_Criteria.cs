namespace ParlayRMS.Criteria.Common
{
    using Fanex.Data.Repository;

    public class SelBalanceUpDay_Criteria : CriteriaBase
    {
        public override string GetSettingKey() => "RMS_SelBalanceUpDay";

        public override bool IsValid() => true;
    }
}