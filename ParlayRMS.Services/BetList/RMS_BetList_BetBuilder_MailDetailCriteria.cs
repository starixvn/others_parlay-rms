namespace ParlayRMS.Services.BetList
{
    using Fanex.Data.Repository;

    public class RMS_BetList_BetBuilder_MatchDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_BetList_BetBuilder_MatchDetail";
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}
