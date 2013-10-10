// --------------------------------------------------------------------------
// <copyright file="DateRangeAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Mvc.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Date Range Attribute.
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

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>True if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            if (date >= _minDate && date <= _maxDate)
            {
                return true;
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
            return string.Format("Field {0} must be between {1} and {2}", name, _minDate.ToShortDateString(), _maxDate.ToShortDateString());
        }
    }
}