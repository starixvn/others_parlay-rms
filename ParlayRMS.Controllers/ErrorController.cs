namespace ParlayRMS.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : BaseController
    {
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
