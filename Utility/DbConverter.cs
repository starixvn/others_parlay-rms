// --------------------------------------------------------------------------
// <copyright file="DbConverter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Utility
{
    using System;

    /// <summary>
    /// Converts values queried from Database to System type values.
    /// </summary>
    public static class DbConverter
    {
        /// <summary>
        /// Converts to Boolean value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static bool ToBoolean(object value)
        {
            bool result = false;
            if (IsValid(value))
            {
                bool.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Converts to Byte value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static byte ToByte(object value)
        {
            byte result = 0;
            if (IsValid(value))
            {
                byte.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Converts to 2-byte integer value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static short ToInt16(object value)
        {
            short result = 0;
            if (IsValid(value))
            {
                short.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Converts to 4-byte integer value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static int ToInt32(object value)
        {
            int result = 0;
            if (IsValid(value))
            {
                int.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Converts to 4-byte integer value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static long ToInt64(object value)
        {
            long result = 0;
            if (IsValid(value))
            {
                long.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Converts to Double value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static double ToDouble(object value)
        {
            double result = 0.0d;
            if (IsValid(value))
            {
                double.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Converts to Decimal value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static decimal ToDecimal(object value)
        {
            decimal result = 0.0m;
            if (IsValid(value))
            {
                decimal.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Converts to String value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static string ToString(object value)
        {
            string result = string.Empty;
            if (IsValid(value))
            {
                result = Convert.ToString(value);
            }

            return result;
        }

        /// <summary>
        /// Converts to DateTime value.
        /// </summary>
        /// <param name="value">Value that will be converted.</param>
        /// <returns>Converted value.</returns>
        public static DateTime ToDateTime(object value)
        {
            DateTime result = DateTime.MinValue;
            if (IsValid(value))
            {
                DateTime.TryParse(value.ToString(), out result);
            }

            return result;
        }

        /// <summary>
        /// Check a value is null or not.
        /// </summary>
        /// <param name="value">Value that needs to check.</param>
        /// <returns>Whether or not the passed value is valid.</returns>
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