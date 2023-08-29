using ParlayRMS.Services.Systems;
using ParlayRMS.Web.Session;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ParlayRMS.Web.Authorization
{
    public class AuthorizationAttribute : AuthorizeAttribute, IDisposable
    {
        private readonly CachingService _cachingService = new CachingService();
        private bool _disposed;

        public string Function { get; set; }

        public UserPolicy Policy { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            ICurrentSession currentSession = new CurrentSession();

            if (filterContext.HttpContext == null)
            {
                CloseConnection(filterContext);
                return;
            }

            var principal = filterContext.HttpContext.User;

            if (ReferenceEquals(principal, null) || ReferenceEquals(principal.Identity.AuthenticationType, null))
            {
                CloseConnection(filterContext);
                return;
            }

            if (!FunctionAuthorization(currentSession.UserId, Function))
            {
                CloseConnection(filterContext);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cachingService.Dispose();
                }
            }

            _disposed = true;
        }

        private void CloseConnection(AuthorizationContext filterContext)
        {
            if (filterContext == null
                || filterContext.HttpContext == null
                || filterContext.HttpContext.Request == null)
            {
                SignOut();
                return;
            }

            var response = filterContext.HttpContext.Response;

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
                response.Redirect("~/home/index", false);
            }
        }

        private void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        private bool FunctionAuthorization(int userId, string functionKey)
        {
            if (userId <= 0)
            {
                return false;
            }

            var userCache = _cachingService.GetUserCache(userId);
            var policyList = userCache.Functions;
            var foundItems = policyList.Where(x => x.FunctionKey == functionKey);
            if (foundItems.Any())
            {
                var firstItem = foundItems.First();
                switch (Policy)
                {
                    case UserPolicy.Read:
                        if (firstItem.Policy.Readable)
                        {
                            return true;
                        }

                        break;

                    case UserPolicy.Create:
                    case UserPolicy.Delete:
                    case UserPolicy.Update:
                        if (firstItem.Policy.Writable)
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
