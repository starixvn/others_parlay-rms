// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.MixParlayMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using ParlayRMS.DataAccess.Implement.MixParlayMonitor;
    using ParlayRMS.DataAccess.Interface.MixParlayMonitor;
    using ParlayRMS.DTO.MixParlayMonitor;
    using ParlayRMS.Services.Interface.MixParlayMonitor;
    using SignalR;
    using SignalR.Hubs;

    /// <summary>
    /// Mix Parlay Monitor Service.
    /// </summary>
    public class MixParlayMonitorService : BaseService, IMixParlayMonitorService
    {
        private static IHubContext _hub = GlobalHost.ConnectionManager.GetHubContext<MixParlayMonitorHub>();

        private readonly IMixParlayMonitorDataAccess _mixparlayDataAccess = new MixParlayMonitorDataAccess();

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    _hub.Clients.Broadcast();

                    Thread.Sleep(2000);
                }
            });
        }

        /// <summary>
        /// Gets the mix parlay monitor tickets.
        /// </summary>
        /// <param name="combos">The combo values.</param>
        /// <param name="sportTypes">Type of the sport.</param>
        /// <param name="levels">The level.</param>
        /// <param name="stakeAfterPT">Stake after PT.</param>
        /// <param name="netWinAfterPT">Net win after PT.</param>
        /// <param name="reportType">Report Type.</param>
        /// <param name="selectTop">Number of top items.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <param name="markets">Markets Info.</param>
        /// <returns>
        /// List of RMS_Monitor_Parlay_BetList objects.
        /// </returns>
        public IEnumerable<RMS_Monitor_ComboParlay_Betlist> GetMixParlayMonitorTickets(
            string combos,
            string parlayTypes,
            string sportTypes,
            string levels,
            decimal stakeAfterPT,
            decimal netWinAfterPT,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets)
        {
            var data = _mixparlayDataAccess.RMS_Monitor_ComboParlay_Betlist(combos, parlayTypes, sportTypes, levels, stakeAfterPT, netWinAfterPT, reportType, selectTop, maxTransId, markets);

            ////if (data.Count() == 0)
            ////{
            ////    data = CreateTestData(combos, levels, stakeAfterPT, maxTransId);
            ////}

            foreach (var item in data)
            {
                item.Odds = Math.Round(item.Odds, 2);
                item.Stake = Math.Round(item.Stake, 2);
                item.NetWinAfterPT = Math.Round(item.NetWinAfterPT, 2);
            }

            return data;
        }

        /// <summary>
        /// Creates the test data.
        /// </summary>
        /// <param name="combos">The combos.</param>
        /// <param name="levels">The levels.</param>
        /// <param name="stakeAfterPT">Stake after PT.</param>
        /// <param name="maxTransId">The maximum trans identifier.</param>
        /// <returns>List of RMS_Monitor_Parlay_BetList objects.</returns>
        private IEnumerable<RMS_Monitor_ComboParlay_Betlist> CreateTestData(string combos, string levels, decimal stakeAfterPT, long maxTransId)
        {
            Random rnd = new Random();
            var count = rnd.Next(0, 5);
            var list = new List<RMS_Monitor_ComboParlay_Betlist>();
            for (int i = count; i > 0; i--)
            {
                var combo = rnd.Next(2, 6);
                var level = rnd.Next(0, 10);
                var stake = Convert.ToDecimal(rnd.NextDouble() * 100);
                var odds = Convert.ToDecimal(rnd.NextDouble() * 100);
                var refno = rnd.Next(1000000, 9999999);
                if (combos.Contains(combo.ToString()) &&
                    levels.Contains(level.ToString()) &&
                    stake * odds > stakeAfterPT)
                {
                    list.Add(new RMS_Monitor_ComboParlay_Betlist
                    {
                        Combo = combo,
                        Level = level,
                        Username = "test",
                        Stake = stake,
                        Odds = odds,
                        NetWinAfterPT = (stake - 1) * odds,
                        RefNo = refno,
                        TransDate = DateTime.Now,
                        TransId = maxTransId + i
                    });
                }
            }

            return list;
        }
    }
}