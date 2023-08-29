namespace ParlayRMS.Controllers
{
    using System.Web.Mvc;
    using Web.Mvc;

    [LogActionFilter]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
