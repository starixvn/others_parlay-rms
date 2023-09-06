namespace ParlayRMS.Criteria.LeagueBetTypeAnalysis
{
    using Fanex.Data.Repository;

    public class LeagueByGroupSel_Criteria : CriteriaBase
    {
        public int SportType { get; set; }

        public override string GetSettingKey() => "RMS_LeagueGroupSel";

        public override bool IsValid() => true;
    }
}