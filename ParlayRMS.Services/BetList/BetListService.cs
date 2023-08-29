namespace ParlayRMS.Services.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.BetList;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using ParlayRMS.DTO.BetList;

    public class BetListService : BaseService
    {
        private readonly BetListDataAccess _betListDataAccess = new BetListDataAccess();

        public Tuple<List<bool>, List<ITicket>> Map(long refNo, IEnumerable<RMS_Parlay_TransDetails> data)
        {
            var result = new List<ITicket>();
            var blackListLeagues = new List<bool>();
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
                    Stake = item.Stake,
                    TransDate = item.TransDate,
                    TransDesc = item.TransDesc,
                };

                result.Add(ticket);

                blackListLeagues.Add(item.IsBlacklistLeague);
            }

            return new Tuple<List<bool>, List<ITicket>>(blackListLeagues, result.OrderBy(x => x.BetId).ToList());
        }

        public BetListData GetBetListData(long transId, long refNo)
        {
            return _betListDataAccess.RMS_Parlay_TransDetails(transId, refNo);
        }

        public Tuple<List<bool>, List<ITicket>> CreateChoice(long transId, long refNo)
        {
            var data = GetBetListData(transId, refNo);
            return Map(refNo, data.Tickets);
        }

        public RMS_Monitor_Parlay_BetInfo GetBetInfo(long transId)
        {
            return _betListDataAccess.RMS_Monitor_Parlay_BetInfo(transId);
        }


        public List<RMS_BetList_BetBuilder_DetailModel> RMS_BetList_BetBuilder_Detail(RMS_BetList_BetBuilder_DetailCriteria criteria)
        {
            var repository = new DynamicRepository();

            return repository.Fetch<RMS_BetList_BetBuilder_DetailModel>(criteria).ToList();
        }

        public List<RMS_BetList_BetBuilder_MatchDetailModel> RMS_BetList_BetBuilder_MatchDetail(long transId)
        {
            var criteria = new RMS_BetList_BetBuilder_MatchDetailCriteria
            {
                TransId = transId
            };
            var repository = new DynamicRepository();

            return repository.Fetch<RMS_BetList_BetBuilder_MatchDetailModel>(criteria).ToList();
        }

        public List<RMS_BetList_PenaltyShootout_MatchDetailModel> RMS_BetList_PenaltyShootout_MatchDetail(long transId)
        {
            var criteria = new RMS_BetList_PenaltyShootout_MatchDetailCriteria
            {
                TransId = transId
            };
            var repository = new DynamicRepository();

            return repository.Fetch<RMS_BetList_PenaltyShootout_MatchDetailModel>(criteria).ToList();
        }
    }
}
