using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;

    [NapAuth(FunctionKey = FunctionKeys.Winloss, Policy = Constant.Policy.Read)]
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
