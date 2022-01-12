using ParlayRMS.Models.Functions;
using ParlayRMS.Web.Authorization;
using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.LeagueEventMonitor, Policy = UserPolicy.Read)]
    public class LeagueEventMonitorController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
            ViewBag.AspNetCoreApiUrl = !string.IsNullOrEmpty(aspNetCoreAuth) ? $"{aspNetCoreAuth}/" : string.Empty;
            return View("~/wwwroot/league-event-monitor/index.cshtml");
        }
    }
}
