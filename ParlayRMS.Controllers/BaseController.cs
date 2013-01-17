// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.5.10">Check login and process view</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.Web.Context;
    using ParlayRMS.Web.Session;
    using System.Collections;
    using ParlayRMS.DTO;

    /// <summary>
    /// Base controller for controller to inherit
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        /// <summary>
        /// Context of current logging in session
        /// </summary>
        private readonly ICurrentContext currentContext = new CurrentContext();

        /// <summary>
        /// Session of logging in user
        /// </summary>
        private readonly ICurrentSession currentSession = new CurrentSession();

        /// <summary>
        /// Gets current context of this session
        /// </summary>
        public ICurrentContext CurrentContext
        {
            get
            {
                return this.currentContext;
            }
        }

        /// <summary>
        /// Gets current session
        /// </summary>
        public ICurrentSession CurrentSession
        {
            get
            {
                return this.currentSession;
            }
        }

        protected JsonResult Json(Result result)
        {
            return base.Json(new { code = result.Code, message = result.Message });
        }

        protected JsonResult InvalidModelJson()
        {
            return this.Json(new { code = -1, message = this.GetModelErrorMessage() });
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
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
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

        /// <summary>
        /// Gets Sport Type by id
        /// </summary>
        /// <param name="name">Sport name</param>
        /// <returns>Sport type id</returns>
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

        public bool HideGroup(out string[] hiddenGroupNameList)
        {
            var authorizationService = new AuthorizationService();
            hiddenGroupNameList = new string[0];
            var hiddenGroupNameListString = AppSetting.HiddenGroups;

            if (!string.IsNullOrEmpty(hiddenGroupNameListString))
            {
                hiddenGroupNameList = hiddenGroupNameListString.Split(';');

                var user = authorizationService.GetUserById(CurrentSession.UserId);
                var groupName = user.Group.Name == null ? string.Empty : user.Group.Name;

                if (hiddenGroupNameList.Contains<string>(groupName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}