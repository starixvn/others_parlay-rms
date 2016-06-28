using ParlayRMS.DataAccess.Implement.ParlayMonitor;
using ParlayRMS.DataAccess.Interface.ParlayMonitor;
using ParlayRMS.DTO.ParlayMonitor;
using ParlayRMS.Services.Interface.ParlayMonitor;
using ParlayRMS.Utility;
using SignalR;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace ParlayRMS.Services.Implement.ParlayMonitor
{
    public class ParlayMonitorService : BaseService, IParlayMonitorService
    {
        private static IHubContext _hub = GlobalHost.ConnectionManager.GetHubContext<ParlayMonitorHub>();

        private readonly IParlayMonitorDataAccess _parlayDataAccess = new ParlayMonitorDataAccess();

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

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        public IEnumerable<RMS_Monitor_Parlay_Betlist> GetParlayMonitorTickets(
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
            var data = _parlayDataAccess.RMS_Monitor_Parlay_Betlist(combos, parlayTypes, sportTypes, levels, stakeAfterPT, netWinAfterPT, reportType, selectTop, maxTransId, markets);
            foreach (var item in data)
            {
                item.Stake = Math.Round(item.Stake, 2);
                item.NetWinAfterPT = Math.Floor(item.NetWinAfterPT);
                item.SportTypeName = CacheRef.GetSportNameDropDown(item.SportType);
            }

            return data;
        }
    }
}