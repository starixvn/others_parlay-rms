namespace ParlayRMS.Utility
{
    public class Formatter
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
            return string.Format("{0:#,##0.##}", obj);
        }
    }
}