namespace ParlayRMS.Web.Context
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Net;
    using System.Web;

    public class CurrentContext : ICurrentContext
    {
        private static ICurrentContext instance = null;

        public static ICurrentContext Instance
        {
            get
            {
                return instance ?? new CurrentContext();
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