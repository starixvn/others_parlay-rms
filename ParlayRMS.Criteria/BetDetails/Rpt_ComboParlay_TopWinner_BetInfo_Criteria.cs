namespace ParlayRMS.Criteria.BetDetails
{
    using Fanex.Data.Repository;

    public  class Rpt_ComboParlay_TopWinner_BetInfo_Criteria : CriteriaBase
    {
        public long TransId { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_ComboParlay_TopWinner_BetInfo";

        public override bool IsValid() => true;
    }
}
