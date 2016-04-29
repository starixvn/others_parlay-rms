using Fanex.Data.Repository;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public class DetailCriteria : CriteriaBase
    {
        public long CustId { get; set; }

        [SpParam(SqlParamName = "selection")]
        public string Combos { get; set; }

        [SpParam(SqlParamName = "sporttype")]
        public string SportTypes { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_MixParlay_PotentialWinningMonitor_Detail";
        }
    }
}