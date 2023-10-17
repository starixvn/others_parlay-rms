namespace ParlayRMS.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.PotentialWinningMonitor;
    using Fanex.Data.Repository;
    using Criteria.ParlayMonitor;
    using DTO.PotentialWinning;

    public class PotentialWinningMonitorDataAccess
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<RMS_Monitor_Potential_Winning> RMS_Monitor_Parlay_PotentialWinning(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            bool isCornerParlay,
            byte filterBy,
            int? agentId,
            int? customerClass,
            string company,
            string username)
        {
            var criteria = new Monitor_ComboParlay_PotentialWinning_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                WinningAmount = winningAmount,
                LimitRowCount = selectTop,
                IsCornerParlay = isCornerParlay,
                IsAgentView = filterBy > 0,
                Recommend = agentId,
                CustomerClass = customerClass,
                Company = company,
                Username = string.IsNullOrWhiteSpace(username) ? null : username
            };

            return filterBy > 0
                ? GetDataForAgent(criteria)
                : GetDataForMember(criteria);
        }

        private IEnumerable<RMS_Monitor_Potential_Winning> GetDataForMember(Monitor_ComboParlay_PotentialWinning_Criteria criteria)
        {
            var dataForMember = repository.Fetch<RMS_Monitor_Potential_Winning>(criteria).ToList();
            dataForMember.ForEach(d =>
            {
                d.TransDateString = d.TransDate.ToString("MM/dd/yyyy h:mm:ss tt");
                d.CustomerClass = string.IsNullOrWhiteSpace(d.CustomerClass)
                    ? "None"
                    : d.CustomerClass;

            });

            return dataForMember;
        }

        private IEnumerable<RMS_Monitor_Potential_Winning> GetDataForAgent(Monitor_ComboParlay_PotentialWinning_Criteria criteria)
        {
            var dataForAgent = repository.Fetch<RMS_Monitor_Potential_Winning>(criteria).ToList();
            return dataForAgent;
        }

        public IEnumerable<RMS_Monitor_Potential_Winning_Detail> RMS_Monitor_Parlay_PotentialWinningDetail(
            string combos,
            string parlayTypes,
            string sportsTypes,
            int custId,
            bool isCornerParlay)
        {
            var criteria = new Monitor_ComboParlay_PotentialWinningDetails_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                CustId = custId,
                IsCornerParlay = isCornerParlay
            };

            var details = repository.Fetch<RMS_Monitor_Potential_Winning_Detail>(criteria).ToList();
            details.ForEach(d =>
            {
                d.TransDateString = d.TransDate.ToString("MM/dd/yyyy h:mm:ss tt");
                d.TrackingUser = string.IsNullOrWhiteSpace(d.TrackingUser)
                    ? "None"
                    : d.TrackingUser;
            });
            
            return details;
        }

        public UserTracking RMS_ComboParlay_PotentialWinning_UserTracking(int userid, string username, string transid)
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
