namespace ParlayRMS.Services
{
	using ParlayRMS.DataAccess;
	using ParlayRMS.DTO.LeagueBetTypeAnalysis;
	using ParlayRMS.DTO.Leagues;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ParlayRMS.ViewModel.LeagueBetTypeAnalysis;

	public class LeagueBetTypeAnalysisService : BaseService
    {
        private const byte SORT_BY_TOTAL_TICKETS = 1;
        private const byte SORT_BY_RATIO = 2;
        private const byte SORT_BY_HOUSE_WIN_LOSS = 3;
        private readonly LeagueBetTypeAnalysisDataAccess _dataAccess = new LeagueBetTypeAnalysisDataAccess();

        public List<RMS_Rpt_LeagueBettype_Analysis> GetLeagueBetTypeList(LeagueBetTypeAnalysisOptions options)
        {
            var data = _dataAccess.RMS_Rpt_LeagueBettype_Analysis(
                options.FromDate,
                options.ToDate,
                options.ParlayType,
                options.LeagueGroups,
                options.Leagues,
                options.Site,
                options.Ratio,
                options.ReportBy,
                options.SportType);
            if (options.SortById.HasValue)
            {
                data = SortData(data, options.SortById.Value, options.IsSortAsc);
            }
            return data;
        }

        public List<LeagueDetail> GetLeagueDetails(LeagueDetailOptions options)
        {
	        var data = _dataAccess.LeagueDetails(
		        options.FromDate,
		        options.ToDate,
		        options.ParlayType,
		        options.LeagueGroups,
		        options.Leagues,
		        options.Site,
		        options.HouseWinLoss,
		        options.SportType);
	        if (options.SortById.HasValue)
	        {
		        data = SortData(data, options.SortById.Value, options.IsSortAsc);
	        }
	        return data;
        }

        public List<BetTypeDetail> GetBetTypeDetails(BetTypeDetailOptions options)
        {
	        var data = _dataAccess.BetTypeDetails(
		        options.FromDate,
		        options.ToDate,
		        options.ParlayType,
		        options.LeagueGroups,
		        options.BetType,
		        options.Site,
		        options.HouseWinLoss,
		        options.SportType);
	        if (options.SortById.HasValue)
	        {
		        data = SortData(data, options.SortById.Value, options.IsSortAsc);
	        }
	        return data;
        }

		public IEnumerable<LeagueGroup> GetLeagueGroups(int sportType)
        {
            return _dataAccess.GetLeagueGroups(sportType);
        }

        public List<RMS_LeagueSel> GetListLeague(string leagueName, int groupId, int sportType, DateTime fromDate, DateTime toDate)
        {
            return _dataAccess.RMS_LeagueSel(leagueName, groupId, sportType, fromDate, toDate);
        }

        private List<T> SortData<T>(List<T> list, byte sortId, bool isSortAsc) where T : ModelBase
		{
            switch (sortId)
            {
                case SORT_BY_TOTAL_TICKETS:
                    return isSortAsc ? list.OrderBy(m => m.TotalTickets).ToList() : list.OrderByDescending(m => m.TotalTickets).ToList();
                case SORT_BY_RATIO:
                    return isSortAsc ? list.OrderBy(m => double.Parse(m.Ratio.Replace("%", ""))).ToList() : list.OrderByDescending(m => double.Parse(m.Ratio.Replace("%", ""))).ToList();
                case SORT_BY_HOUSE_WIN_LOSS:
                    return isSortAsc ? list.OrderBy(m => m.MainHouseWinloss).ToList() : list.OrderByDescending(m => m.MainHouseWinloss).ToList();
                default:
                    return list;
            }
        }
    }
}
