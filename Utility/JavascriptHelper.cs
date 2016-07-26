namespace ParlayRMS.Utility
{
    /// <summary>
    /// JavaScript helper.
    /// </summary>
    public static class JavascriptHelper
    {
        /// <summary>
        /// Converts boolean value to string.
        /// </summary>
        /// <param name="value">Boolean value.</param>
        /// <returns>String that represents passed boolean value.</returns>
        public static string GetString(bool value)
        {
            return value.ToString().ToLower();
        }
    }
}
