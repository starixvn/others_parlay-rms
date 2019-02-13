using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Implement.PotentialWinningMonitor;
using ParlayRMS.Services.Interface.BetList;
using ParlayRMS.Services.Interface.PotentialWinningMonitor;
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
        private readonly IPotentialWinningMonitor _potentialWinning = new PotentialWinningMonitorService();
        private readonly IBetListService _betlistService = new BetListService();

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
            parlayTypes.Add(new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() });
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
        public ActionResult GetData(PotentialWinningModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var data = _potentialWinning.GetPotentialWinningList(model.Combos, model.ParlayTypes, model.SportsTypes, model.Levels, model.WinningAmount, model.SelectTop);

            return Json(new
            {
                Code = 0,
                Records = data,
            });
        }

        [HttpPost]
        public ActionResult ViewDetailList(PotentialWinningDetailModel detailModel)
        {
            var model = _potentialWinning.GetPotentialWinningDetailList(detailModel.Combos, detailModel.ParlayTypes, detailModel.SportsTypes, detailModel.CustId);
            ViewBag.UserName = detailModel.UserName;
            return PartialView(model);
        }
    }
}