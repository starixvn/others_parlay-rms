using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;

    [NapAuth(FunctionKey = FunctionKeys.LeagueEventMonitor, Policy = Constant.Policy.Read)]
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
