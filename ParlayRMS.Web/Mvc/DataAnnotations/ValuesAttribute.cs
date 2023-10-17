namespace ParlayRMS.Web.Mvc.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ValuesAttribute : ValidationAttribute
    {
        public ValuesAttribute(params int[] values)
        {
            Type = typeof(int);
            Values = values.Select(x => (object)x).ToArray();
        }

        public ValuesAttribute(params double[] values)
        {
            Type = typeof(double);
            Values = values.Select(x => (object)x).ToArray();
        }

        public ValuesAttribute(Type type, params object[] values)
        {
            Type = type;
            Values = values;
        }

        public object[] Values { get; private set; }

        public Type Type { get; private set; }

        public override bool IsValid(object value)
        {
            foreach (var v in Values)
            {
                if (Equals(v, value))
                {
                    return true;
                }
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Field {name} only accepts values: {string.Join(", ", Values)}";
        }
    }
}
