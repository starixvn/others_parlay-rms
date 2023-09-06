namespace ParlayRMS.Criteria.PotentialWinningMonitor
{
    using Fanex.Data.Repository;

    public class Monitor_ComboParlay_PotentialWinning_Criteria : CriteriaBase
    {
        public string Selection { get; set; }
        public string SportType { get; set; }
        public string Level { get; set; }
        public decimal WinningAmount { get; set; }

        public int LimitRowCount { get; set; }
        public string ParlayType { get; set; }

        public bool IsCornerParlay { get; set; }

        public bool IsAgentView { get; set; }
        public int? Recommend { get; set; }
        public int? CustomerClass { get; set; }
        public string Company { get; set; }

        public string Username { get; set; }


        public override string GetSettingKey() => "RMS_Monitor_ComboParlay_PotentialWinning";

        public override bool IsValid() => true;
    }
}
