namespace ParlayRMS.Criteria.Winloss
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_ComboParlay_Winloss_Criteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime MoveBetsDay { get; set; }
        public DateTime BalanceUpDay { get; set; }

        public string Sporttype { get; set; }

        public string Selection { get; set; }

        public string ParlayType { get; set; }

        public int ReportType { get; set; }
        
        public int Site { get; set; }
        public int LiveStatus { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_ComboParlay_Winloss";

        public override bool IsValid() => true;
    }
}
