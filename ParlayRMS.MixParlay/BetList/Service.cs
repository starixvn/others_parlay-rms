using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ParlayRMS.MixParlay.BetList
{
    public class Service : IService
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public static IEnumerable<ITicket> Map(long refNo, IEnumerable<TransDetails> data)
        {
            List<ITicket> result = new List<ITicket>();
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
                    BetCheck = item.BetCheck,
                    TransDate = item.TransDate,
                    TransDesc = item.TransDesc
                };

                result.Add(ticket);
            }

            return result.OrderBy(x => x.BetId).ToList();
        }

        public IEnumerable<ITicket> CreateChoice(long refNo)
        {
            var criteria = new Criteria
            {
                RefNo = refNo
            };

            var transDetails = repository.Fetch<TransDetails>(criteria);
            return Map(refNo, transDetails);
        }
    }
}
