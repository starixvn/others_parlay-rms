// -----------------------------------------------------------------------
// <copyright file="ValuesAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Web.Mvc.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValuesAttribute : ValidationAttribute
    {
        public object[] Values { get; private set; }

        public Type Type { get; private set; }

        public ValuesAttribute(params int[] values)
        {
            this.Type = typeof(int);
            this.Values = values.Select(x => (object)x).ToArray();
        }

        public ValuesAttribute(params double[] values)
        {
            this.Type = typeof(double);
            this.Values = values.Select(x=> (object)x).ToArray();
        }

        public ValuesAttribute(Type type, params object[] values)
        {
            this.Type = type;
            this.Values = values;
        }

        public override bool IsValid(object value)
        {
            foreach (var v in this.Values)
            {
                if (object.Equals(v, value))
                {
                    return true;
                }
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Field {0} only accepts values: {1}", name, string.Join(", ", this.Values));
        }
    }
}