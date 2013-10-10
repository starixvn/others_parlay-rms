// --------------------------------------------------------------------------
// <copyright file="RMSDisplayAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Extension
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// RMS Display Attribute.
    /// </summary>
    public class RMSDisplayAttribute : DisplayNameAttribute
    {
        private readonly PropertyInfo _nameProperty;

        public RMSDisplayAttribute(string displayNameKey, Type resourceType)
            : base(displayNameKey)
        {
            if (resourceType != null)
            {
                this._nameProperty = resourceType.GetProperty(base.DisplayName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }

        public override string DisplayName
        {
            get
            {
                if (this._nameProperty == null)
                {
                    return base.DisplayName;
                }

                return (string)this._nameProperty.GetValue(this._nameProperty.DeclaringType, null);
            }
        }
    }
}