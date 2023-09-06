namespace ParlayRMS.Services.Leagues
{
    using System.Collections.Generic;
    using Criteria;
    using DataAccess;
    using ParlayRMS.DTO.Leagues;

    public class LeaguesService : BaseService
    {
        private readonly LeaguesDataAccess _leagueDataAccess = new LeaguesDataAccess();

        public List<RMS_LeagueSel> GetListLeague(string leagueName, int groupid, int sportType)
        {
            return _leagueDataAccess.RMS_LeagueSel(leagueName, groupid, sportType);
        }

        public Result AddLeague(string leagueName, int groupId, int adminId, string ip)
        {
            return _leagueDataAccess.RMS_LeagueIns(leagueName, groupId, adminId, ip);
        }

        public Result UpdateLeague(int leagueId, int level, int adminId, string ip)
        {
            return _leagueDataAccess.RMS_LeagueUpd(leagueId, level, adminId, ip);
        }
    }
}
