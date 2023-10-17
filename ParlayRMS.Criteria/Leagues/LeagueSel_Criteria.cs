namespace ParlayRMS.Criteria.Leagues
{
    using Fanex.Data.Repository;

    public class LeagueSel_Criteria : CriteriaBase
    {
        public string LeagueName { get; set; }

        public int SportType { get; set; }
        public int GroupId { get; set; }

        public override string GetSettingKey() => "RMS_LeagueSel";

        public override bool IsValid() => true;
    }
}