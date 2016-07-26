namespace ParlayRMS.Utility.Data
{
    using System;
    using System.Data;

    /// <summary>
    /// Data reader extensions.
    /// </summary>
    public static class DataReaderExtension
    {
        /// <summary>
        /// Converts value of a field to Boolean.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static bool ToBoolean(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToBoolean(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to Byte.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static byte ToByte(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToByte(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to 2-byte integer.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static short ToInt16(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToInt16(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to 4-byte integer.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static int ToInt32(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToInt32(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to 8-byte integer.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static long ToInt64(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToInt64(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to Double.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static double ToDouble(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToDouble(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to Decimal.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static decimal ToDecimal(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToDecimal(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to String.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static string ToString(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToString(reader[fieldName]);
        }

        /// <summary>
        /// Converts value of a field to DateTime.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Converted value.</returns>
        public static DateTime ToDateTime(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToDateTime(reader[fieldName]);
        }
    }
}
