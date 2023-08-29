namespace ParlayRMS.Services.Common
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Common;
    using Fanex.Caching;
    using ParlayRMS.DTO.Common;
    using Utility;

    public class CommonService : BaseService
    {
        private static readonly CacheService CacheServiceInstance = new CacheServiceImpl();
        private readonly CommonDataAccess _commonDataAccess = new CommonDataAccess();

        public RMS_SelBalanceUpDay GetBalanceUpDayInfo()
        {
            if (!AppSetting.IsCache)
            {
                return _commonDataAccess.RMS_SelBalanceUpDay();
            }

            var key = "BalanceUpDayInfo";
            var result = CacheServiceInstance.Get<RMS_SelBalanceUpDay>(key);
            if (result == null)
            {
                result = _commonDataAccess.RMS_SelBalanceUpDay();
                CacheServiceInstance.Set(key, result, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(1) });
            }

            return result;
        }

        public RMS_Rpt_Parlay_SelMinTransdate GetMinTransDate()
        {
            if (!AppSetting.IsCache)
            {
                return _commonDataAccess.RMS_Rpt_Parlay_SelMinTransdate();
            }

            var key = "MinTransDate";
            var result = CacheServiceInstance.Get<RMS_Rpt_Parlay_SelMinTransdate>(key);
            if (result == null)
            {
                result = _commonDataAccess.RMS_Rpt_Parlay_SelMinTransdate();
                CacheServiceInstance.Set(key, result, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(1) });
            }

            return result;
        }

        public List<int> GetBettypeBelongsBetRadar()
        {
            if (!AppSetting.IsCache)
            {
                return _commonDataAccess.GetBettypeBelongsBetRadar();
            }

            var key = "BettypeBelongsBetRadar";
            var result = CacheServiceInstance.Get<List<int>>(key);
            if (result == null)
            {
                result = _commonDataAccess.GetBettypeBelongsBetRadar();
                CacheServiceInstance.Set(key, result, new CacheItemOptions { RelativeExpiration = TimeSpan.FromDays(1) });
            }

            return result;
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
