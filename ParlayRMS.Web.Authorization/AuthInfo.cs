// --------------------------------------------------------------------------
// <copyright file="AuthInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Authorization
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Authentication Information.
    /// </summary>
    public class AuthInfo
    {
        #region Properties

        private int _userId;

        private int _groupId;

        private string _groupName;

        public int UserId
        {
            get 
            { 
                return _userId; 
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("userid");
                }

                _userId = value;
            }
        }

        public int GroupId
        {
            get 
            { 
                return _groupId; 
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("GroupId");
                }

                _groupId = value;
            }
        }

        public string GroupName
        {
            get 
            { 
                return _groupName; 
            }

            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("_groupName");
                }

                _groupName = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Creates the user data.
        /// </summary>
        /// <param name="authInfoXml">XML string contains the authentication information.</param>
        /// <returns>Authentication information.</returns>
        public static AuthInfo CreateUserData(string authInfoXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AuthInfo));
            AuthInfo authInfo = (AuthInfo)serializer.Deserialize(new StringReader(authInfoXml));
            return authInfo;
        }

        /// <summary>
        /// Converts the authentication information to the XML.
        /// </summary>
        /// <returns>XML string contains the authentication information.</returns>
        public string ToXml()
        {
            string xml = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AuthInfo));
                serializer.Serialize(sw, this);
                sw.Close();
                xml = sw.ToString();
            }

            return xml;
        }
    }
}