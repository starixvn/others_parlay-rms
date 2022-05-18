using ParlayRMS.Models.Functions;
using ParlayRMS.Web.Authorization;
using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.Winloss, Policy = UserPolicy.Read)]
    public class WinLossRiskControlController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
            ViewBag.AspNetCoreApiUrl = !string.IsNullOrEmpty(aspNetCoreAuth) ? $"{aspNetCoreAuth}/" : string.Empty;
            ViewBag.UserId = CurrentSession.UserId;
            return View("~/wwwroot/win-loss-risk-control/index.cshtml");
        }
    }
}