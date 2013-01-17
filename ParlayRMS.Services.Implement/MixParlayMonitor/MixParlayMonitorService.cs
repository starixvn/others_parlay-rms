// -----------------------------------------------------------------------
// <copyright file="MixParlayService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.MixParlayMonitor
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.MixParlayMonitor;
    using ParlayRMS.DataAccess.Interface.MixParlayMonitor;
    using ParlayRMS.DTO;
    using ParlayRMS.Services.Interface.MixParlayMonitor;
    using SignalR.Hubs;
    using System.Threading;
    using SignalR;
    using ParlayRMS.DTO.MixParlayMonitor;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MixParlayMonitorService : BaseService, IMixParlayMonitorService
    {
        private readonly IMixParlayMonitorDataAccess mixparlayDataAccess = new MixParlayMonitorDataAccess();

        private static IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<MixParlayMonitorHub>();

        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    hub.Clients.Broadcast();

                    Thread.Sleep(2000);
                }
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="combos"></param>
        /// <param name="sportsTypes"></param>
        /// <param name="levels"></param>
        /// <param name="maxPayout"></param>
        /// <param name="maxTransId"></param>
        /// <param name="selectTop"></param>
        /// <returns></returns>
        public IEnumerable<RMS_Monitor_Parlay_Betlist> GetMixParlayMonitorTickets(string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop, long maxTransId)
        {
            var data = mixparlayDataAccess.RMS_Monitor_Parlay_Betlist(combos, sportsTypes, levels, maxPayout, selectTop, maxTransId);

#if DEBUG

            // generate temporary tickets for testing, note these tickets can't be matched by conditions
            if (data.Count() == 0)
            {
                Random rnd = new Random();
                var count = rnd.Next(0, 5);
                var list = new List<RMS_Monitor_Parlay_Betlist>();
                for (int i = count; i > 0; i--)
                {
                    var combo = rnd.Next(2, 6);
                    var level = rnd.Next(0, 10);
                    var stake = Convert.ToDecimal(rnd.NextDouble() * 100);
                    var odds = Convert.ToDecimal(rnd.NextDouble() * 100);
                    var refno = rnd.Next(1000000, 9999999);
                    if (combos.Contains(combo.ToString()) &&
                        levels.Contains(level.ToString()) &&
                        stake * odds > maxPayout)
                    {
                        list.Add(new RMS_Monitor_Parlay_Betlist
                        {
                            Combo = combo,
                            Level = level,
                            Username = "test",
                            Stake = stake,
                            Odds = odds,
                            MaxPayout = stake * odds,
                            RefNo = refno,
                            TransDate = DateTime.Now,
                            TransId = maxTransId + i
                        });
                    }
                }

                data = list;
            }
#endif

            foreach (var item in data)
            {
                item.Odds = Math.Round(item.Odds, 2);
                item.Stake = Math.Round(item.Stake, 2);
                item.MaxPayout = Math.Round(item.Odds * item.Stake, 2);
            }

            return data;
        }
    }
}