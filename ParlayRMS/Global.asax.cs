// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS
{
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Web.Session;
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;

    /// <summary>
    /// MVC Application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Runs when the application is starting.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterBundles();

            DbSettingProviderManager.Current.Start();
        }

        /// <summary>
        /// Handles authentication request event.
        /// </summary>
        /// <param name="sender">Object which causes the event.</param>
        /// <param name="e">Event argument.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity
                    && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var cookie = FormsAuthentication.FormsCookieName;
                    var httpCookie = Context.Request.Cookies[cookie];

                    if (httpCookie == null)
                    {
                        return;
                    }

                    var ticket = FormsAuthentication.Decrypt(httpCookie.Value);
                    if (ticket == null || ticket.Expired)
                    {
                        return;
                    }

                    var identity = new FormsIdentity(ticket);
                    if (string.IsNullOrEmpty(ticket.UserData))
                    {
                        return;
                    }

                    var udata = AuthInfo.CreateUserData(ticket.UserData);
                    var principal = new Principal(identity, udata);
                    Context.User = principal;
                }
            }
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError().GetBaseException();
            var httpException = exception as HttpException;
            if (httpException != null || Request.IsLocal)
            {
                return;
            }

            // Get base exception data
            var message = exception.Message +
                "\r\nType: " + exception.GetType().ToString() +
                "\r\nSource: " + exception.Source +
                "\r\nUsername: " + CurrentSession.Instance.UserName +
                "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                "\r\nForm: " + Request.Form.ToString() +
                "\r\nQueryString: " + Request.QueryString.ToString() +
                "\r\nTargetSite: " + exception.TargetSite +
                "\r\nStackTrace: " + exception.StackTrace;

            // Get additional exception data
            if (exception is SqlException)
            {
                message += "\r\nProcedure: " + ((SqlException)exception).Procedure +
                    "\r\nLine Number: " + ((SqlException)exception).LineNumber +
                    "\r\nServer: " + ((SqlException)exception).Server;
            }

            LogService.Error(message);
        }

        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The global filter collection.</param>
        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The route collection.</param>
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = System.Web.Http.RouteParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        /// <summary>
        /// Registers the bundles.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        private static void RegisterBundles()
        {
            var bundles = BundleTable.Bundles;

            bundles.Add(new StyleBundle("~/Content/Libs/jquery-ui/themes/base/css").Include(
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.accordion.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.autocomplete.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.button.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.core.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.datepicker.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.dialog.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.menu.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.progressbar.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.resizable.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.selectable.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.slider.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.spinner.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.tabs.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.theme.css",
                "~/Content/Libs/jquery-ui/themes/base/jquery.ui.tooltip.css"));
            bundles.Add(new StyleBundle("~/Content/Libs/css").Include(
                "~/Content/Libs/jquery-ui/jquery-ui.more.css",
                "~/Content/Libs/contextmenu/jquery.contextmenu.css",
                "~/Scripts/Libs/multiselect/jquery.multiselect.css",
                "~/Scripts/Libs/multiselect/jquery.multiselect.filter.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/BetList.css",
                "~/Content/Components.css",
                "~/Content/confirm.css",
                "~/Content/Icon.css",
                "~/Content/modalPopup.css",
                "~/Content/Paging.css",
                "~/Content/Site.css",
                "~/Content/Validators.css"));
            bundles.Add(new StyleBundle("~/Content/Login/css").Include(
                "~/Content/Login.css"));
            bundles.Add(new StyleBundle("~/Content/Site/css").Include(
                "~/Content/Site.css"));
            bundles.Add(new StyleBundle("~/Content/Validators/css").Include(
                "~/Content/Validators.css"));

            // JS
            bundles.Add(new ScriptBundle("~/Scripts/Libs/js").Include(
                "~/Scripts/Libs/bettertooltip/jquery.betterTooltip.js",
                "~/Scripts/Libs/contextmenu/jquery.contextMenu.js",
                "~/Scripts/Libs/cookie/jquery.cookie.js",
                "~/Scripts/Libs/highcharts/highcharts.js",
                "~/Scripts/Libs/jquery/jquery.js",
                "~/Scripts/Libs/jquery-ui/jquery-ui.custom.js",
                "~/Scripts/Libs/jquery-ui/jquery-ui.more.js",
                "~/Scripts/Libs/moment/moment.js",
                "~/Scripts/Libs/multiselect/jquery.multiselect.js",
                "~/Scripts/Libs/multiselect/jquery.multiselect.filter.js",
                "~/Scripts/Libs/numeral/numeral.js",
                "~/Scripts/Libs/simplemodal/jquery.simplemodal.js",
                "~/Scripts/Libs/sortElements/jquery.sortElements.js",
                "~/Scripts/Libs/soundManager/soundmanager2-nodebug.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Core/js").Include(
                "~/Scripts/Core.js",
                "~/Scripts/Components.js",
                "~/Scripts/Confirm.js",
                "~/Scripts/Popup.js",
                "~/Scripts/Menu.js",
                "~/Scripts/Validators.js",
                "~/Scripts/UpdateLevel.js"));

            // group all scripts written in namespace
            bundles.Add(new ScriptBundle("~/Scripts/TotalOutstanding/js").Include(
                "~/Scripts/ParlayRMS.TotalOutstanding.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Report/TopWinners/js").Include("~/Scripts/Report/TopWinners.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Report/Winloss/js").Include("~/Scripts/Report/ParlayRMS.Winloss.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Report/MemberWinLoss/js").Include("~/Scripts/Report/MemberWinLoss.js"));

            // chart
            bundles.Add(new ScriptBundle("~/Scripts/Data/js").Include("~/Scripts/Data.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Tabular/js").Include("~/Scripts/Tabular.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Chart/js").Include("~/Scripts/Chart.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Cache/js").Include("~/Scripts/Cache.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Monitoring/js").Include("~/Scripts/Monitoring.js"));

            // report
            bundles.Add(new ScriptBundle("~/Scripts/Member/Core/js").Include("~/Scripts/Member/Core.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/SimilarBets/js").Include("~/Scripts/Report/SimilarBets.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/ViewIPInfo/js").Include("~/Scripts/Report/ViewIPInfo.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/ViewIPInfoByUser/js").Include("~/Scripts/Report/ViewIPInfoByUser.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ManageUser/js").Include("~/Scripts/ManageUser.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageMember/js").Include("~/Scripts/ManageMember.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageAgent/js").Include("~/Scripts/ManageAgent.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageGroup/js").Include("~/Scripts/ManageGroup.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageMenu/js").Include("~/Scripts/ManageMenu.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageFunction/js").Include("~/Scripts/ManageFunction.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Monitor/MixParlay/js").Include("~/Scripts/Monitor/MixParlay.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Monitor/PotentialWinning/js").Include("~/Scripts/Monitor/PotentialWinning.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Monitor/SystemParlay/js").Include("~/Scripts/Monitor/SystemParlay.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Monitor/DangerMonitor/js").Include("~/Scripts/Monitor/DangerMonitor.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Pager/js").Include("~/Scripts/Pager.js"));

            bundles.Add(new ScriptBundle("~/Scripts/UnderMaintenance/js").Include("~/Scripts/UnderMaintenance.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Login/js").Include("~/Scripts/Login.js"));
        }
    }
}