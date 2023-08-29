namespace ParlayRMS.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public static class AppSetting
    {
        public static bool IsDebug => Convert.ToBoolean(GetValue("IsDebug"));

        public static bool IsReCheckIn => DbConverter.ToBoolean(GetValue("RecheckIn"));

        public static string[] HiddenGroups
        {
            get
            {
                var value = GetValue("HiddenGroups");
                return value.Split(';');
            }
        }

        public static bool IsCache => DbConverter.ToBoolean(GetValue("IsCache"));

        public static int PageSize => Convert.ToInt32(GetValue("PageSize"));

        public static string LimitDataFromDate => GetValue("LimitDataFromDate");

        public static string MemberWinLossLimitDate => GetValue("MemberWinLossLimitDate");

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

        public static int MaxPayoutAfterPT => Convert.ToInt32(GetValue("MaxPayoutAfterPT"));

        public static int SystemParlayStakeAfterPT => Convert.ToInt32(GetValue("SystemParlayStakeAfterPT"));

        public static int NetWinStakeAfterPT => Convert.ToInt32(GetValue("NetWinStakeAfterPT"));

        public static int MixParlayStakeAfterPT => Convert.ToInt32(GetValue("MixParlayStakeAfterPT"));

        public static int HouseLoseAmountFrom => Convert.ToInt32(GetValue("HouseLoseAmountFrom"));

        public static int HouseLoseAmountTo => Convert.ToInt32(GetValue("HouseLoseAmountTo"));

        public static string[] SelectTop
        {
            get
            {
                var value = GetValue("SelectTop");
                return value.Split(',');
            }
        }

        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
