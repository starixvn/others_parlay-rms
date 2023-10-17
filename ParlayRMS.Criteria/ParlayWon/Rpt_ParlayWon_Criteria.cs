namespace ParlayRMS.Criteria.ParlayWon
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_ParlayWon_Criteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime MoveBetsDay { get; set; }
        public string ParlayType { get; set; }
        public string Selection { get; set; }
        public string Level { get; set; }
        public string Sporttype { get; set; }
        public decimal FromHouseAmt { get; set; }
        public decimal ToHouseAmt { get; set; }
        public int SortById { get; set; }
        public bool IsSortAsc { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public string Currency { get; set; }
        public int Site { get; set; }
        public int LiveStatus { get; set; }
        public int? CustomerClass { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_ParlayWon";

        public override bool IsValid() => true;
    }
}
