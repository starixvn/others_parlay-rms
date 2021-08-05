using ParlayRMS.DTO.PotentialWinning;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.PotentialWinningMonitor;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Monitoring;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.MonitorPotentialWinning, Policy = UserPolicy.Read)]
    public class PotentialWinningController : BaseController
    {
        private readonly PotentialWinningMonitorService _potentialWinning = new PotentialWinningMonitorService();

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
                    Selected = true
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.TenUp, Value = "10", Selected = true });
            ViewBag.Combos = combos;

            // Parlay type
            var parlayTypes = new List<SelectListItem>();
            parlayTypes.Add(new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() });
            parlayTypes.Add(new SelectListItem { Text = ViewLang.GodOfFortune, Value = ((int)ParlayType.GodOfFortune).ToString() });
            parlayTypes.Add(new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() });
            parlayTypes.Add(new SelectListItem { Text = ViewLang.PenaltyShootoutCombo, Value = ((int)ParlayType.PenaltyShootoutCombo).ToString() });
            parlayTypes.Add(new SelectListItem { Text = ViewLang.SingleMatchParlay, Value = ((int)ParlayType.SingleMatchParlay).ToString() });
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
            var values = AppSetting.SelectTop;
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item, Value = item, Selected = item == "100" });
            }

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

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetData(PotentialWinningModel model)
        {
            ModelState.Remove("CustomerClass");
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var data = _potentialWinning.GetPotentialWinningList(model.Combos, model.ParlayTypes, model.SportsTypes, model.Levels, model.WinningAmount, model.SelectTop, model.IsCornerParlay, model.FilterBy, model.AgentId, model.CustomerClass);

            return Json(new
            {
                Code = 0,
                Records = data,
            });
        }

        [HttpPost]
        public ActionResult ViewDetailList(PotentialWinningDetailModel detailModel)
        {
            var model = _potentialWinning.GetPotentialWinningDetailList(detailModel.Combos, detailModel.ParlayTypes, detailModel.SportsTypes, detailModel.CustId, detailModel.IsCornerParlay);
            ViewBag.UserName = detailModel.UserName;
            ViewBag.CustId = detailModel.CustId;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CheckBy(UserTrackingCriteria criteria)
        {
            //var data = _potentialWinning.UpdateCheckedUser(criteria.UserId, criteria.Username, criteria.TransId);
            var data = new UserTracking { Message = "abc", StatusId = 1 };
            return Json(data);
        }
    }
}