namespace ParlayRMS.Controllers
{
    using DTO;
    using Services.Users;
    using Utility;
    using Web.Context;
    using Web.Session;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        private readonly CurrentContext _currentContext = new CurrentContext();

        private readonly CurrentSession _currentSession = new CurrentSession();

        public ICurrentContext CurrentContext => _currentContext;

        public ICurrentSession CurrentSession => _currentSession;

        public int GetSportTypeIdByName(string name)
        {
            var sport = 0;
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

        public bool HideGroup(out string[] hiddenGroupNameList)
        {
            var usersService = new UsersService();
            var user = usersService.GetUserById(CurrentSession.UserId);
            var groupName = user.Group.Name == null ? string.Empty : user.Group.Name;
            hiddenGroupNameList = AppSetting.HiddenGroups;
            if (hiddenGroupNameList.Contains(groupName))
            {
                return true;
            }

            return false;
        }

        protected JsonResult Json(Result result)
        {
            return base.Json(new { code = result.Code, message = result.Message });
        }

        protected JsonResult InvalidModelJson()
        {
            return Json(new { code = -1, message = GetModelErrorMessage() });
        }

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

        protected string GetModelErrorMessage()
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    return error.ErrorMessage;
                }
            }

            return "Undefined Error";
        }

        protected string[] GetModelErrorMessages()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToArray();
        }

        protected override void HandleUnknownAction(string actionName)
        {
            try
            {
                View(actionName).ExecuteResult(ControllerContext);
            }
            catch
            {
                Response.Redirect("Error/AccessDenied", false);
            }
        }
    }
}
