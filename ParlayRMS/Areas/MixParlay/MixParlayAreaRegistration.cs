using System.Web.Mvc;
using System.Web.Optimization;

namespace ParlayRMS.Areas.MixParlay
{
    public class MixParlayAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MixParlay";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterRoutes(context);
            RegisterBundles(BundleTable.Bundles);
        }

        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MixParlay_default",
                "MixParlay/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] {
                    "ParlayRMS.MixParlay.BetList",
                    "ParlayRMS.MixParlay.MixParlayMonitor",
                    "ParlayRMS.MixParlay.PotentialWinningMonitor",
                }
            );
        }

        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/mix-parlay/potential-winning-monitor").Include(
                    "~/Areas/MixParlay/Scripts/potential-winning-monitor.js"));
            bundles.Add(new ScriptBundle("~/mix-parlay/mix-parlay-monitor").Include(
                    "~/Areas/MixParlay/Scripts/mix-parlay-monitor.js"));
        }
    }
}