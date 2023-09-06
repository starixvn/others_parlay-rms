namespace ParlayRMS.Criteria.ParlayMonitor
{
    using Fanex.Data.Repository;

    public class Monitor_ComboParlay_Betlist_Criteria : CriteriaBase
    {
        public string Selection { get; set; }
        public string ParlayType { get; set; }
        public string SportType { get; set; }
        public string Level { get; set; }
        public decimal Stake { get; set; }
        public decimal MaxPayout { get; set; }
        public int ReportType { get; set; }
        public long Max_TransId { get; set; }
        public int LimitRowCount { get; set; }
        public string Market { get; set; }
        public bool IsCornerParlay { get; set; }
        public decimal? Odds { get; set; }
        public int? CustomerClass { get; set; }
        public string LeagueGroupId { get; set; }
        public string Company { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey() => "RMS_Monitor_ComboParlay_Betlist";

        public override bool IsValid() => true;
    }
}
