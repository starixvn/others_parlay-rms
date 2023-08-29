namespace ParlayRMS.Utility
{
    using System;

    public static class DbConverter
    {
        public static bool ToBoolean(object value)
        {
            var result = false;
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
            var result = 0;
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
            var result = 0.0d;
            if (IsValid(value))
            {
                double.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static decimal ToDecimal(object value)
        {
            var result = 0.0m;
            if (IsValid(value))
            {
                decimal.TryParse(value.ToString(), out result);
            }

            return result;
        }

        public static string ToString(object value)
        {
            var result = string.Empty;
            if (IsValid(value))
            {
                result = Convert.ToString(value);
            }

            return result;
        }

        public static DateTime ToDateTime(object value)
        {
            var result = DateTime.MinValue;
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
