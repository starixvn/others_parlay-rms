namespace ParlayRMS.Controllers
{
    using System.Web.Mvc;
    using ParlayRMS.Web.Mvc;

    [LogActionFilter]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Test()
        {
            return this.View();
        }
    }
}