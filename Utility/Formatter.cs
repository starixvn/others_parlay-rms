namespace ParlayRMS.Utility
{
    using System;

    public static class Formatter
    {
        public static string FormatNumber1(object obj)
        {
            return string.Format("{0:G29}", obj);
        }

        public static string FormatNumber2(object obj)
        {
            return string.Format("{0:#,##0.00}", obj);
        }

        public static string FormatNumber3(object obj)
        {
            return string.Format("{0:#,##0}", obj);
        }

        public static string FormatNumber4(object obj)
        {
            var formattedValue = string.Format("{0:#,##0.00}", obj);

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

        public static string FormatDate(DateTime date)
        {
            return date.ToString("MM/dd/yyyy");
        }

        public static string FormatDateTime(DateTime date)
        {
            return date.ToString("MM/dd/yyyy h:mm:ss tt");
        }

        public static string FormatWithoutRounding(decimal value)
        {
            return FormatNumber3(Math.Floor(value));
        }

        public static string FormatOdds(decimal value)
        {
            return value < 10
                ? FormatNumber2(value)
                : FormatWithoutRounding(value);
        }
    }
}
