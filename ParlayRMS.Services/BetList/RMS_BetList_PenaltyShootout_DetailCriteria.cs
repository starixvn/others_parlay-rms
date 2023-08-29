namespace ParlayRMS.Services.BetList
{
    using Fanex.Data.Repository;

    public class RMS_BetList_PenaltyShootout_DetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_BetList_BetBuilder_Detail";
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}
