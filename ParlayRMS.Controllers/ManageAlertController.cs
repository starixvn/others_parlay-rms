using System.Configuration;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;

    [NapAuth(FunctionKey = FunctionKeys.ManageAlert, Policy = Constant.Policy.Read)]
    public class ManageAlertController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var aspNetCoreAuth = ConfigurationManager.AppSettings["AspNetCoreAuth"];
            ViewBag.AspNetCoreApiUrl = !string.IsNullOrEmpty(aspNetCoreAuth) ? $"{aspNetCoreAuth}/" : string.Empty;
            ViewBag.WebUrl = Request.RawUrl.Replace("ManageAlert", "");
            ViewBag.UserId = CurrentSession.UserId;
            ViewBag.UserName = CurrentSession.Username;
            return View("~/wwwroot/manage-alert-letstalk/index.cshtml");
        }
    }
}
