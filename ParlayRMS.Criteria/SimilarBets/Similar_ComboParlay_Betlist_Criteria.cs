namespace ParlayRMS.Criteria.SimilarBets
{
    using Fanex.Data.Repository;

    public class Similar_ComboParlay_Betlist_Criteria : CriteriaBase
    {
        public long TransId{ get; set; }

        public bool IsSimilar { get; set; }
        
        public override string GetSettingKey() => "RMS_Similar_ComboParlay_Betlist";

        public override bool IsValid() => true;
    }
}
