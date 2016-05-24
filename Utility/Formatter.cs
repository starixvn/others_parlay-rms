// --------------------------------------------------------------------------
// <copyright file="Formatter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Utility
{
    using System;

    /// <summary>
    /// Includes formatting number functions.
    /// </summary>
    public static class Formatter
    {
        /// <summary>
        /// Formats number such as: 1100.11100 -> 1100.111.
        /// </summary>
        /// <param name="obj">The object that needs to be formatted.</param>
        /// <returns>Formatted value.</returns>
        public static string FormatNumber1(object obj)
        {
            return string.Format("{0:G29}", obj);
        }

        /// <summary>
        /// Formats number such as: 1100.10 -> 1,100.1.
        /// </summary>
        /// <param name="obj">The object that needs to be formatted.</param>
        /// <returns>Formatted value.</returns>
        public static string FormatNumber2(object obj)
        {
            return string.Format("{0:#,##0.00}", obj);
        }

        /// <summary>
        /// Formats number such as: 1111 -> 1,111.
        /// </summary>
        /// <param name="obj">Integer number that needs to be formatted.</param>
        /// <returns>Formatted value.</returns>
        public static string FormatNumber3(object obj)
        {
            return string.Format("{0:#,##0}", obj);
        }

        /// <summary>
        /// Formats number such as: 1111.20 -> 1,111.2.
        /// </summary>
        /// <param name="obj">Integer number that needs to be formatted.</param>
        /// <returns>Formatted value.</returns>
        public static string FormatNumber4(object obj)
        {
            string formattedValue = string.Format("{0:#,##0.00}", obj);

            if (formattedValue.EndsWith("00"))
            {
                formattedValue = formattedValue.Remove(formattedValue.Length - 3);
            }
            else if (formattedValue.EndsWith("0"))
            {
                formattedValue = formattedValue.Remove(formattedValue.Length - 1);
            }

            return formattedValue;
        }

        /// <summary>
        /// Formats date.
        /// </summary>
        /// <param name="date">The date value that needs to be formatted.</param>
        /// <returns>Formatted value, such as: 10/10/2013.</returns>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("MM/dd/yyyy");
        }

        public static string FormatWithoutRounding(decimal value)
        {
            return FormatNumber3(Math.Floor(value));
        }
    }
}