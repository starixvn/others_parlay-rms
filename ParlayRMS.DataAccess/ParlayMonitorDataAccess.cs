namespace ParlayRMS.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria.ParlayMonitor;
    using DTO.ParlayMonitor;
    using DTO.PotentialWinning;

    public class ParlayMonitorDataAccess
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<RMS_Monitor_Parlay_Betlist> RMS_Monitor_Parlay_Betlist(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal stakeAfterPT,
            decimal maxPayout,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets,
            bool isCornerParlay,
            decimal? odds,
            string leagueGroups,
            int? customerClass,
            string company,
            string ip,
            byte liveType,
            string username)
        {
            var criteria = new Monitor_ComboParlay_Betlist_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                Stake = stakeAfterPT,
                MaxPayout = maxPayout,
                ReportType = reportType,
                Max_TransId = maxTransId,
                LimitRowCount = selectTop,
                Market = markets,
                IsCornerParlay = isCornerParlay,
                Odds = odds,
                CustomerClass = customerClass,
                LeagueGroupId = leagueGroups,
                Company = company,
                Ip = ip,
                LiveStatus = liveType,
                Username = string.IsNullOrWhiteSpace(username) ? null : username
            };

            var betList = repository.Fetch<RMS_Monitor_Parlay_Betlist>(criteria).ToList();
            betList.ForEach(b =>
            {
                b.TransDateString = b.TransDate.ToString("MM/dd/yyyy h:mm:ss tt");
                b.CustomerClass = string.IsNullOrWhiteSpace(b.CustomerClass)
                    ? "None"
                    : b.CustomerClass;
                b.TrackingUser = string.IsNullOrWhiteSpace(b.TrackingUser)
                    ? "None"
                    : b.TrackingUser;
            });
            
            return betList;
        }

        public UserTracking RMS_Monitor_Parlay_UserTracking(int userid, string username, string transid)
        {
            var command = new ComboParlay_PotentialWinning_UserTracking_Command
            {
                Username = username,
                UserId = userid,
                TransId = transid
            };

            repository.Execute(command);

            var userTracking = new UserTracking
            {
                StatusId = command.StatusId,
                Message = command.Message
            };

            return userTracking;
        }
    }
}
