// --------------------------------------------------------------------------
// <copyright file="LogActionFilter.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Mvc
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Log Action Filter.
    /// </summary>
    public class LogActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("OnActionExecuting", filterContext.RouteData);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
        }

        /// <summary>
        /// Logs the controller and action were called.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="routeData">The route data.</param>
        private void Log(string methodName, RouteData routeData)
        {
            #if DEBUG

            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = string.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);

            Debug.WriteLine(message, "Action Filter Log");
            #endif
        }
    }
}