namespace ParlayRMS.ViewModel.Extension
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class RMSDisplayAttribute : DisplayNameAttribute
    {
        private readonly PropertyInfo _nameProperty;

        public RMSDisplayAttribute(string displayNameKey, Type resourceType)
            : base(displayNameKey)
        {
            if (resourceType != null)
            {
                _nameProperty = resourceType.GetProperty(base.DisplayName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }

        public override string DisplayName
        {
            get
            {
                if (_nameProperty == null)
                {
                    return base.DisplayName;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }
}
