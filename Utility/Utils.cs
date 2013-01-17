namespace ParlayRMS.Utility
{
    using System;
    using System.Data;

    public static class Utils
    {
        public enum TruncateOptions
        {
            None = 0x0,
            FinishWord = 0x1,
            AllowLastWordToGoOverMaxLength = 0x2,
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

        /// <summary>
        /// Truncates a string to a certain length
        /// </summary>
        /// <param name="valueToTruncate"></param>
        /// <param name="maxLength">Maximum length that the string should allow</param>
        /// <param name="options">Option flags to determine how to truncate the string
        ///		<para>&#160;&#160;&#160;&#160;<c>None</c>: Just cut off the word at the maximum length</para>
        ///		<para>&#160;&#160;&#160;&#160;<c>FinishWord</c>: Make sure that the string is not truncated in the middle of a word</para>
        ///		<para>&#160;&#160;&#160;&#160;<c>AllowLastWordToGoOverMaxLength</c>: If FinishWord is set, this allows the string to be longer than the maximum length if there is a word started and not finished before the maximum length</para>
        ///		<para>&#160;&#160;&#160;&#160;<c>IncludeEllipsis</c>: Include an ellipsis HTML character at the end of the truncated string.  This counts as one of the characters for the maximum length</para>
        /// </param>
        /// <returns>Truncated string</returns>
        public static string TruncateString(string valueToTruncate, int maxLength, TruncateOptions options)
        {
            if (valueToTruncate == null || maxLength <= 0)
            {
                return "";
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
    }
}