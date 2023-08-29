using System;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    public class ServerTimeController : Controller
    {
        [HttpPost]
        public ActionResult Today()
        {
            return Json(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day));
        }
    }
}
