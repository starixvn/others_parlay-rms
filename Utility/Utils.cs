using System;
using System.Collections;

namespace ParlayRMS.Utility
{
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

        public static string ResolveCheckBox(bool isChecked)
        {
            return isChecked ? "active" : "inactive";
        }

        public static string ToStringFormat(this decimal obj, int prec = 2)
        {
            var ret = decimal.Round(obj, prec);
            var valRet = ret.ToString("#,##0.########");

            return ret < 0.0m ? "<font color=\"red\">" + valRet + "</font>" : valRet;
        }

        public static string ResolvePermission(bool hasPermission)
        {
            if (!hasPermission)
            {
                return "disabled";
            }

            return string.Empty;
        }

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