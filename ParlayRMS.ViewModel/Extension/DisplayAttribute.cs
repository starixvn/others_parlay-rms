// -----------------------------------------------------------------------
// <copyright file="DisplayAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.Extension
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMSDisplayAttribute : DisplayNameAttribute
    {
        private readonly PropertyInfo nameProperty;

        public RMSDisplayAttribute(string displayNameKey, Type resourceType)
            : base(displayNameKey)
        {
            if (resourceType != null)
            {
                this.nameProperty = resourceType.GetProperty(base.DisplayName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }

        public override string DisplayName
        {
            get
            {
                if (this.nameProperty == null)
                {
                    return base.DisplayName;
                }

                return (string)this.nameProperty.GetValue(this.nameProperty.DeclaringType, null);
            }
        }
    }
}