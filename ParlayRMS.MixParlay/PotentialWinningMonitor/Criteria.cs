using Fanex.Data.Repository;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    public class Criteria : CriteriaBase
    {
        [SpParam(SqlParamName = "selection")]
        public string Combos { get; set; }

        [SpParam(SqlParamName = "sporttype")]
        public string SportTypes { get; set; }

        [SpParam(SqlParamName = "level")]
        public string Levels { get; set; }

        public decimal WinningAmount { get; set; }

        [SpParam(SqlParamName = "limitrowcount")]
        public int SelectTop { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_MixParlay_PotentialWinningMonitor";
        }
    }
}