namespace ParlayRMS.ViewModel.Extension
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class MaxStringLengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' must be under {1} characters.";
        private readonly int _maxCharacters = 50;

        public MaxStringLengthAttribute(int maxCharacters)
            : base(DefaultErrorMessage)
        {
            _maxCharacters = maxCharacters;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, _maxCharacters);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            return valueAsString != null && valueAsString.Length < _maxCharacters;
        }
    }
}
