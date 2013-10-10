// --------------------------------------------------------------------------
// <copyright file="PropertiesMustMatchAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Extension
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using ParlayRMS.Utility;

    /// <summary>
    /// Properties Must Match Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            this.OriginalProperty = originalProperty;
            this.ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }

        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            // temp hard code here
            return string.Format(CultureInfo.CurrentUICulture, this.ErrorMessageString, ViewLang.Newpassword, ViewLang.Confirmpassword);

            // return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
            //    OriginalProperty, ConfirmProperty);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>True if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(this.OriginalProperty, true).GetValue(value);
            object confirmValue = properties.Find(this.ConfirmProperty, true).GetValue(value);
            return object.Equals(originalValue, confirmValue);
        }
    }
}