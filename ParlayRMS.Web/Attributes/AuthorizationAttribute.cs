using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ParlayRMS.Services.Implement;
using ParlayRMS.Services.Interface;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Web.Attributes
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        private static readonly UserService userService = new UserService();
        private static readonly ICachingService cachingService = new CachingService();
        private ICurrentSession currentSession;
        private ICurrentContext currentContext;

        public AuthorizationAttribute()
        {
            this.currentSession = CurrentSession.Instance;
            this.currentContext = CurrentContext.Instance;
        }

        public eFunctions Function { get; set; }

        public ePolicy Policy { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            try
            {
                if (filterContext.HttpContext == null)
                {
                    CloseConnection(filterContext);
                    return;
                }

                var principal = filterContext.HttpContext.User as Principal;

                if (ReferenceEquals(principal, null) || ReferenceEquals(principal.AuthInfo, null))
                {
                    CloseConnection(filterContext);
                    return;
                }

                if (!FunctionAuthorization(currentSession.UserId, (int)this.Function))
                {
                    CloseConnection(filterContext);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CloseConnection(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext == null
                    || filterContext.HttpContext == null
                    || filterContext.HttpContext.Request == null)
                {
                    SignOut();
                    return;
                }

                HttpResponseBase response = filterContext.HttpContext.Response;

                if (response == null)
                {
                    SignOut();
                    return;
                }

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    response.StatusCode = 403;
                    response.ContentType = "application/json";
                    response.Write("{\"code\":403, \"message\":\"You don't have permission\"}");
                    response.Flush();
                    response.End();
                }
                else if (!filterContext.HttpContext.Request.IsAuthenticated)
                {
                    SignOut();
                }
                else
                {
                    response.Redirect("~/home/index", true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        private bool FunctionAuthorization(int userId, int functionId)
        {
            if (userId <= 0)
            {
                return false;
            }

            var userCache = cachingService.GetUserCache(userId);
            var policyList = userCache.PolicyList;
            var foundItems = policyList.Where(x => x.FunctionId == functionId);
            if (foundItems.Count() > 0)
            {
                var firstItem = foundItems.First();
                switch (this.Policy)
                {
                    case ePolicy.Read:
                        if (firstItem.Policy.Read)
                        {
                            return true;
                        }

                        break;
                    case ePolicy.Create:
                    case ePolicy.Delete:
                    case ePolicy.Update:
                        if (firstItem.Policy.Write)
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }
    }
}