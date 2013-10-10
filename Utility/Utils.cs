// --------------------------------------------------------------------------
// <copyright file="Utils.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Utility
{
    using System;
    using System.Collections;

    /// <summary>
    /// Utilities class.
    /// </summary>
    public static class Utils
    {
        public enum TruncateOptions
        {
            /// <summary>
            /// Normal truncation.
            /// </summary>
            None = 0x0,

            /// <summary>
            /// Truncates to the last available word.
            /// </summary>
            FinishWord = 0x1,

            /// <summary>
            /// Allows last word to go over maximum length.
            /// </summary>
            AllowLastWordToGoOverMaxLength = 0x2,

            /// <summary>
            /// Includes '...' at the end of truncated string.
            /// </summary>
            IncludeEllipsis = 0x4
        }

        /// <summary>
        /// Resolves check box.
        /// </summary>
        /// <param name="isChecked">Value indicates that if the check box is checked or not.</param>
        /// <returns>Activation status of check box.</returns>
        public static string ResolveCheckBox(bool isChecked)
        {
            return isChecked ? "active" : "inactive";
        }

        /// <summary>
        /// Formats decimal number.
        /// </summary>
        /// <param name="obj">Decimal value.</param>
        /// <param name="prec">Number of digits after the point.</param>
        /// <returns>The string demonstrates the formatted decimal.</returns>
        public static string ToStringFormat(this decimal obj, int prec = 2)
        {
            var ret = decimal.Round(obj, prec);
            var valRet = ret.ToString("#,##0.########");

            return ret < 0.0m ? "<font color=\"red\">" + valRet + "</font>" : valRet;
        }

        /// <summary>
        /// Resolves permission. Not in use any more.
        /// </summary>
        /// <param name="hasPermission">Has permission indicator.</param>
        /// <returns>Result string.</returns>
        public static string ResolvePermission(bool hasPermission)
        {
            if (!hasPermission)
            {
                return "disabled";
            }

            return string.Empty;
        }

        /// <summary>
        /// Truncates a string to a certain length.
        /// </summary>
        /// <param name="valueToTruncate">Value to truncate.</param>
        /// <param name="maxLength">Maximum length that the string should allow.</param>
        /// <param name="options">Option flags to determine how to truncate the string.
        ///     <para>&#160;&#160;&#160;&#160;<c>None</c>: Just cut off the word at the maximum length.</para>
        ///     <para>&#160;&#160;&#160;&#160;<c>FinishWord</c>: Make sure that the string is not truncated in the middle of a word.</para>
        ///     <para>
        ///         &#160;&#160;&#160;&#160;<c>AllowLastWordToGoOverMaxLength</c>: If FinishWord is set, 
        ///         this allows the string to be longer than the maximum length if there is a word started and not finished before the maximum length.
        ///     </para>
        ///     <para>
        ///         &#160;&#160;&#160;&#160;<c>IncludeEllipsis</c>: Include an ellipsis HTML character at the end of the truncated string.
        ///         This counts as one of the characters for the maximum length.
        ///     </para>
        /// </param>
        /// <returns>Truncated string.</returns>
        public static string TruncateString(string valueToTruncate, int maxLength, TruncateOptions options)
        {
            if (valueToTruncate == null || maxLength <= 0)
            {
                return string.Empty;
            }

            if (valueToTruncate.Length <= maxLength)
            {
                return valueToTruncate;
            }

            bool includeEllipsis = (options & TruncateOptions.IncludeEllipsis) == TruncateOptions.IncludeEllipsis;
            bool finishWord = (options & TruncateOptions.FinishWord) == TruncateOptions.FinishWord;
            bool allowLastWordOverflow = (options & TruncateOptions.AllowLastWordToGoOverMaxLength) == TruncateOptions.AllowLastWordToGoOverMaxLength;

            string retValue = valueToTruncate;

            if (includeEllipsis)
            {
                maxLength -= 1;
            }

            int lastSpaceIndex = retValue.LastIndexOf(" ", maxLength, StringComparison.CurrentCultureIgnoreCase);

            if (!finishWord)
            {
                retValue = retValue.Remove(maxLength);
            }
            else if (allowLastWordOverflow)
            {
                int spaceIndex = retValue.IndexOf(" ", maxLength, StringComparison.CurrentCultureIgnoreCase);
                if (spaceIndex != -1)
                {
                    retValue = retValue.Remove(spaceIndex);
                }
            }
            else if (lastSpaceIndex > -1)
            {
                retValue = retValue.Remove(lastSpaceIndex);
            }

            if (includeEllipsis && retValue.Length < valueToTruncate.Length)
            {
                retValue += "...";
            }

            return retValue;
        }

        /// <summary>
        /// Rounds all properties of an object.
        /// </summary>
        /// <param name="obj">Value object.</param>
        public static void Round(object obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(decimal))
                {
                    var value = (decimal)property.GetValue(obj, null);
                    value = Math.Round(value, 2);
                    property.SetValue(obj, value, null);
                }
            }
        }

        /// <summary>
        /// Rounds the values of a specified list.
        /// </summary>
        /// <param name="list">The list that needs to round the value.</param>
        public static void Round(IEnumerable list)
        {
            foreach (var item in list)
            {
                Round(item);
            }
        }
    }
}