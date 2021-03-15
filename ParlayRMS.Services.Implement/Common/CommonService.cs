// --------------------------------------------------------------------------
// <copyright file="CommonService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Common
{
    using System;
    using System.Collections.Generic;
    using Fanex.Caching;
    using ParlayRMS.DataAccess.Implement.Common;
    using ParlayRMS.DTO.Common;
    using ParlayRMS.Utility;

    /// <summary>
    /// Common Services.
    /// </summary>
    public class CommonService : BaseService
    {
        private static readonly CacheService CacheServiceInstance = new CacheServiceImpl();
        private readonly CommonDataAccess _commonDataAccess = new CommonDataAccess();

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
                CacheServiceInstance.Set<RMS_SelBalanceUpDay>(key, result, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(1) });
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
                CacheServiceInstance.Set<RMS_Rpt_Parlay_SelMinTransdate>(key, result, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(1) });
            }

            return result;
        }

        /// <summary>
        /// Get the list of bet type belong Bet radar.
        /// </summary>
        /// <returns>
        /// Return the list bet type.
        /// </returns>
        public List<int> GetBettypeBelongsBetRadar()
        {
            if (!AppSetting.IsCache)
            {
                return this._commonDataAccess.GetBettypeBelongsBetRadar();
            }

            string key = "BettypeBelongsBetRadar";
            var result = CacheServiceInstance.Get<List<int>>(key);
            if (result == null)
            {
                result = this._commonDataAccess.GetBettypeBelongsBetRadar();
                CacheServiceInstance.Set<List<int>>(key, result, new CacheItemOptions { RelativeExpiration = TimeSpan.FromDays(1) });
            }

            return result;
        }

        /// <summary>
        /// Get currencies.
        /// </summary>
        /// <returns>
        /// Return currency list.
        /// </returns>
        public IEnumerable<Currency> GetCurrencies()
        {
            return this._commonDataAccess.GetCurrencies();
        }
    }
}
