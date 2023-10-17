using Fanex.Data.Repository;

namespace ParlayRMS.Criteria.BetList
{
    public class ComboParlay_TransDetails_Criteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public string CurrencyName { get; set; }

        public override string GetSettingKey() => "RMS_ComboParlay_TransDetails";

        public override bool IsValid() => true;
    }
}