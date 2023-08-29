namespace ParlayRMS.ViewModel.Extension
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class MinStringLengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = 6;

        public MinStringLengthAttribute(int minCharacters)
            : base(DefaultErrorMessage)
        {
            _minCharacters = minCharacters;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            return valueAsString != null && valueAsString.Length >= _minCharacters;
        }
    }
}
