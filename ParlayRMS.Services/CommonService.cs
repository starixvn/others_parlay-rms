namespace ParlayRMS.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using ParlayRMS.DataAccess;
    using ParlayRMS.DTO.Common;
    using ParlayRMS.Utility;

    public class CommonService : BaseService
    {
        private static readonly ICache CacheInstance = new MemoryCache(
                new MemoryCacheOptions
                {
                     ExpirationScanFrequency = TimeSpan.FromSeconds(30),
                     CompactionPercentage = 0.15,
                     CompactOnMemoryPressure = true
                }
            );
        private readonly CommonDataAccess _commonDataAccess = new CommonDataAccess();

        public RMS_SelBalanceUpDay GetBalanceUpDayInfo()
        {
            if (!AppSetting.IsCache)
            {
                return _commonDataAccess.RMS_SelBalanceUpDay();
            }

            const string key = "BalanceUpDayInfo";

            var cacheIsAvailable = CacheInstance.TryGet(key, out RMS_SelBalanceUpDay balanceUpDay);
            if (!cacheIsAvailable)
            {
                balanceUpDay = _commonDataAccess.RMS_SelBalanceUpDay();
                CacheInstance.Set(key, balanceUpDay, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(1) });
            }

            return balanceUpDay;
        }

        public RMS_Rpt_Parlay_SelMinTransdate GetMinTransDate()
        {
            if (!AppSetting.IsCache)
            {
                return _commonDataAccess.RMS_Rpt_Parlay_SelMinTransdate();
            }

            const string key = "MinTransDate";
            var cacheIsAvailable = CacheInstance.TryGet(key, out RMS_Rpt_Parlay_SelMinTransdate minTransdate);
            if (!cacheIsAvailable)
            {
                minTransdate = _commonDataAccess.RMS_Rpt_Parlay_SelMinTransdate();
                CacheInstance.Set(key, minTransdate, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(1) });
            }

            return minTransdate;
        }

        public List<int> GetBettypeBelongsBetRadar()
        {
            if (!AppSetting.IsCache)
            {
                return _commonDataAccess.GetBettypeBelongsBetRadar();
            }

            const string key = "BettypeBelongsBetRadar";
            var cacheIsAvailable = CacheInstance.TryGet(key, out List<int> betTypes);
            if (!cacheIsAvailable)
            {
                betTypes = _commonDataAccess.GetBettypeBelongsBetRadar();
                CacheInstance.Set(key, betTypes, new CacheItemOptions { RelativeExpiration = TimeSpan.FromDays(1) });
            }

            return betTypes;
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return _commonDataAccess.GetCurrencies();
        }

        public IEnumerable<Company> GetCompanies()
        {
            return _commonDataAccess.RMS_SiteByGroupSel();
        }
    }
}
