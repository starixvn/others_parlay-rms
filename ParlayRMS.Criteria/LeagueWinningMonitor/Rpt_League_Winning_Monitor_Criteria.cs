namespace ParlayRMS.Criteria.LeagueWinningMonitor
{
    using Fanex.Data.Repository;

    public class Rpt_League_Winning_Monitor_Criteria : CriteriaBase
    {
        public string Selection { get; set; }
        public string ParlayType { get; set; }
        public string SportType { get; set; }
        public string Level { get; set; }
        public int LimitRowCount { get; set; }
        public int LeagueGroupId { get; set; }
        public decimal MaxPayout { get; set; }
        public int ReportType { get; set; }
        public int LeagueId { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_League_Winning_Monitor";

        public override bool IsValid() => true;
    }
}