// -----------------------------------------------------------------------
// <copyright file="Converter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DbConverter
    {
        public static bool ToBoolean(object value)
        {
            bool result = false;
            if (IsValid(value))
            {
                bool.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static byte ToByte(object value)
        {
            byte result = 0;
            if (IsValid(value))
            {
                byte.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static short ToInt16(object value)
        {
            short result = 0;
            if (IsValid(value))
            {
                short.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static int ToInt32(object value)
        {
            int result = 0;
            if (IsValid(value))
            {
                int.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static long ToInt64(object value)
        {
            long result = 0;
            if (IsValid(value))
            {
                long.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static double ToDouble(object value)
        {
            double result = 0.0d;
            if (IsValid(value))
            {
                double.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static decimal ToDecimal(object value)
        {
            decimal result = 0.0m;
            if (IsValid(value))
            {
                decimal.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static string ToString(object value)
        {
            string result = string.Empty;
            if (IsValid(value))
            {
                result = Convert.ToString(value);
            }

            return result;
        }

        public static DateTime ToDateTime(object value)
        {
            DateTime result = DateTime.MinValue;
            if (IsValid(value))
            {
                DateTime.TryParse(value.ToString(), out result);
            }

            return result;
        }

        private static bool IsValid(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            return true;
        }
    }
}
