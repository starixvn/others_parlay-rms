namespace ParlayRMS.Web.Mvc.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;

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
            return date >= _minDate && date <= _maxDate;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Field {name} must be between {_minDate.ToShortDateString()} and {_maxDate.ToShortDateString()}";
        }
    }
}
