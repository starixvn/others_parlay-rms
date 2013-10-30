// --------------------------------------------------------------------------
// <copyright file="CustomConfigurationSection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.Utility
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    
    /// <summary>
    /// Custom section.
    /// </summary>
    public class CustomConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("elementList", Options = ConfigurationPropertyOptions.IsRequired)]
        public ElementCollection ElementList
        {
            get
            {
                return (ElementCollection)this["elementList"];
            }
        }
    }

    /// <summary>
    /// Custom element.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Need to review here.")]
    public class EntryElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return (string)base["key"];
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)base["value"];
            }
        }
    }

    /// <summary>
    /// Custom element collection.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Need to review here.")]
    [ConfigurationCollection(typeof(EntryElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EntryElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">ArgumentNullException will be thrown if the passed element is invalid.</exception>
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return ((EntryElement)element).Key;
        }
    }
}