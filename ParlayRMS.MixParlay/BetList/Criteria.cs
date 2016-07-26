using Fanex.Data.Repository;

namespace ParlayRMS.MixParlay.BetList
{
    public class Criteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_MixParlay_TransDetails";
        }
    }
}