// --------------------------------------------------------------------------
// <copyright file="MinStringLengthAttribute.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Extension
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    /// <summary>
    /// Min String Length Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MinStringLengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = 6;

        public MinStringLengthAttribute(int minCharacters)
            : base(DefaultErrorMessage)
        {
            this._minCharacters = minCharacters;
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, this.ErrorMessageString, name, this._minCharacters);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>True if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return valueAsString != null && valueAsString.Length >= this._minCharacters;
        }
    }
}