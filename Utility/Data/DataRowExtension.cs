// -----------------------------------------------------------------------
// <copyright file="DataRowExtension.cs" company="">
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
    public static class DataRowExtension
    {
        public static bool ToBoolean(this DataRow dr, string fieldName)
        {
            return DbConverter.ToBoolean(dr[fieldName]);
        }

        public static byte ToByte(this DataRow dr, string fieldName)
        {
            return DbConverter.ToByte(dr[fieldName]);
        }

        public static short ToInt16(this DataRow dr, string fieldName)
        {
            return DbConverter.ToInt16(dr[fieldName]);
        }

        public static int ToInt32(this DataRow dr, string fieldName)
        {
            return DbConverter.ToInt32(dr[fieldName]);
        }

        public static long ToInt64(this DataRow dr, string fieldName)
        {
            return DbConverter.ToInt64(dr[fieldName]);
        }

        public static double ToDouble(this DataRow dr, string fieldName)
        {
            return DbConverter.ToDouble(dr[fieldName]);
        }

        public static decimal ToDecimal(this DataRow dr, string fieldName)
        {
            return DbConverter.ToDecimal(dr[fieldName]);
        }

        public static string ToString(this DataRow dr, string fieldName)
        {
            return DbConverter.ToString(dr[fieldName]);
        }

        public static DateTime ToDateTime(this DataRow dr, string fieldName)
        {
            return DbConverter.ToDateTime(dr[fieldName]);
        }
    }
}
