// --------------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.BetList
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using ParlayRMS.DataAccess.Implement.BetList;
    using ParlayRMS.DataAccess.Interface.BetList;
    using ParlayRMS.DTO.BetList;
    using ParlayRMS.Services.Interface.BetList;

    /// <summary>
    /// Bet List Service.
    /// </summary>
    public class BetListService : BaseService, IBetListService
    {
        private readonly IBetListDataAccess _betListDataAccess = new BetListDataAccess();

        /// <summary>
        /// Maps Transaction Detail to Ticket Information.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <param name="data">Transaction data.</param>
        /// <returns>List of Ticket objects.</returns>
        public static IEnumerable<ITicket> Map(long refNo, IEnumerable<RMS_Parlay_TransDetails> data)
        {
            List<ITicket> result = new List<ITicket>();
            foreach (var item in data)
            {
                ITicket ticket = new Ticket
                {
                    BetId = item.TransId,
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
                    Status = item.Status
                };

                result.Add(ticket);
            }

            return result.OrderBy(x => x.BetId).ToList();
        }

        /// <summary>
        /// Creates choice.
        /// </summary>
        /// <param name="refNo">Reference number.</param>
        /// <returns>List of tickets.</returns>
        public IEnumerable<ITicket> CreateChoice(long refNo)
        {
            var data = this._betListDataAccess.RMS_Parlay_TransDetails(refNo);
            return Map(refNo, data);
        }
    }
}