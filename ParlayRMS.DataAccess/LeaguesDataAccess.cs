namespace ParlayRMS.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria;
    using Criteria.Leagues;
    using DTO.Leagues;

    public class LeaguesDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public List<RMS_LeagueSel> RMS_LeagueSel(string leagueName, int groupId, int sportType)
        {
            var criteria = new LeagueSel_Criteria
            {
                LeagueName = leagueName,
                SportType = sportType,
                GroupId = groupId
            };
            var counter = 1;
            var leagues = repository.Fetch<RMS_LeagueSel>(criteria).ToList();
            leagues.ForEach(r => { r.No = counter++; });

            return leagues;
        }
        
        public Result RMS_LeagueIns(string leagueName, int groupId, int adminId, string ip)
        {
            var command = new LeagueIns_Command
            {
                LeagueName = leagueName,
                AdminId = adminId,
                GroupId = groupId,
                Ip = ip
            };

            repository.Execute(command);

            return ResultFactory.From(command);
        }

        public Result RMS_LeagueUpd(int leagueId, int groupId, int adminId, string ip)
        {
            var command = new LeagueUpd_Command
            {
                LeagueId = leagueId,
                AdminId = adminId,
                GroupId = groupId,
                Ip = ip
            };

            repository.Execute(command);

            return ResultFactory.From(command);
        }
    }
}
