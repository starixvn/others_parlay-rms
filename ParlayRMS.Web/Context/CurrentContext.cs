namespace ParlayRMS.Web.Context
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Net;
    using System.Web;

    /// <summary>
    /// Current Context.
    /// </summary>
    public class CurrentContext : ICurrentContext
    {
        public static ICurrentContext Instance
        {
            get
            {
                return new CurrentContext();
            }
        }

        public HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        public string Ip4Address
        {
            get
            {
                string ip = string.Empty;

                foreach (IPAddress ipa in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (ipa.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = ipa.ToString();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(ip))
                {
                    foreach (IPAddress ipa in Dns.GetHostAddresses(Dns.GetHostName()))
                    {
                        if (ipa.AddressFamily.ToString().Equals("InterNetwork"))
                        {
                            ip = ipa.ToString();
                            break;
                        }
                    }
                }

                return ip.Trim();
            }
        }

        public string Ip6Address
        {
            get
            {
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return ip;
            }
        }

        public string DomainName
        {
            get { return HttpContext.Current.Request.ServerVariables["SERVER_NAME"]; }
        }

        public string FullDomainName
        {
            get
            {
                NameValueCollection variables = HttpContext.Current.Request.ServerVariables;
                string protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
                string domain = variables["SERVER_NAME"];
                string port = variables["SERVER_PORT"];

                return protocol + domain + ":" + port;
            }
        }

        public string CountryCode
        {
            get
            {
                RegionInfo regionInfo = this.ResolveCountry(HttpContext.Current.Request);
                if (regionInfo != null)
                {
                    return regionInfo.GeoId.ToString();
                }

                return null;
            }
        }

        /// <summary>
        /// Get the language ID which is passed from the browser and return the associated CultureInfo object.
        /// </summary>
        /// <param name="request">The HttpRequest object which is received from client.</param>
        /// <returns>CultureInfo object contains culture of the language which the user is using.</returns>
        private CultureInfo ResolveCulture(HttpRequest request)
        {
            string[] languages = request.UserLanguages;

            if (languages == null || languages.Length == 0)
            {
                return null;
            }

            try
            {
                string language = languages[0].ToLowerInvariant().Trim();

                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the country ID which is passed from the browser and return the associated RegionInfo object.
        /// </summary>
        /// <param name="request">The HttpRequest object which is received from client.</param>
        /// <returns>RegionInfo object contains the information of user's region.</returns>
        private RegionInfo ResolveCountry(HttpRequest request)
        {
            CultureInfo culture = this.ResolveCulture(request);
            if (culture != null)
            {
                return new RegionInfo(culture.LCID);
            }

            return null;
        }
    }
}