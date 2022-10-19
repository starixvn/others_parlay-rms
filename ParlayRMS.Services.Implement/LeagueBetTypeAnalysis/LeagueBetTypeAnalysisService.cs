using ParlayRMS.DataAccess.Implement.LeagueBetTypeAnalysis;
using ParlayRMS.DTO.LeagueBetTypeAnalysis;
using ParlayRMS.DTO.Leagues;
using ParlayRMS.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParlayRMS.Services.Implement.LeagueBetTypeAnalysis
{
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

        public IEnumerable<LeagueGroup> GetLeagueGroups(int sportType)
        {
            return _dataAccess.GetLeagueGroups(sportType);
        }

        public List<RMS_LeagueSel> GetListLeague(string leagueName, int groupId, int sportType, DateTime fromDate, DateTime toDate)
        {
            return _dataAccess.RMS_LeagueSel(leagueName, groupId, sportType, fromDate, toDate);
        }

        private List<RMS_Rpt_LeagueBettype_Analysis> SortData(List<RMS_Rpt_LeagueBettype_Analysis> list, byte sortId, bool isSortAsc)
        {
            if (sortId == SORT_BY_TOTAL_TICKETS)
            {
                return isSortAsc ? list.OrderBy(m => m.TotalTickets).ToList() : list.OrderByDescending(m => m.TotalTickets).ToList();
            }
            if (sortId == SORT_BY_RATIO)
            {
                return isSortAsc ? list.OrderBy(m => double.Parse(m.Ratio.Replace("%", ""))).ToList() : list.OrderByDescending(m => double.Parse(m.Ratio.Replace("%", ""))).ToList();
            }
            if (sortId == SORT_BY_HOUSE_WIN_LOSS)
            {
                return isSortAsc ? list.OrderBy(m => m.MainHouseWinloss).ToList() : list.OrderByDescending(m => m.MainHouseWinloss).ToList();
            }
            return list;
        }
    }
}
