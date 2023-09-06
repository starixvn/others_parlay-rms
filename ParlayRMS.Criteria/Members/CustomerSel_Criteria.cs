namespace ParlayRMS.Criteria.Members
{
    using Fanex.Data.Repository;

    public class CustomerSel_Criteria : CriteriaBase
    {
        public string Username { get; set; }

        public int? Level { get; set; }

        public override string GetSettingKey() => "RMS_CustomerSel";

        public override bool IsValid() => true;
    }
}
