using Fanex.Data.Repository;

namespace ParlayRMS.MixParlay.TotalOutstanding
{
    public class AgentCriteria : CriteriaBase
    {
        public long CustId { get; set; }

        [SpParam(SqlParamName = "selection")]
        public string Combos { get; set; }

        [SpParam(SqlParamName = "sporttype")]
        public string SportTypes { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_MixParlay_Outstanding_Ag";
        }
    }
}