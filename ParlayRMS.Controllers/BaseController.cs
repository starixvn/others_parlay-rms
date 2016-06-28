namespace ParlayRMS.Controllers
{
    using ONEbook.UM.Filters;
    using ParlayRMS.DTO;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.Web.Context;
    using ParlayRMS.Web.Session;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;

    /// <summary>
    /// Base controller for controller to inherit.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        /// <summary>
        /// Context of current logging in session.
        /// </summary>
        private readonly ICurrentContext _currentContext = new CurrentContext();

        /// <summary>
        /// Session of logging in user.
        /// </summary>
        private readonly ICurrentSession _currentSession = new CurrentSession();

        public ICurrentContext CurrentContext
        {
            get
            {
                return this._currentContext;
            }
        }

        public ICurrentSession CurrentSession
        {
            get
            {
                return this._currentSession;
            }
        }

        /// <summary>
        /// Gets sport type by id.
        /// </summary>
        /// <param name="name">Sport name.</param>
        /// <returns>Sport type id.</returns>
        public int GetSportTypeIdByName(string name)
        {
            int sport = 0;
            if (name == ViewLang.Allsports)
            {
                sport = 0;
            }
            else if (name == ViewLang.Socceronly)
            {
                sport = 1;
            }
            else if (name == ViewLang.Othersports)
            {
                sport = 2;
            }
            else
            {
                sport = 3;
            }

            return sport;
        }

        /// <summary>
        /// Hides the group.
        /// </summary>
        /// <param name="hiddenGroupNameList">The hidden group name list.</param>
        /// <returns>Whether the current user belongs to a hidden group or not.</returns>
        public bool HideGroup(out string[] hiddenGroupNameList)
        {
            var usersService = new UsersService();
            var user = usersService.GetUserById(CurrentSession.UserId);
            var groupName = user.Group.Name == null ? string.Empty : user.Group.Name;
            hiddenGroupNameList = AppSetting.HiddenGroups;
            if (hiddenGroupNameList.Contains<string>(groupName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts a system message to JSON result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>JSON result contains the message and code.</returns>
        protected JsonResult Json(Result result)
        {
            return base.Json(new { code = result.Code, message = result.Message });
        }

        /// <summary>
        /// Creates a JSON result indicates the invalid model error.
        /// </summary>
        /// <returns>Whether the passed model via AJAX is valid or not.</returns>
        protected JsonResult InvalidModelJson()
        {
            return this.Json(new { code = -1, message = this.GetModelErrorMessage() });
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
        }

        /// <summary>
        /// Gets the model error message.
        /// </summary>
        /// <returns>The first error message.</returns>
        protected string GetModelErrorMessage()
        {
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    return error.ErrorMessage;
                }
            }

            return "Undefined Error";
        }

        /// <summary>
        /// Gets the model error messages.
        /// </summary>
        /// <returns>List of error messages.</returns>
        protected string[] GetModelErrorMessages()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToArray();
        }

        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            try
            {
                this.View(actionName).ExecuteResult(this.ControllerContext);
            }
            catch
            {
                Response.Redirect("Error/AccessDenied", false);
            }
        }
    }
}