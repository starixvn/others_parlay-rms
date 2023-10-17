namespace ParlayRMS.Criteria.Agents
{
    using Fanex.Data.Repository;

    public class DangerousAgentSel_Criteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey() => "RMS_DangerousAgentSel";

        public override bool IsValid() => true;
    }
}
