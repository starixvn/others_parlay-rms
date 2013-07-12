// -----------------------------------------------------------------------
// <copyright file="DateRangeAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Web.Mvc.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        private DateTime _minDate;
        private DateTime _maxDate;

        public DateRangeAttribute(string minDate, string maxDate)
        {
            _minDate = Convert.ToDateTime(minDate);
            _maxDate = Convert.ToDateTime(maxDate);
        }

        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            if (date >= _minDate && date <= _maxDate)
            {
                return true;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Field {0} must be between {1} and {2}", name, _minDate.ToShortDateString(), _maxDate.ToShortDateString());
        }
    }
}