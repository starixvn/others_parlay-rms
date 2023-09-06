namespace ParlayRMS.Utility
{
    using System;

    public static class Formatter
    {
        public static string FormatNumber1(object obj)
        {
            return $"{obj:G29}";
        }

        public static string FormatNumber2(object obj)
        {
            return $"{obj:#,##0.00}";
        }

        public static string FormatNumber3(object obj)
        {
            return $"{obj:#,##0}";
        }

        public static string FormatNumber4(object obj)
        {
            var formattedValue = $"{obj:#,##0.00}";

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
