using ParlayRMS.Utility;
using ParlayRMS.Utility.Data;
using System;

namespace ParlayRMS.ViewModel.LeagueBetTypeAnalysis
{
	public class BetTypeDetailOptions
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string ParlayType { get; set; }

		public string BetType { get; set; }

		public string LeagueGroups { get; set; }

		public int Site { get; set; }

		public double HouseWinLoss { get; set; }

		public int SportType { get; set; }

		public byte? SortById { get; set; }

		public bool IsSortAsc { get; set; }

		public int Page { get; set; }

		public int PageSize { get; set; }

		public string GetTitle()
		{
			var title =
				$"{ViewLang.LeagueReport} from {FromDate:MMM dd, yyyy} to {ToDate:MMM dd, yyyy} - [{ViewLang.ParlayType}] {ModelsHelper.GetSimpleParlayTitle(ParlayType)} - [{ViewLang.Group}] - {ModelsHelper.GetLeagueGroupTitle(LeagueGroups)} - [{ViewLang.Site}] {ModelsHelper.GetSiteTitle(Site)}";
			return title;
		}
	}
}
