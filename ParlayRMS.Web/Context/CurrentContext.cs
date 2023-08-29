namespace ParlayRMS.Web.Context
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Web;

    public class CurrentContext : ICurrentContext
    {
        public static ICurrentContext Instance => new CurrentContext();

        public HttpContext Context => HttpContext.Current;

        public string Ip4Address
        {
            get
            {
                var ip = string.Empty;

                foreach (var ipa in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (ipa.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = ipa.ToString();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(ip))
                {
                    foreach (var ipa in Dns.GetHostAddresses(Dns.GetHostName()))
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
                var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return ip;
            }
        }

        public string DomainName => HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

        public string FullDomainName
        {
            get
            {
                var variables = HttpContext.Current.Request.ServerVariables;
                var protocol = variables["SERVER_PORT_SECURE"] == "1" ? "https://" : "http://";
                var domain = variables["SERVER_NAME"];
                var port = variables["SERVER_PORT"];

                return protocol + domain + ":" + port;
            }
        }

        public string CountryCode
        {
            get
            {
                var regionInfo = ResolveCountry(HttpContext.Current.Request);
                if (regionInfo != null)
                {
                    return regionInfo.GeoId.ToString();
                }

                return null;
            }
        }

        private CultureInfo ResolveCulture(HttpRequest request)
        {
            var languages = request.UserLanguages;

            if (languages == null || languages.Length == 0)
            {
                return null;
            }

            try
            {
                var language = languages[0].ToLowerInvariant().Trim();

                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private RegionInfo ResolveCountry(HttpRequest request)
        {
            var culture = ResolveCulture(request);
            if (culture != null)
            {
                return new RegionInfo(culture.LCID);
            }

            return null;
        }
    }
}
