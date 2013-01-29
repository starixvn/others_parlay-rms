namespace ParlayRMS.Utility
{
    using System.Configuration;
    using System;

    public static class AppSetting
    {
        /// <summary>
        ///
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                return Convert.ToBoolean(GetValue("IsDebug"));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static bool IsReCheckIn
        {
            get
            {
                return DbConverter.ToBoolean(GetValue("RecheckIn"));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string[] HiddenGroups
        {
            get
            {
                var value = GetValue("HiddenGroups");
                return value.Split(';');
                //return ;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static bool IsCache
        {
            get
            {
                return DbConverter.ToBoolean(GetValue("IsCache"));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string LBUrl
        {
            get
            {
                return GetValue("LBUrl");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static bool IsUM
        {
            get
            {
                return DbConverter.ToBoolean(GetValue("IsUM"));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string TimeFrom
        {
            get
            {
                return GetValue("TimeFrom");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string TimeTo
        {
            get
            {
                return GetValue("TimeTo");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static int PageSize
        {
            get
            {
                return Convert.ToInt32(GetValue("PageSize"));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string LimitDataFromDate
        {
            get
            {
                return GetValue("LimitDataFromDate");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}