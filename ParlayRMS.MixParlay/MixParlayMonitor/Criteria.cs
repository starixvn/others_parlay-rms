using Fanex.Data.Repository;

namespace ParlayRMS.MixParlay.MixParlayMonitor
{
    public class Criteria : CriteriaBase
    {
        [SpParam(SqlParamName = "selection")]
        public string Combos { get; set; }

        [SpParam(SqlParamName = "sporttype")]
        public string SportTypes { get; set; }

        [SpParam(SqlParamName = "level")]
        public string Levels { get; set; }

        [SpParam(SqlParamName = "market")]
        public string Markets { get; set; }

        public decimal Stake { get; set; }

        public int ReportType { get; set; }

        [SpParam(SqlParamName = "max_transid")]
        public decimal MaxTransId { get; set; }

        [SpParam(SqlParamName = "limitrowcount")]
        public int SelectTop { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_MixParlay_Monitor";
        }
    }
}