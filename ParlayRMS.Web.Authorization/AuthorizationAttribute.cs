// --------------------------------------------------------------------------
// <copyright file="AuthorizationAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Authorization
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using ParlayRMS.Services.Implement.Systems;
    using ParlayRMS.Web.Context;
    using ParlayRMS.Web.Session;

    /// <summary>
    /// Authorization Attribute.
    /// </summary>
    public class AuthorizationAttribute : AuthorizeAttribute, IDisposable
    {
        private readonly CachingService _cachingService = new CachingService();
        private bool _disposed = false;

        public string Function { get; set; }

        public UserPolicy Policy { get; set; }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            ICurrentSession currentSession = new CurrentSession();
            ICurrentContext currentContext = new CurrentContext();
         
            ////string actionName = filterContext.ActionDescriptor.ActionName;
            ////string controllerName = filterContext.Controller.ToString();

            try
            {
                if (filterContext.HttpContext == null)
                {
                    ////string msg5 = "msg5 filterContext.HttpContext == null";
                    ////WriteLog(msg5);
                    CloseConnection(filterContext);
                    return;
                }

                var principal = filterContext.HttpContext.User;
                
                if (ReferenceEquals(principal, null) || ReferenceEquals(principal.Identity.AuthenticationType, null))
                {
                    ////string msg6 = string.Format("msg6 userId={0}, controllerName={1}, actionName={2}", currentSession.UserId, controllerName, actionName);
                    ////WriteLog(msg6);
                    CloseConnection(filterContext);
                    return;
                }

                if (!FunctionAuthorization(currentSession.UserId, this.Function))
                {
                    ////string msg7 = string.Format("msg7 userId={0}, controllerName={1}, actionName={2}", currentSession.UserId, controllerName, actionName);
                    ////WriteLog(msg7);
                    CloseConnection(filterContext);
                    return;
                }

                    ////string msg8 = string.Format("msg8 userId={0}, controllerName={1}, actionName={2}", currentSession.UserId, controllerName, actionName);
                    ////WriteLog(msg8);
            }
            catch (Exception)
            {
                throw; ////ex;
            }
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    (this._cachingService as CachingService).Dispose();
                }
            }

            this._disposed = true;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
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
                    response.Redirect("~/home/index", false);
                    ////TODO: Ken - try new redirect
                    ////return new RedirectResult("~/home/index",false);
                }
            }
            catch (Exception)
            {
                throw; ////ex;
            }
        }

        /// <summary>
        /// Signs the user out and redirects to login page.
        /// </summary>
        private void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        /// <summary>
        /// Whether the user has permission on a function or not.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="functionKey">The function key.</param>
        /// <returns>Indicates whether the user has permission on a function or not.</returns>
        private bool FunctionAuthorization(int userId, string functionKey)
        {
            if (userId <= 0)
            {
                return false;
            }

            var userCache = _cachingService.GetUserCache(userId);
            var policyList = userCache.Functions;
            var foundItems = policyList.Where(x => x.FunctionKey == functionKey);
            ////WriteLog(string.Format("userId:{0}, functionId:{1}, policyList.Count()={2}, foundItems.Count()={3}", 
            ////userId, functionId, policyList.Count(), foundItems.Count()));
            if (foundItems.Count() > 0)
            {
                var firstItem = foundItems.First();
                switch (this.Policy)
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

        ////private void WriteLog(string msg)
        ////{
        ////    LogService.Debug(msg, "PARLAY_RMS");
        ////}
    }
}