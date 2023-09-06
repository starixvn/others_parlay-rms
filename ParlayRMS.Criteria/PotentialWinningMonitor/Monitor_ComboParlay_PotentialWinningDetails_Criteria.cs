namespace ParlayRMS.Criteria.PotentialWinningMonitor
{
    using Fanex.Data.Repository;

    public class Monitor_ComboParlay_PotentialWinningDetails_Criteria : CriteriaBase
    {
        public int CustId { get; set; }
        public string Selection { get; set; }
        public string SportType { get; set; }
        public string ParlayType { get; set; }

        public bool IsCornerParlay { get; set; }

        public override string GetSettingKey() => "RMS_Monitor_ComboParlay_PotentialWinningDetails";

        public override bool IsValid() => true;
    }
}
