namespace ParlayRMS.Utility
{
    using System;
    using System.Configuration;

    public class CustomConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("elementList", Options = ConfigurationPropertyOptions.IsRequired)]
        public ElementCollection ElementList => (ElementCollection)this["elementList"];
    }

    public class EntryElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key => (string)base["key"];

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value => (string)base["value"];
    }

    [ConfigurationCollection(typeof(EntryElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EntryElement();
        }

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
