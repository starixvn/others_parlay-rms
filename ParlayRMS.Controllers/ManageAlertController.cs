using ParlayRMS.Models.Functions;
using ParlayRMS.Web.Authorization;
using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Read)]
    public class ManageAlertController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
            ViewBag.AspNetCoreApiUrl = !string.IsNullOrEmpty(aspNetCoreAuth) ? $"{aspNetCoreAuth}/" : string.Empty;
            ViewBag.WebUrl = $"{Request.Url.Scheme}://{Request.Url.Host}";
            ViewBag.UserId = CurrentSession.UserId;
            ViewBag.UserName = CurrentSession.UserName;
            return View("~/wwwroot/manage-alert-letstalk/index.cshtml");
        }
    }
}
