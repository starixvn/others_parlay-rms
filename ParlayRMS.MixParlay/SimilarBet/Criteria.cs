using Fanex.Data.Repository;

namespace ParlayRMS.MixParlay.SimilarBet
{
    public class Criteria : CriteriaBase
    {
        public long TransId { get; set; }

        public override string GetSettingKey()
        {
            return "RMS_MixParlay_SimilarBet";
        }
    }
}
