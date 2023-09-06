namespace ParlayRMS.Criteria.Members
{
    using Fanex.Data.Repository;

    public class CustomerAgentOfMasterSel_Criteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey() => "RMS_CustomerAgentOfMasterSel";

        public override bool IsValid() => true;
    }
}
