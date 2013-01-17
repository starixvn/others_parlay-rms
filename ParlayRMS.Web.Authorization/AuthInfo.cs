using System;
using System.IO;
using System.Xml.Serialization;

namespace ParlayRMS.Web.Authorization
{
    public class AuthInfo
    {
        #region Properties

        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("userid");
                _userId = value;
            }
        }

        private int _groupId;

        public int GroupId
        {
            get { return _groupId; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("GroupId");
                _groupId = value;
            }
        }

        private string _groupName;

        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("_groupName");

                _groupName = value;
            }
        }

        #endregion Properties

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

        public static AuthInfo CreateUserData(string authInfoXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AuthInfo));
            AuthInfo authInfo = (AuthInfo)serializer.Deserialize(new StringReader(authInfoXml));
            return authInfo;
        }
    }
}