using Fanex.BetList.Core.Entities;
using ParlayRMS.DataAccess.Implement.BetList;
using ParlayRMS.DataAccess.Interface.BetList;
using ParlayRMS.DTO.BetList;
using ParlayRMS.Services.Interface.BetList;
using System.Collections.Generic;
using System.Linq;

namespace ParlayRMS.Services.Implement.BetList
{
    public class BetListService : BaseService, IBetListService
    {
        private readonly IBetListDataAccess _betListDataAccess = new BetListDataAccess();

        public static IEnumerable<ITicket> Map(long refNo, IEnumerable<RMS_Parlay_TransDetails> data)
        {
            var result = new List<ITicket>();
            foreach (var item in data)
            {
                ITicket ticket = new Ticket
                {
                    TransId = item.TransId,
                    BetId = null,
                    BetTypeId = item.BetType,
                    HomeId = item.HomeId,
                    AwayId = item.AwayId,
                    Handicap1 = item.Hdp1,
                    Handicap2 = item.Hdp2,
                    Odds = item.Odds,
                    BetTeam = item.BetTeam,
                    EventStatus = item.BetStatus,
                    LiveHomeScore = item.LiveHomeScore,
                    LiveAwayScore = item.LiveAwayScore,
                    IsLive = item.LiveIndicator,
                    LeagueId = item.LeagueId,
                    SportTypeId = item.SportType,
                    IsNeutral = item.IsNeutral,
                    MatchId = item.MatchId,
                    MatchCode = item.MatchCode.ToString(),
                    RefNo = refNo.ToString(),
                    Status = item.Status,
                    BetCheck = item.BetCheck
                };

                result.Add(ticket);
            }

            return result.OrderBy(x => x.BetId).ToList();
        }

        public IEnumerable<ITicket> CreateChoice(long refNo)
        {
            var data = _betListDataAccess.RMS_Parlay_TransDetails(refNo);
            return Map(refNo, data);
        }
    }
}