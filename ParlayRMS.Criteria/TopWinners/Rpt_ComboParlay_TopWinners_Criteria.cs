namespace ParlayRMS.Criteria.TopWinners
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_ComboParlay_TopWinners_Criteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime MoveBetsDay { get; set; }
        public DateTime BalanceUpDay { get; set; }
        public string Sporttype { get; set; }

        public string Selection { get; set; }

        public string ParlayType { get; set; }

        public int LimitedRow { get; set; }

        public string Currency { get; set; }
        public int Site { get; set; }
        public int LiveStatus { get; set; }
        public int? CustomerClass { get; set; }
        
        public override string GetSettingKey() => "RMS_Rpt_ComboParlay_TopWinners";

        public override bool IsValid() => true;
    }
}
