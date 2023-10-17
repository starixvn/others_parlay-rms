using ParlayRMS.Models;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Monitoring;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using Services;

    [NapAuth(FunctionKey = FunctionKeys.PotentialWinningMonitor, Policy = Constant.Policy.Read)]
    public class PotentialWinningController : BaseController
    {
        private readonly PotentialWinningMonitorService _potentialWinning = new PotentialWinningMonitorService();
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
                    Selected = true
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.TenUp, Value = "10", Selected = true });
            ViewBag.Combos = combos;

            var parlayTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() },
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

            var values = AppSetting.SelectTop;
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item, Value = item, Selected = item == "100" });
            }

            ViewBag.SelectTops = selectTops;
            ViewBag.WinningAmount = 20000;

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

            var data = _potentialWinning.GetPotentialWinningList(model.Combos, model.ParlayTypes, model.SportsTypes, model.Levels, model.WinningAmount, model.SelectTop, model.IsCornerParlay, model.FilterBy, model.AgentId, model.CustomerClass, model.Company, model.UserName);

            return Json(new
            {
                Code = 0,
                Records = data
            });
        }

        [HttpPost]
        public ActionResult ViewDetailList(PotentialWinningDetailModel detailModel)
        {
            var model = _potentialWinning.GetPotentialWinningDetailList(detailModel.Combos, detailModel.ParlayTypes, detailModel.SportsTypes, detailModel.CustId, detailModel.IsCornerParlay);
            ViewBag.CustId = detailModel.CustId;
            ViewBag.BetDetailUserName = detailModel.UserName;
            ViewBag.IsDislayCheckedBy = !detailModel.AgentId.HasValue;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CheckBy(UserTrackingCriteria criteria)
        {
            var data = _potentialWinning.UpdateCheckedUser(criteria.UserId, criteria.Username, criteria.TransId);
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
