namespace ParlayRMS.Criteria.ViewIPInfo
{
    using System;
    using Fanex.Data.Repository;

    public class Rpt_ComboParlay_BetsStatisticsByIP_Sporttype_Criteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public string Ip { get; set; }

        public int CustId { get; set; }

        public int SportType { get; set; }

        public string TicketStatus { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_ComboParlay_BetsStatisticsByIP_Sporttype";

        public override bool IsValid() => true;
    }
}
