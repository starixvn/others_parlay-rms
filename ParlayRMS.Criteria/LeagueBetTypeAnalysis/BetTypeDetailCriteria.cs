using Fanex.Data.Repository;
using System;

namespace ParlayRMS.Criteria.LeagueBetTypeAnalysis
{
	public class BetTypeDetailCriteria: CriteriaBase
	{
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string ParlayType { get; set; }
		public string LeagueGroups { get; set; }
		public string BetType { get; set; }
		public int Site { get; set; }
		public double HouseWinLoss { get; set; }
		public int SportType { get; set; }

		public override string GetSettingKey() => "RMS_Rpt_LeagueBettype_BetType_Detail";

		public override bool IsValid() => true;
	}
}
