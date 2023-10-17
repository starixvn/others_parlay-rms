using Fanex.Data.Repository;

namespace ParlayRMS.Criteria.BetList
{
    public class Monitor_ComboParlay_BetInfo_Criteria : CriteriaBase
    {
        public long TransId { get; set; }

        public override string GetSettingKey() => "RMS_Monitor_ComboParlay_BetInfo";

        public override bool IsValid() => true;
    }
}