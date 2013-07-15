namespace ParlayRMS.Utility
{
    using System;

    public static class Formatter
    {
        /// <summary>
        /// 1100.11100 -> 1100.111
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string FormatNumber1(object obj)
        {
            return string.Format("{0:G29}", obj);
        }

        /// <summary>
        /// 1100.10 -> 1,100.1
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string FormatNumber2(object obj)
        {
            return string.Format("{0:#,##0.00}", obj);
        }

        /// <summary>
        /// 1111 -> 1,111
        /// </summary>
        /// <param name="obj">integer number</param>
        /// <returns></returns>
        public static string FormatNumber3(object obj)
        {
            return string.Format("{0:#,##0}", obj);
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("MM/dd/yyyy");
        }
    }
}