// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorCacheService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.MixParlayMonitor
{
    using System.Collections.Generic;
    using Fanex.Caching;
    using ParlayRMS.DTO.MixParlayMonitor;
    using ParlayRMS.Services.Interface.MixParlayMonitor;
    using ParlayRMS.Web.Session;

    /// <summary>
    /// Mix Parlay Monitor Cache Service.
    /// </summary>
    public class MixParlayMonitorCacheService : BaseService, IMixParlayMonitorService
    {
        private static readonly IMixParlayMonitorService MixparlayService = new MixParlayMonitorService();

        /// <summary>
        /// Gets mix parlay monitor tickets.
        /// </summary>
        /// <param name="combos">Combo values.</param>
        /// <param name="sportsTypes">Sport type values.</param>
        /// <param name="levels">Level values.</param>
        /// <param name="stakeAfterPT">Stake after PT.</param>
        /// <param name="netWinAfterPT">Net win after PT.</param>
        /// <param name="reportType">Report Type.</param>
        /// <param name="selectTop">Select Top value.</param>
        /// <param name="maxTransId">Maximum transaction ID.</param>
        /// <param name="markets">The markets.</param>
        /// <returns>
        /// List of Monitor Parlay Bets.
        /// </returns>
        public IEnumerable<RMS_Monitor_Parlay_Betlist> GetMixParlayMonitorTickets(
            string combos,
            string sportsTypes,
            string levels,
            decimal stakeAfterPT,
            decimal netWinAfterPT,
            int reportType,
            int selectTop,
            long maxTransId,
            string markets)
        {
            return MixparlayService.GetMixParlayMonitorTickets(combos, sportsTypes, levels, stakeAfterPT, netWinAfterPT, reportType, selectTop, maxTransId, markets);
        }

        ////private MixParlayMonitorTicketCache GetMixParlayMonitorTicketCache(
        ////    string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop, long maxTransId)
        ////{
        ////    var key = "RMS.MONITOR.MIXPARLAY." + string.Join("_", new string[] 
        ////    { 
        ////        combos, 
        ////        sportsTypes, 
        ////        levels, 
        ////        maxPayout.ToString(), 
        ////        selectTop.ToString(), 
        ////        maxTransId.ToString() 
        ////    });
        ////    var result = cacheService.Get<MixParlayMonitorTicketCache>(key);
        ////    if (result == null)
        ////    {
        ////        var data = mixparlayService.GetMixParlayMonitorTickets(combos, sportsTypes, levels, maxPayout, selectTop, 0);
        ////        result = new MixParlayMonitorTicketCache
        ////        {
        ////            DataCache = data,
        ////            MaxTransId = data.Max(x => x.TransId)
        ////        };

        ////        cacheService.Set<MixParlayMonitorTicketCache>(key, result, TimeSpan.FromSeconds(120));
        ////    }
        ////    else
        ////    {
        ////        var data = mixparlayService.GetMixParlayMonitorTickets(combos, sportsTypes, levels, maxPayout, selectTop, result.MaxTransId);
        ////        if (data.Count() > 0)
        ////        {
        ////            if (data.Count() < selectTop)
        ////            {
        ////                var data2 = result.DataCache.Take(selectTop - data.Count());

        ////                result = new MixParlayMonitorTicketCache
        ////                {
        ////                    DataCache = data.Union(data2).OrderBy(x => x.TransId).Take(selectTop),
        ////                    MaxTransId = data.Max(x => x.TransId)
        ////                };
        ////            }
        ////            else
        ////            {
        ////                result = new MixParlayMonitorTicketCache
        ////                {
        ////                    DataCache = data,
        ////                    MaxTransId = data.Max(x => x.TransId)
        ////                };
        ////            }

        ////            cacheService.Set<MixParlayMonitorTicketCache>(key, result, TimeSpan.FromSeconds(120));
        ////        }
        ////    }

        ////    return result;
        ////}
    }
}
