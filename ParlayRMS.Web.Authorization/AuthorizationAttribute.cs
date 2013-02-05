namespace ParlayRMS.Web.Authorization
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.Systems;
    using ParlayRMS.Web.Context;
    using ParlayRMS.Web.Session;
    using Fanex.Logging;

    public class AuthorizationAttribute : AuthorizeAttribute
    {
        private readonly CachingService cachingService = new CachingService();

        public eFunction Function { get; set; }

        public ePolicy Policy { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            ICurrentSession currentSession = new CurrentSession();
            ICurrentContext currentContext = new CurrentContext();
         
            //string actionName = filterContext.ActionDescriptor.ActionName;
            //string controllerName = filterContext.Controller.ToString();

            try
            {
                if (filterContext.HttpContext == null)
                {
                    //string msg5 = "msg5 filterContext.HttpContext == null";
                    //WriteLog(msg5);
                    CloseConnection(filterContext);
                    return;
                }

                var principal = filterContext.HttpContext.User;
                

                if (ReferenceEquals(principal, null) || ReferenceEquals(principal.Identity.AuthenticationType, null))
                {
                    //string msg6 = string.Format("msg6 userId={0}, controllerName={1}, actionName={2}", currentSession.UserId, controllerName, actionName);
                    //WriteLog(msg6);
                    CloseConnection(filterContext);
                    return;
                }

                if (!FunctionAuthorization(currentSession.UserId, (int)this.Function))
                {
                    //string msg7 = string.Format("msg7 userId={0}, controllerName={1}, actionName={2}", currentSession.UserId, controllerName, actionName);
                    //WriteLog(msg7);
                    CloseConnection(filterContext);
                    return;
                }

                //string msg8 = string.Format("msg8 userId={0}, controllerName={1}, actionName={2}", currentSession.UserId, controllerName, actionName);
                //WriteLog(msg8);
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
            var policyList = userCache.Functions;
            var foundItems = policyList.Where(x => x.FunctionId == functionId);
            //WriteLog(string.Format("userId:{0}, functionId:{1}, policyList.Count()={2}, foundItems.Count()={3}", userId, functionId, policyList.Count(), foundItems.Count()));
            if (foundItems.Count() > 0)
            {
                var firstItem = foundItems.First();
                switch (this.Policy)
                {
                    case ePolicy.Read:
                        if (firstItem.Policy.Readable)
                        {
                            return true;
                        }

                        break;

                    case ePolicy.Create:
                    case ePolicy.Delete:
                    case ePolicy.Update:
                        if (firstItem.Policy.Writable)
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        //private void WriteLog(string msg)
        //{
        //    LogService.Debug(msg, "PARLAY_RMS");
        //}
    }
}