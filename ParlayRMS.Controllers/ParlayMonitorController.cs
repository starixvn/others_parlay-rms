using ParlayRMS.DTO.ParlayMonitor;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Common;
using ParlayRMS.Services.ParlayMonitor;
using ParlayRMS.Services.Users;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Monitoring;
using ParlayRMS.ViewModel.PalayMonitor;
using ParlayRMS.Web.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
    public class ParlayMonitorController : BaseController
    {
        private readonly ParlayMonitorCacheService _parlayMonitorCacheService = new ParlayMonitorCacheService();
        private readonly UsersService _userService = new UsersService();
        private readonly CommonService _commonService = new CommonService();

        public ActionResult Filter()
        {
            var combos = new List<SelectListItem>();
            for (var i = 2; i <= 9; i++)
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

            var parlayTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString(), Selected = true },
                new SelectListItem { Text = ViewLang.GodOfFortune, Value = ((int)ParlayType.GodOfFortune).ToString() },
                new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() },
                new SelectListItem { Text = ViewLang.PenaltyShootoutCombo, Value = ((int)ParlayType.PenaltyShootoutCombo).ToString() },
                new SelectListItem { Text = ViewLang.SingleMatchParlay, Value = ((int)ParlayType.SingleMatchParlay).ToString() }
            };
            ViewBag.ParlayTypes = parlayTypes;

            var levels = new List<SelectListItem> { new SelectListItem { Text = ViewLang.None, Value = "0" } };
            for (var i = 1; i <= 10; i++)
            {
                levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.Levels = levels;

            var values = new List<int> { 10, 20, 50, 100, 200, 1000, 2000, 3000 };
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString(), Selected = item == 200 });
            }

            ViewBag.SelectTops = selectTops;

            var markets = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.Today_only, Value = ((int)Market.TodayOnly).ToString(), Selected = true },
                new SelectListItem { Text = ViewLang.Early_only, Value = ((int)Market.EarlyOnly).ToString(), Selected = true },
                new SelectListItem { Text = ViewLang.Combined, Value = ((int)Market.Combined).ToString(), Selected = true }
            };
            ViewBag.Markets = markets;

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

        [HttpPost]
        public ActionResult Data(DataModel model)
        {
            ModelState.Remove("Odds");
            ModelState.Remove("CustomerClass");
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var isValidCustomerClass = !(model.CustomerClass.HasValue && model.CustomerClass.Value > short.MaxValue);

            var data = isValidCustomerClass ? _parlayMonitorCacheService.GetParlayMonitorTickets(
                model.Combos,
                model.ParlayTypes,
                model.SportsTypes,
                model.Levels,
                model.StakeAfterPT,
                model.NetWinAfterPT,
                model.ReportType,
                model.SelectTop,
                model.MaxTransId,
                model.Markets,
                model.IsCornerParlay,
                model.Odds,
                model.LeagueGroups,
                model.CustomerClass,
                model.Company,
                model.UserName) : new List<RMS_Monitor_Parlay_Betlist>();
            var maxTransId = model.MaxTransId;
            if (data.Any())
            {
                maxTransId = Math.Max(model.MaxTransId, long.Parse(data.First().TransId));
            }

            return Json(new
            {
                code = 0,
                Records = data,
                Title = string.Empty,
                MaxTransId = maxTransId.ToString()
            });
        }

        [HttpPost]
        public ActionResult CheckBy(UserTrackingCriteria criteria)
        {
            var data = _parlayMonitorCacheService.UpdateCheckedUser(criteria.UserId, criteria.Username, criteria.TransId);
            return Json(data);
        }

        [HttpPost]
        public ActionResult Company()
        {
            var data = _commonService.GetCompanies();
            return Json(data.GroupBy(x => x.IsDeposit).Select(y => new { GroupId = y.Key, Items = y.Select(z => z.Site) }).ToList());
        }
    }
}
