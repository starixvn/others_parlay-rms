// -----------------------------------------------------------------------
// <copyright file="DataReaderExtension.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Utility.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DataReaderExtension
    {
        public static bool ToBoolean(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToBoolean(reader[fieldName]);
        }

        public static byte ToByte(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToByte(reader[fieldName]);
        }

        public static short ToInt16(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToInt16(reader[fieldName]);
        }

        public static int ToInt32(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToInt32(reader[fieldName]);
        }

        public static long ToInt64(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToInt64(reader[fieldName]);
        }

        public static double ToDouble(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToDouble(reader[fieldName]);
        }

        public static decimal ToDecimal(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToDecimal(reader[fieldName]);
        }

        public static string ToString(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToString(reader[fieldName]);
        }

        public static DateTime ToDateTime(this IDataReader reader, string fieldName)
        {
            return DbConverter.ToDateTime(reader[fieldName]);
        }
    }
}
