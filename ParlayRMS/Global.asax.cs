using Fanex.BetList.Core;
using Fanex.BetList.Core.RemotingRef;
using Fanex.NAP.Core.SignIn.Attribute;
using Fanex.RemotingRef;
using ONEbook.UM.AspNetMVC.Filters;
using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ParlayRMS
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RefDataTypeContainer
                .ConnectTo(new Uri(ConfigurationManager.AppSettings["Fanex.RemotingRef.Service.Url"]))
                .FromApplication("NAP-ParlayRMS")
                .RegisterSupportedLanguages("en-US");
            BetListManager.RegisterRemotingRef(new CacheRef());
            FanexLogging.Register();
            OnebookUMConfig.Register();
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterBundles();
            FanexData.Register();
        }

        
        protected void Application_Error(object sender, EventArgs e)
        {
            FanexLogging.HandleApplicationError(Server);
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckUMFilterAttribute());
            filters.Add(new SessionAuthorizeAttribute());
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ParlayRMS.Controllers" });
        }

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
                "~/Scripts/Libs/multiselect/jquery.multiselect-customized.css",
                "~/Scripts/Libs/multiselect/jquery.multiselect.filter.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/google-abel-font.css",
                "~/Content/BetList.css",
                "~/Content/Components.css",
                "~/Content/confirm.css",
                "~/Content/Icon.css",
                "~/Content/modalPopup.css",
                "~/Content/Paging.css",
                "~/Content/Site.css",
                "~/Content/Validators.css"));
            bundles.Add(new StyleBundle("~/Content/Menu").Include("~/Content/Menu.css"));
            bundles.Add(new StyleBundle("~/Content/Login").Include(
                "~/Content/Login.css"));
            bundles.Add(new StyleBundle("~/Content/Validators").Include(
                "~/Content/Validators.css"));
            bundles.Add(new StyleBundle("~/Content/simplepagination").Include("~/Content/Libs/simplepagination/simplePagination.css"));

            bundles.Add(new StyleBundle("~/Content/ParlayWon").Include(
              "~/Content/ParlayWon.css"));

            bundles.Add(new StyleBundle("~/Content/LeagueBetTypeAnalysis").Include(
              "~/Content/LeagueBetTypeAnalysis.css"));

            bundles.Add(new StyleBundle("~/Content/TopWinners").Include(
              "~/Content/TopWinners.css"));

            bundles.Add(new StyleBundle("~/Content/LeagueWinningMonitor").Include(
              "~/Content/LeagueWinningMonitor.css"));

            bundles.Add(new StyleBundle("~/Content/ViewIPInfo").Include(
              "~/Content/ViewIpInfo.css"));

            bundles.Add(new StyleBundle("~/Content/ParlayMonitor").Include(
                "~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.css",
                "~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.filter.css",
                "~/Content/ParlayMonitor.css"));

            bundles.Add(new StyleBundle("~/Content/PotentialWinning").Include(
                "~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.css",
                "~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.filter.css",
                "~/Content/PotentialWinning.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Libs/js").Include(
                     "~/Scripts/jquery-1.9.0.js",
                     "~/Scripts/Libs/bettertooltip/jquery.betterTooltip.js",
                     "~/Scripts/Libs/contextmenu/jquery.contextMenu.js",
                     "~/Scripts/Libs/cookie/jquery.cookie.js",
                     "~/Scripts/Libs/jquery-ui/jquery-ui.custom.js",
                     "~/Scripts/Libs/jquery-ui/jquery-ui.more.js",
                     "~/Scripts/autoNumeric/autoNumeric.js",
                     "~/Scripts/Libs/moment/moment.js",
                     "~/Scripts/Libs/multiselect/jquery.multiselect-customized.js",
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
            bundles.Add(new ScriptBundle("~/Scripts/Menu.LayoutAngular/js").Include("~/Scripts/jquery-1.9.0.js", "~/Scripts/Menu.LayoutAngular.js"));

            bundles.Add(new ScriptBundle("~/Scripts/TotalOutstanding/js").Include(
                "~/Scripts/ParlayRMS.TotalOutstanding.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Report/TopWinners/js").Include("~/Scripts/Report/TopWinners.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Report/Winloss/js").Include("~/Scripts/Report/ParlayRMS.Winloss.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Report/MemberWinLoss/js").Include("~/Scripts/Report/MemberWinLoss.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Member/Core/js").Include("~/Scripts/Member/Core.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/SimilarBets/js").Include("~/Scripts/Report/SimilarBets.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/ViewIPInfo/js").Include("~/Scripts/Report/ViewIPInfo.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Report/ViewIPInfoByUser/js").Include("~/Scripts/Report/ViewIPInfoByUser.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ManageUser/js").Include("~/Scripts/ManageUser.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageMember/js").Include("~/Scripts/ManageMember.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageLeague/js").Include("~/Scripts/ManageLeague.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageAgent/js").Include("~/Scripts/ManageAgent.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageGroup/js").Include("~/Scripts/ManageGroup.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageMenu/js").Include("~/Scripts/ManageMenu.js"));
            bundles.Add(new ScriptBundle("~/Scripts/ManageFunction/js").Include("~/Scripts/ManageFunction.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Monitor/Parlay/js").Include("~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.js",
                     "~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.filter.js",
                     "~/Scripts/Monitor/Parlay.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Monitor/PotentialWinning/js").Include("~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.js",
                    "~/Scripts/Libs/multiselect/jquery-ui-multiselect-widget-3.0.0/jquery.multiselect.filter.js",
                    "~/Scripts/Monitor/PotentialWinning.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Pager/js").Include("~/Scripts/Pager.js"));

            bundles.Add(new ScriptBundle("~/Scripts/UnderMaintenance/js").Include("~/Scripts/UnderMaintenance.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Login/js").Include("~/Scripts/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Report/ParlayWon/js").Include(
                "~/Scripts/Libs/simplepagination/jquery.simplePagination.js",
                "~/Scripts/Report/ParlayWon.js"
                 ));

            bundles.Add(new ScriptBundle("~/Scripts/Report/LeagueBetTypeAnalysis/js").Include(
                "~/Scripts/Libs/simplepagination/jquery.simplePagination.js",
                "~/Scripts/Report/LeagueBetTypeAnalysis.js"
                 ));

            bundles.Add(new ScriptBundle("~/Scripts/Monitor/LeagueWinning/js").Include("~/Scripts/Monitor/LeagueWinning.js"));
        }
    }
}
