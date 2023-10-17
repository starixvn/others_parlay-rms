namespace ParlayRMS.Criteria.MemberWinLoss
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_ComboParlay_Winloss_Ag_Criteria : CriteriaBase
    {
        public string Recommends { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime MoveBetsDay { get; set; }
        public DateTime BalanceUpDay { get; set; }
        public string Selection { get; set; }
        public string Parlaytype { get; set; }
        public string SportType { get; set; }
        public int LiveStatus { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_ComboParlay_Winloss_Ag";

        public override bool IsValid() => true;
    }
}
