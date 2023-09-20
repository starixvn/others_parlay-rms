namespace ParlayRMS.Criteria.BetList
{
    using Fanex.Data.Repository;

    public class BetList_PenaltyShootout_MatchDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_BetList_PenaltyShootout_MatchDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
