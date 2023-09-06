using System;
using System.Collections;

namespace ParlayRMS.Utility
{
    public static class Utils
    {
        public static string ResolvePermission(bool hasPermission)
        {
            return !hasPermission ? "disabled" : string.Empty;
        }

      
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

        public static void Round(IEnumerable list)
        {
            foreach (var item in list)
            {
                Round(item);
            }
        }

        public static string DisplayMax15Character(string data)
        {
            if (data.Length > 15)
            {
                return "..." + data.Substring(data.Length - 15);
            }
            return data;
        }
    }
}
