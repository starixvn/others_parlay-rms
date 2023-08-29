namespace ParlayRMS.Web.Authorization
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public class AuthInfo
    {
        #region Properties

        private int _userId;

        private int _groupId;

        private string _groupName;

        public int UserId
        {
            get => _userId;

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
            get => _groupId;

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
            get => _groupName;

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

        public static AuthInfo CreateUserData(string authInfoXml)
        {
            var serializer = new XmlSerializer(typeof(AuthInfo));
            var authInfo = (AuthInfo)serializer.Deserialize(new StringReader(authInfoXml));
            return authInfo;
        }

        public string ToXml()
        {
            var xml = string.Empty;
            using (var sw = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(AuthInfo));
                serializer.Serialize(sw, this);
                sw.Close();
                xml = sw.ToString();
            }

            return xml;
        }
    }
}
