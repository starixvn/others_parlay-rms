using System;
using Fanex.Data.Repository;

namespace ParlayRMS.Criteria.LeagueBetTypeAnalysis
{
    public class Rpt_LeagueBettype_Analysis_Criteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ParlayType { get; set; }
        public string LeagueGroups { get; set; }
        public string Leagues { get; set; }
        public int Site { get; set; }
        public double Ratio { get; set; }
        public int ReportBy { get; set; }
        public int SportType { get; set; }

        public override string GetSettingKey() => "RMS_Rpt_LeagueBettype_Analysis";

        public override bool IsValid() => true;
    }
}