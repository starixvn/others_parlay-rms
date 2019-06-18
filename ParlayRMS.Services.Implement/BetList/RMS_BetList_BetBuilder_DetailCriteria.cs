using Fanex.Data.Repository;

namespace ParlayRMS.Services.Implement.BetList
{
    public class RMS_BetList_BetBuilder_DetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public int CustId { get; set; }

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
