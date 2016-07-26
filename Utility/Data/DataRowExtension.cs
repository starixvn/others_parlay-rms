namespace ParlayRMS.Utility.Data
{
    using System;
    using System.Data;

    /// <summary>
    /// Data row extensions.
    /// </summary>
    public static class DataRowExtension
    {
        /// <summary>
        /// Converts value of a field to Boolean.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static bool ToBoolean(this DataRow dr, string fieldName)
        {
            return DbConverter.ToBoolean(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to Byte.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static byte ToByte(this DataRow dr, string fieldName)
        {
            return DbConverter.ToByte(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to 2-byte integer.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static short ToInt16(this DataRow dr, string fieldName)
        {
            return DbConverter.ToInt16(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to 4-byte integer.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static int ToInt32(this DataRow dr, string fieldName)
        {
            return DbConverter.ToInt32(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to 8-byte integer.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static long ToInt64(this DataRow dr, string fieldName)
        {
            return DbConverter.ToInt64(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to Double.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static double ToDouble(this DataRow dr, string fieldName)
        {
            return DbConverter.ToDouble(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to Decimal.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static decimal ToDecimal(this DataRow dr, string fieldName)
        {
            return DbConverter.ToDecimal(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to String.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static string ToString(this DataRow dr, string fieldName)
        {
            return DbConverter.ToString(dr[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to DateTime.
        /// </summary>
        /// <param name="dr">Data Row object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static DateTime ToDateTime(this DataRow dr, string fieldName)
        {
            return DbConverter.ToDateTime(dr[fieldName]);
        }
    }
}
