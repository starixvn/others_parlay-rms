// --------------------------------------------------------------------------
// <copyright file="SystemParlayMonitorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.21">Created</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Services.Implement.SystemParlayMonitor;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Interface.SystemParlayMonitor;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Monitoring;
    using ParlayRMS.ViewModel.SystemParlayMonitor;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// System parlay monitor controller.
    /// </summary>
    public class SystemParlayMonitorController : BaseController
    {
        private readonly IBetListService _betlistService = new BetListService();
        private readonly ISystemParlayMonitorService _systemParlayMonitorService = new SystemParlayMonitorService();

        /// <summary>
        /// Initialize filter form.
        /// </summary>
        /// <returns>Partial view which represents the filter form.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
        public ActionResult SystemParlayFilter()
        {
            // Combo
            var comboNames = new string[] { ViewLang.Trixie, ViewLang.Yankee, ViewLang.Canadian, ViewLang.Heinz, ViewLang.SuperHeinz, ViewLang.Goliath };
            var combos = new List<SelectListItem>();
            string comboFormat = "{0} ({1})";
            for (int i = 3; i <= 8; i++)
            {
                combos.Add(new SelectListItem
                {
                    Text = string.Format("{0} ({1})", comboNames[i - 3], i),
                    Value = i.ToString(),
                    Selected = true
                });
            }

            ViewBag.Combos = combos;

            // Sports Type
            var sportTypes = new List<SelectListItem>();
            sportTypes.Add(new SelectListItem { Text = ViewLang.Socceronly, Value = ((int)SportsType.SoccerOnly).ToString() });
            sportTypes.Add(new SelectListItem { Text = ViewLang.Othersports, Value = ((int)SportsType.OtherSports).ToString() });
            sportTypes.Add(new SelectListItem { Text = ViewLang.Combinedsports, Value = ((int)SportsType.CombinedSports).ToString() });
            ViewBag.SportTypes = sportTypes;

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

            // Refresh Time
            values = new List<int> { 30, 60, 120 };
            var refreshTimes = new List<SelectListItem>();
            foreach (var item in values)
            {
                refreshTimes.Add(new SelectListItem { Text = item.ToString(), Value = (item * 1000).ToString(), Selected = item == 60 ? true : false });
            }

            ViewBag.RefreshTimes = refreshTimes;
            ViewBag.StakeAfterPT = Formatter.FormatNumber3(AppSetting.SystemParlayStakeAfterPT);

            return this.PartialView("Filter");
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            return this.View("Index");
        }

        /// <summary>
        /// Gets mix parlay data.
        /// </summary>
        /// <param name="model">The model contains filter conditions which are used to query data.</param>
        /// <returns>JSON result contains queried data.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
        public ActionResult SystemParlayData(SystemParlayMonitorOptions model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var data = _systemParlayMonitorService.GetSystemParlayMonitorData(
                            model.Combos,
                            model.SportTypes,
                            model.Levels,
                            model.StakeAfterPT,
                            model.MaxTransId,
                            model.SelectTop);
            long maxTransId = model.MaxTransId;
            string title = string.Empty;

            if (data.Count() > 0)
            {
                maxTransId = Math.Max(model.MaxTransId, data.First().TransId);
            }

            return this.Json(new
            {
                code = 0,
                Records = data,
                MaxTransId = maxTransId
            });
        }

        /// <summary>
        /// Gets bet details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Redirect to BetDetails view which will show the bet details.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
        public ActionResult SystemParlayBetDetails(MixParlayBetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = this._betlistService.CreateChoice(options.RefNo);
            foreach (var ticket in tickets)
            {
                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div>", false);
                }
                else
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div><div class='line'></div>", false);
                }
            }

            MixParlayBetDetails model = new MixParlayBetDetails();
            model.TicketDetail = builder.GetBetList();

            return View("BetDetails", model);
        }
    }
}