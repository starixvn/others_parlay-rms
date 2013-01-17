// -----------------------------------------------------------------------
// <copyright file="CommonService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Implement.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.DTO;
    using ParlayRMS.DataAccess.Interface;
    using Fanex.Caching;
    using ParlayRMS.DataAccess.Implement;
    using ParlayRMS.Utility;
    using ParlayRMS.DTO.Common;
    using ParlayRMS.DataAccess.Interface.Common;
    using ParlayRMS.DataAccess.Implement.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommonService : BaseService, ICommonService
    {
        private readonly ICommonDataAccess commonDataAccess = new CommonDataAccess();
        private static readonly ICache cacheService = CacheService.CreateInstance(CacheType.MemoryCache);

        /// <summary>
        /// RMS_SelBalanceUpDay
        /// </summary>
        /// <returns></returns>
        public RMS_SelBalanceUpDay GetBalanceUpDayInfo()
        {
            if (!AppSetting.IsCache)
            {
                return this.commonDataAccess.RMS_SelBalanceUpDay();
            }

            string key = "BalanceUpDayInfo";
            var result = cacheService.Get<RMS_SelBalanceUpDay>(key);
            if (result == null)
            {
                result = this.commonDataAccess.RMS_SelBalanceUpDay();
                cacheService.Set<RMS_SelBalanceUpDay>(key, result, TimeSpan.FromMinutes(1));
            }

            return result;
        }
    }
}
