using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.LeagueWinningMonitor;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel;
using ParlayRMS.ViewModel.LeagueWinningMonitor;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.LeagueWinningMonitor, Policy = UserPolicy.Read)]
    public class LeagueWinningMonitorController : BaseController
    {
        private readonly LeagueWinningMonitorService leagueWinningMonitor = new LeagueWinningMonitorService();

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Filter()
        {
            // Combo
            var combos = new List<SelectListItem>();
            for (int i = 2; i <= 9; i++)
            {
                combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i <= 6
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.TenUp, Value = "10" });
            ViewBag.Combos = combos;

            // Parlay type
            var parlayTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() },
                new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() }
            };
            ViewBag.ParlayTypes = parlayTypes;

            // Group
            var groups = new List<SelectListItem>();
            groups.AddRange(Config.LeagueGroups);
            groups.Add(new SelectListItem { Text = "Fixed Max Payout", Value = "99", Selected = true });
            ViewBag.Groups = groups;

            // Level
            var levels = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.None, Value = "0", Selected = true }
            };
            for (int i = 1; i <= 10; i++)
            {
                levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = true });
            }

            ViewBag.Levels = levels;

            // Select Top
            var values = AppSetting.SelectTop;
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item, Value = item, Selected = item == "200" });
            }
            selectTops.Add(new SelectListItem { Text = "2000", Value = "2000" });
            selectTops.Add(new SelectListItem { Text = "3000", Value = "3000" });
            ViewBag.SelectTops = selectTops;
            ViewBag.WinningAmount = 20000;

            // Refresh Time
            var refreshTime = new List<int> { 60, 120 };
            var refreshTimes = new List<SelectListItem>();
            foreach (var item in refreshTime)
            {
                refreshTimes.Add(new SelectListItem { Text = item.ToString(), Value = (item * 1000).ToString(), Selected = item == 60 });
            }

            ViewBag.RefreshTimes = refreshTimes;
            return PartialView();
        }

        [HttpPost]
        public ActionResult Data(LeagueWinningMonitorModel model)
        {
            ViewBag.ReportBy = model.ReportBy;
            var data = leagueWinningMonitor.GetLeagueWinningMonitorList(model.Combos, model.ParlayTypes, model.SportsTypes, model.Levels, model.WinningAmount, model.SelectTop, model.Group, model.ReportBy);

            return PartialView(data);
        }

        [HttpPost]
        public ActionResult ViewDetailList(LeagueWinningMonitorModel model)
        {
            ViewBag.Filter = model;
            var data = leagueWinningMonitor.GetLeagueWinningMonitorListDetail(model.Combos, model.ParlayTypes, model.SportsTypes, model.Levels, model.WinningAmount, model.SelectTop, model.Group, model.ReportBy, model.LeagueId, model.CustId);

            return PartialView(data);
        }
    }
}