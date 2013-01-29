// -----------------------------------------------------------------------
// <copyright file="MixParlayCacheService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.MixParlayMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ParlayRMS.Services.Interface.MixParlayMonitor;
    using ParlayRMS.Models;
    using Fanex.Caching;
    using ParlayRMS.Web.Session;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.MixParlayMonitor;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MixParlayMonitorCacheService : BaseService, IMixParlayMonitorService
    {
        private static readonly ICache cacheService = CacheService.CreateInstance(CacheType.MemoryCache);
        private static readonly ICurrentSession currentSession = CurrentSession.Instance;
        private static readonly IMixParlayMonitorService mixparlayService = new MixParlayMonitorService();

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
            return mixparlayService.GetMixParlayMonitorTickets(combos, sportsTypes, levels, maxPayout, selectTop, maxTransId);
        }

        //private MixParlayMonitorTicketCache GetMixParlayMonitorTicketCache(string combos, string sportsTypes, string levels, decimal maxPayout, int selectTop, long maxTransId)
        //{
        //    var key = "RMS.MONITOR.MIXPARLAY." + string.Join("_", new string[] { combos, sportsTypes, levels, maxPayout.ToString(), selectTop.ToString(), maxTransId.ToString() });
        //    var result = cacheService.Get<MixParlayMonitorTicketCache>(key);
        //    if (result == null)
        //    {
        //        var data = mixparlayService.GetMixParlayMonitorTickets(combos, sportsTypes, levels, maxPayout, selectTop, 0);
        //        result = new MixParlayMonitorTicketCache
        //        {
        //            DataCache = data,
        //            MaxTransId = data.Max(x => x.TransId)
        //        };

        //        cacheService.Set<MixParlayMonitorTicketCache>(key, result, TimeSpan.FromSeconds(120));
        //    }
        //    else
        //    {
        //        var data = mixparlayService.GetMixParlayMonitorTickets(combos, sportsTypes, levels, maxPayout, selectTop, result.MaxTransId);
        //        if (data.Count() > 0)
        //        {
        //            if (data.Count() < selectTop)
        //            {
        //                var data2 = result.DataCache.Take(selectTop - data.Count());

        //                result = new MixParlayMonitorTicketCache
        //                {
        //                    DataCache = data.Union(data2).OrderBy(x => x.TransId).Take(selectTop),
        //                    MaxTransId = data.Max(x => x.TransId)
        //                };
        //            }
        //            else
        //            {
        //                result = new MixParlayMonitorTicketCache
        //                {
        //                    DataCache = data,
        //                    MaxTransId = data.Max(x => x.TransId)
        //                };
        //            }

        //            cacheService.Set<MixParlayMonitorTicketCache>(key, result, TimeSpan.FromSeconds(120));
        //        }
        //    }

        //    return result;
        //}
    }
}
