namespace ParlayRMS.Criteria.TopWinners
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_ComboParlay_TopWinnersDetails_Criteria : CriteriaBase
    {
        public int CustId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime MoveBetsDay { get; set; }
        
        public string Sporttype { get; set; }

        public string Selection { get; set; }

        public string ParlayType { get; set; }

        
        public string Currency { get; set; }
        public int LiveStatus { get; set; }
        
        public override string GetSettingKey() => "RMS_Rpt_ComboParlay_TopWinners_Details";

        public override bool IsValid() => true;
    }
}
