namespace ParlayRMS.Web.Context
{
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
    }
}
