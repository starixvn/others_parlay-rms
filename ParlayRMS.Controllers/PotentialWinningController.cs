// --------------------------------------------------------------------------
// <copyright file="PotentialWinningController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Controllers
{
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

    /// <summary>
    /// Monitor Controller.
    /// </summary>
    [AuthorizationAttribute(Function = FunctionKeys.MonitorPotentialWinning, Policy = UserPolicy.Read)]
    public class PotentialWinningController : BaseController
    {
        private readonly IPotentialWinningMonitor _potentialWinning = new PotentialWinningMonitorService();
        private readonly IBetListService _betlistService = new BetListService();

        #region Mix Parlay

        /// <summary>
        /// Initialize filter form.
        /// </summary>
        /// <returns>Partial view which represents the filter form.</returns> 
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
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

            // Parlay type
            var parlayTypes = new List<SelectListItem>();
            parlayTypes.Add(new SelectListItem { Text = ViewLang.MixparLay, Value = ((int)ParlayType.MixParlay).ToString() });
            parlayTypes.Add(new SelectListItem { Text = ViewLang.ComboParlay, Value = ((int)ParlayType.ComboParlay).ToString() });
            ViewBag.ParlayTypes = parlayTypes;

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
            return this.PartialView();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Gets mix parlay data.
        /// </summary>
        /// <param name="model">The model contains filter conditions which are used to query data.</param>
        /// <returns>JSON result contains queried data.</returns>        
        [HttpPost]
        public ActionResult GetData(PotentialWinningModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var data = _potentialWinning.GetPotentialWinningList(model.Combos, model.SportsTypes, model.Levels, model.WinningAmount, model.SelectTop);

            return this.Json(new
            {
                Code = 0,
                Records = data,
            });
        }

        /// <summary>
        /// Gets bet details.
        /// </summary>
        /// <param name="detailModel">The options that are used to query data.</param>
        /// <returns>
        /// Redirect to BetDetails view which will show the bet details.
        /// </returns>
        [HttpPost]
        public ActionResult ViewDetailList(PotentialWinningDetailModel detailModel)
        {
            var model = _potentialWinning.GetPotentialWinningDetailList(detailModel.Combos, detailModel.SportsTypes, detailModel.CustId);

            ViewBag.UserName = detailModel.UserName;

            return PartialView(model);
        }

        #endregion Mix Parlay
    }
}