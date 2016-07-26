namespace ParlayRMS.Web.Mvc.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Values Attribute.
    /// </summary>
    public class ValuesAttribute : ValidationAttribute
    {
        public ValuesAttribute(params int[] values)
        {
            this.Type = typeof(int);
            this.Values = values.Select(x => (object)x).ToArray();
        }

        public ValuesAttribute(params double[] values)
        {
            this.Type = typeof(double);
            this.Values = values.Select(x => (object)x).ToArray();
        }

        public ValuesAttribute(Type type, params object[] values)
        {
            this.Type = type;
            this.Values = values;
        }

        public object[] Values { get; private set; }

        public Type Type { get; private set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>True if the specified value is valid; otherwise, false.</returns>
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

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format("Field {0} only accepts values: {1}", name, string.Join(", ", this.Values));
        }
    }
}