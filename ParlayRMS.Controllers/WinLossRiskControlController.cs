using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    // [Authorization(Function = FunctionKeys.WinLossRiskControl, Policy = UserPolicy.Read)]
    public class WinLossRiskControlController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.AspNetCoreApiUrl = GetAspNetCoreApi();
            return View("~/wwwroot/win-loss-risk-control/index.cshtml");
        }

        private string GetAspNetCoreApi()
        {
            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
            if (!string.IsNullOrEmpty(aspNetCoreAuth))
            {
                var applicationPath = Request.ApplicationPath.Replace("/", "");
                applicationPath = "NetCore" + applicationPath;
                return aspNetCoreAuth.Replace("{ApplicationPath}", applicationPath).Replace("PropagationAuth", "");
            }

            return string.Empty;
        }
    }
}