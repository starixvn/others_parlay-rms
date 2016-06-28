namespace ParlayRMS.Controllers
{
    using System.Web.Mvc;
    using ParlayRMS.Web.Mvc;

    /// <summary>
    /// Home Controller.
    /// </summary>
    [LogActionFilter]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Redirects to Test view.
        /// </summary>
        /// <returns>Test view.</returns>
        public ActionResult Test()
        {
            return this.View();
        }
    }
}