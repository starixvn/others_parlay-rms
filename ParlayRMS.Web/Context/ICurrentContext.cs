namespace ParlayRMS.Web.Context
{
    using System.Web;

    public interface ICurrentContext
    {
        HttpContext Context { get; }

        string Ip4Address { get; }
    }
}
