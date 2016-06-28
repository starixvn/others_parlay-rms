using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Implement.ParlayMonitor;
using ParlayRMS.Services.Implement.Users;
using ParlayRMS.Services.Interface.BetList;
using ParlayRMS.Services.Interface.ParlayMonitor;
using ParlayRMS.Services.Interface.Users;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.PalayMonitor;
using ParlayRMS.Web.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
    public class ParlayMonitorController : BaseController
    {
        private readonly IBetListService _betlistService = new BetListService();
        private readonly IParlayMonitorService _parlayMonitorService = new ParlayMonitorService();
        private readonly IParlayMonitorService _parlayMonitorCacheService = new ParlayMonitorCacheService();
        private readonly IUsersService _userService = new UsersService();

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
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
                    Selected = i >= 2 && i <= 6
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.TenUp, Value = "10" });
            ViewBag.Combos = combos;

            // Parlay type
            var parlayTypes = new List<SelectListItem>();
            parlayTypes.Add(new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString(), Selected = true });
            parlayTypes.Add(new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() });
            ViewBag.ParlayTypes = parlayTypes;

            // Level
            var levels = new List<SelectListItem>();
            levels.Add(new SelectListItem { Text = ViewLang.None, Value = "0" });
            for (int i = 1; i <= 10; i++)
            {
                levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.Levels = levels;

            // Select Top
            var values = new List<int> { 10, 20, 50, 100, 200 };
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString(), Selected = item == 200 ? true : false });
            }

            ViewBag.SelectTops = selectTops;

            // Markets
            var markets = new List<SelectListItem>();
            markets.Add(new SelectListItem { Text = ViewLang.Today_only, Value = ((int)Market.TodayOnly).ToString(), Selected = true });
            markets.Add(new SelectListItem { Text = ViewLang.Early_only, Value = ((int)Market.EarlyOnly).ToString(), Selected = true });
            markets.Add(new SelectListItem { Text = ViewLang.Combined, Value = ((int)Market.Combined).ToString(), Selected = true });
            ViewBag.Markets = markets;

            // Refresh Time
            values = new List<int> { 60, 120 };
            var refreshTimes = new List<SelectListItem>();
            foreach (var item in values)
            {
                refreshTimes.Add(new SelectListItem { Text = item.ToString(), Value = (item * 1000).ToString(), Selected = item == 60 });
            }

            ViewBag.RefreshTimes = refreshTimes;
            ViewBag.StakeAfterPT = Formatter.FormatNumber3(AppSetting.MixParlayStakeAfterPT);
            ViewBag.NetWinStakeAfterPT = Formatter.FormatNumber3(AppSetting.NetWinStakeAfterPT);

            return PartialView();
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.HasReadPermission = _userService.HasReadPermission(CurrentSession.UserId, FunctionKeys.ViewIP);
            return View();
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [HttpPost]
        public ActionResult Data(DataModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var data = _parlayMonitorCacheService.GetParlayMonitorTickets(
                                model.Combos,
                                model.ParlayTypes,
                                model.SportsTypes,
                                model.Levels,
                                model.StakeAfterPT,
                                model.NetWinAfterPT,
                                model.ReportType,
                                model.SelectTop,
                                model.MaxTransId,
                                model.Markets);
            long maxTransId = model.MaxTransId;
            if (data.Count() > 0)
            {
                maxTransId = Math.Max(model.MaxTransId, data.First().TransId);
            }

            return Json(new
            {
                code = 0,
                Records = data,
                Title = string.Empty,
                MaxTransId = maxTransId
            });
        }
    }
}