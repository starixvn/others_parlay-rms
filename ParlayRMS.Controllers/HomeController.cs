namespace ParlayRMS.Controllers
{
    using System.Web.Mvc;

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