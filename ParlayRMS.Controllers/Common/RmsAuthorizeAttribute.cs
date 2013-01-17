namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Web.Authorization;

    public class RmsAuthorizeAttribute : AuthorizeAttribute
    {
        private static readonly IUserService userService = new UserService();
        private static readonly ICachingService cachingService = new CachingService();

        private ICurrentSession currentSession;
        private ICurrentContext currentContext;

        public RmsAuthorizeAttribute()
        {
            this.currentSession = CurrentSession.Instance;
            this.currentContext = CurrentContext.Instance;
        }

        public eFunctions Function { get; set; }

        public RmsPolicy Policy { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);

            if (filterContext.HttpContext == null)
            {
                this.CloseConnection(filterContext);
                return;
            }

            if (this.HasFunctionAuthorization(this.currentSession.UserId, (int)this.Function))
            {
            }
            else
            {
                this.CloseConnection(filterContext);
            }
        }

        public bool HasFunctionAuthorization(int userId, int functionId)
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
                    case RmsPolicy.Read:
                        if (firstItem.Policy.Read)
                        {
                            return true;
                        }

                        break;
                    case RmsPolicy.Create:
                    case RmsPolicy.Delete:
                    case RmsPolicy.Update:
                        if (firstItem.Policy.Write)
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        private void CloseConnection(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult();

            if (filterContext == null ||
                filterContext.HttpContext == null ||
                filterContext.HttpContext.Request == null)
            {
                this.SignOut(filterContext);
            }

            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.StatusCode = 401;
                response.ContentType = "application/json";
                response.Write("{\"code\":401, \"message\":\"You don't have permission\"}");
                response.Flush();
                response.Close();
                response.End();
            }
            else if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                this.SignOut(filterContext);
            }
            else
            {
                response.Redirect("~/Shared/Unauthorized");
            }
        }

        private void SignOut(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;
            int onUserId = this.currentSession.OnUserId;
            if (onUserId > 0)
            {
                userService.Logout(onUserId, this.currentSession.UserName);
                this.currentContext.Context.Session.Abandon();
                response.Redirect("~/Account/Login");
            }
            else
            {
                this.currentContext.Context.Session.Abandon();
            }

            response.Redirect("~/Account/Login");
        }
    }
}