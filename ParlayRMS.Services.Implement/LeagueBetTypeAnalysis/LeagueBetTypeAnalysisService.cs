using ParlayRMS.DataAccess.Implement.LeagueBetTypeAnalysis;
using ParlayRMS.DTO.LeagueBetTypeAnalysis;
using ParlayRMS.DTO.Leagues;
using ParlayRMS.ViewModel.Report;
using System;
using System.Collections.Generic;

namespace ParlayRMS.Services.Implement.LeagueBetTypeAnalysis
{
    public class LeagueBetTypeAnalysisService : BaseService
    {
        private readonly LeagueBetTypeAnalysisDataAccess _dataAccess = new LeagueBetTypeAnalysisDataAccess();

        public List<RMS_Rpt_LeagueBettype_Analysis> GetLeagueBetTypeList(LeagueBetTypeAnalysisOptions options)

        {

            return _dataAccess.RMS_Rpt_LeagueBettype_Analysis(
                options.FromDate,
                options.ToDate,
                options.ParlayType,
                options.LeagueGroups,
                options.Leagues,
                options.Site,
                options.Ratio,
                options.ReportBy,
                options.SportType);
        }

        public IEnumerable<LeagueGroup> GetLeagueGroups(int sportType)
        {
            return _dataAccess.GetLeagueGroups(sportType);
        }

        public List<RMS_LeagueSel> GetListLeague(string leagueName, int groupId, int sportType, DateTime fromDate, DateTime toDate)
        {
            return _dataAccess.RMS_LeagueSel(leagueName, groupId, sportType, fromDate, toDate);
        }
    }
}
