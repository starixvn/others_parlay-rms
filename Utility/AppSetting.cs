namespace ParlayRMS.Utility
{
    using System;
    using System.Configuration;

    public static class AppSetting
    {
        public static bool IsCache => DbConverter.ToBoolean(GetValue("IsCache"));

        public static int PageSize => Convert.ToInt32(GetValue("PageSize"));

        public static string LimitDataFromDate => GetValue("LimitDataFromDate");

        public static string MemberWinLossLimitDate => GetValue("MemberWinLossLimitDate");

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
