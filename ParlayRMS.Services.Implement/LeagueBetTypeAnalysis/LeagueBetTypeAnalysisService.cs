using ParlayRMS.DataAccess.Implement.LeagueBetTypeAnalysis;
using ParlayRMS.DTO.LeagueBetTypeAnalysis;
using ParlayRMS.ViewModel.Report;
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
                options.ReportBy);
        }
    }
}
