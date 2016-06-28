using ParlayRMS.Controllers;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Interface.BetList;
using ParlayRMS.Utility;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace ParlayRMS.MixParlay.PotentialWinningMonitor
{
    [Authorization(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
    public class PotentialWinningMonitorController : BaseController
    {
        private readonly IService _service = new Service();
        private readonly IBetListService _betlistService = new BetListService();

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public ActionResult Filter()
        {
            // Combo
            var combos = new List<SelectListItem>();
            for (int i = 2; i <= 5; i++)
            {
                combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = true
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.SixUp, Value = "6", Selected = true });
            ViewBag.Combos = combos;

            // Sports Type
            var sportsTypes = new List<SelectListItem>();
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Socceronly, Value = ((int)SportsType.SoccerOnly).ToString() });
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Othersports, Value = ((int)SportsType.OtherSports).ToString() });
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Combinedsports, Value = ((int)SportsType.CombinedSports).ToString() });
            ViewBag.SportsTypes = sportsTypes;

            // Level
            var levels = new List<SelectListItem>();
            levels.Add(new SelectListItem { Text = ViewLang.None, Value = "0" });
            for (int i = 1; i <= 10; i++)
            {
                levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.Levels = levels;

            // Select Top
            var values = AppSetting.SelectTop;
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item, Value = item, Selected = item == "100" ? true : false });
            }

            ViewBag.SelectTops = selectTops;
            ViewBag.WinningAmount = 20000;

            // Refresh Time
            var refreshTime = new List<int> { 60, 120 };
            var refreshTimes = new List<SelectListItem>();
            foreach (var item in refreshTime)
            {
                refreshTimes.Add(new SelectListItem { Text = item.ToString(), Value = (item * 1000).ToString(), Selected = item == 60 ? true : false });
            }

            ViewBag.RefreshTimes = refreshTimes;
            return PartialView();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetData(ViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var data = _service.GetTickets(viewModel);
            return Json(new
            {
                Code = 0,
                Records = data,
            });
        }

        [HttpPost]
        public ActionResult Detail(DetailViewModel viewModel)
        {
            var model = _service.GetTicketDetail(viewModel);
            ViewBag.UserName = viewModel.UserName;
            return PartialView(model);
        }
    }
}