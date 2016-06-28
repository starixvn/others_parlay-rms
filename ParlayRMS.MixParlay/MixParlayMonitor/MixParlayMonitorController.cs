using ParlayRMS.Controllers;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.Services.Implement.Users;
using ParlayRMS.Services.Interface;
using ParlayRMS.Services.Interface.Users;
using ParlayRMS.Utility;
using ParlayRMS.Web.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.MixParlay.MixParlayMonitor
{
    [Authorization(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
    public class MixParlayMonitorController : BaseController
    {
        private readonly IService _service = new Service();
        private readonly IUsersService _userService = new UsersService();
        private readonly ICommonService _commonService = new CommonService();

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
                    Selected = (i == 2) || (i == 3)
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.SixUp, Value = "6" });
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
            values = new List<int> { 30, 60, 120 };
            var refreshTimes = new List<SelectListItem>();
            foreach (var item in values)
            {
                refreshTimes.Add(new SelectListItem { Text = item.ToString(), Value = (item * 1000).ToString(), Selected = item == 60 ? true : false });
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
        public ActionResult Data(ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var data = _service.GetTickets(model);
            long maxTransId = model.MaxTransId;
            if (data.Count() > 0)
            {
                maxTransId = Math.Max(model.MaxTransId, data.First().TransId);
            }

            string title = string.Empty;
            if (model.MaxTransId == 0)
            {
                var combosTitle = ViewLang.All;
                var combos = model.Combos.Split(',');
                if (combos.Length < 5)
                {
                    combosTitle = string.Join(",", combos.Select(x => x == "6" ? ViewLang.SixUp : x));
                }

                var sportsTypesTitle = "All";
                var sportsTypes = model.SportTypes.Split(',');
                if (sportsTypes.Length < 3)
                {
                    var list = new List<string>();
                    foreach (var item in sportsTypes)
                    {
                        SportsType sportsType = (SportsType)Convert.ToInt32(item);
                        switch (sportsType)
                        {
                            case SportsType.SoccerOnly:
                                list.Add(ViewLang.Socceronly);
                                break;

                            case SportsType.OtherSports:
                                list.Add(ViewLang.Othersports);
                                break;

                            case SportsType.CombinedSports:
                                list.Add(ViewLang.Combinedsports);
                                break;

                            default:
                                list.Add(ViewLang.Undefined);
                                break;
                        }
                    }

                    sportsTypesTitle = string.Join(",", list);
                }

                var levelsTitle = ViewLang.All;
                var levels = model.Levels.Split(',');
                if (levels.Length < 11)
                {
                    levelsTitle = string.Join(",", levels.Select(x => x == "0" ? ViewLang.None : x));
                }

                title = string.Format(
                                   "Mix Parlay Monitor - {0} [{1}] - {2} [{3}] - {4} [{5}] - {6} [{7}]",
                                   ViewLang.Combo,
                                   combosTitle,
                                   ViewLang.SportsType,
                                   sportsTypesTitle,
                                   ViewLang.Level,
                                   levelsTitle,
                                   ViewLang.SelectTop,
                                   model.SelectTop);

                if (model.StakeAfterPT > 0)
                {
                    title += string.Format(" - {0} [{1}]", ViewLang.StakeAfterPT, Formatter.FormatNumber2(model.StakeAfterPT));
                }
            }

            return Json(new
            {
                code = 0,
                Records = data,
                Title = title,
                MaxTransId = maxTransId
            });
        }
    }
}