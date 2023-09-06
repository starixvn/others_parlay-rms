namespace ParlayRMS.Criteria.TotalOutstanding
{
    using Fanex.Data.Repository;

    public class Rpt_ComboParlay_Outstanding_Mem_Criteria : CriteriaBase
    {
        public int CustId { get; set; }
        
        public string Selection { get; set; }
        public string SportType { get; set; }
        public string ParlayType { get; set; }

        public bool IsCornerParlay { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_ComboParlay_Outstanding_Mem";

        public override bool IsValid() => true;
    }
}
