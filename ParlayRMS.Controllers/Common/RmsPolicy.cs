namespace ParlayRMS.Controllers
{
    using System;
    using System.Text;

    public enum RmsPolicy : int
    {
        Create = 4,
        Read = 1,
        Update = 2,
        Delete = 8
    }
}