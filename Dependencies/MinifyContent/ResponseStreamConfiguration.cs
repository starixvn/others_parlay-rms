using System.Configuration;

namespace MinifyContent
{
    /// <summary>
    /// Response Configuration Section
    /// </summary>
    class ResponseStreamConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Spaces replace with single space
        /// </summary>
        [ConfigurationProperty("supressWithWhiteSpace", DefaultValue = "false", IsRequired= false)]
        public bool SupressWithWhiteSpace
        {
            get
            {
                return (bool)this["supressWithWhiteSpace"];
            }
            set
            {
                this["supressWithWhiteSpace"] = value;
            }
        }

        /// <summary>
        /// Confirm Html comment tag is in javascript on page
        /// </summary>
        [ConfigurationProperty("isHtmlCommentTagInJS", DefaultValue = "false", IsRequired = false)]
        public bool IsHtmlCommentTagInJS
        {
            get
            {
                return (bool)this["isHtmlCommentTagInJS"];
            }
            set
            {
                this["isHtmlCommentTagInJS"] = value;
            }
        }
    }
}
