// --------------------------------------------------------------------------
// <copyright file="CommonService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Common
{
    using System;
    using Fanex.Caching;
    using ParlayRMS.DataAccess.Implement.Common;
    using ParlayRMS.DataAccess.Interface.Common;
    using ParlayRMS.DTO.Common;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Utility;

    /// <summary>
    /// Common Services.
    /// </summary>
    public class CommonService : BaseService, ICommonService
    {
        private static readonly ICache CacheServiceInstance = CacheService.CreateInstance(CacheType.MemoryCache);
        private readonly ICommonDataAccess _commonDataAccess = new CommonDataAccess();

        /// <summary>
        /// Gets balance up day information.
        /// </summary>
        /// <returns>Balance up day information.</returns>
        public RMS_SelBalanceUpDay GetBalanceUpDayInfo()
        {
            if (!AppSetting.IsCache)
            {
                return this._commonDataAccess.RMS_SelBalanceUpDay();
            }

            string key = "BalanceUpDayInfo";
            var result = CacheServiceInstance.Get<RMS_SelBalanceUpDay>(key);
            if (result == null)
            {
                result = this._commonDataAccess.RMS_SelBalanceUpDay();
                CacheServiceInstance.Set<RMS_SelBalanceUpDay>(key, result, TimeSpan.FromMinutes(1));
            }

            return result;
        }

        /// <summary>
        /// Gets min TRANSDATE.
        /// </summary>
        /// <returns>Balance up day information.</returns>
        public RMS_Rpt_Parlay_SelMinTransdate GetMinTransDate()
        {
            if (!AppSetting.IsCache)
            {
                return this._commonDataAccess.RMS_Rpt_Parlay_SelMinTransdate();
            }

            string key = "MinTransDate";
            var result = CacheServiceInstance.Get<RMS_Rpt_Parlay_SelMinTransdate>(key);
            if (result == null)
            {
                result = this._commonDataAccess.RMS_Rpt_Parlay_SelMinTransdate();
                CacheServiceInstance.Set<RMS_Rpt_Parlay_SelMinTransdate>(key, result, TimeSpan.FromMinutes(1));
            }

            return result;
        }
    }
}
