// --------------------------------------------------------------------------
// <copyright file="AppSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    /// <summary>
    /// Gets configuration values.
    /// </summary>
    public static class AppSetting
    {
        public static bool IsDebug
        {
            get
            {
                return Convert.ToBoolean(GetValue("IsDebug"));
            }
        }

        public static bool IsReCheckIn
        {
            get
            {
                return DbConverter.ToBoolean(GetValue("RecheckIn"));
            }
        }

        public static string[] HiddenGroups
        {
            get
            {
                var value = GetValue("HiddenGroups");
                return value.Split(';');
            }
        }

        public static bool IsCache
        {
            get
            {
                return DbConverter.ToBoolean(GetValue("IsCache"));
            }
        }

        public static int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
        }

        public static string LimitDataFromDate
        {
            get
            {
                return GetValue("LimitDataFromDate");
            }
        }

        public static string MemberWinLossLimitDate
        {
            get
            {
                return GetValue("MemberWinLossLimitDate");
            }
        }

        public static int UserPolicyCacheTime
        {
            get
            {
                return Convert.ToInt32(GetValue("UserPolicyCacheTime"));
            }
        }

        public static Dictionary<long, string> DangerousAgents
        {
            get
            {
                var customSection = (CustomConfigurationSection)ConfigurationManager.GetSection("dangerousAgents");
                var agentList = new Dictionary<long, string>();

                foreach (EntryElement item in customSection.ElementList)
                {
                    agentList.Add(Convert.ToInt64(item.Key), item.Value);
                }

                return agentList;
            }
        }

        public static int MaxPayoutAfterPT
        {
            get
            {
                return Convert.ToInt32(GetValue("MaxPayoutAfterPT"));
            }
        }

        public static int SystemParlayStakeAfterPT
        {
            get
            {
                return Convert.ToInt32(GetValue("SystemParlayStakeAfterPT"));
            }
        }

        public static int NetWinStakeAfterPT
        {
            get
            {
                return Convert.ToInt32(GetValue("NetWinStakeAfterPT"));
            }
        }

        public static int MixParlayStakeAfterPT
        {
            get
            {
                return Convert.ToInt32(GetValue("MixParlayStakeAfterPT"));
            }
        }

        public static string[] SelectTop
        {
            get
            {
                var value = GetValue("SelectTop");
                return value.Split(',');
            }
        }

        /// <summary>
        /// Gets configuration value of the given key.
        /// </summary>
        /// <param name="key">Configuration key.</param>
        /// <returns>The value associated with the key.</returns>
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}