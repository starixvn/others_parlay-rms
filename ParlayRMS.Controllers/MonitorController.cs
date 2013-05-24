// -----------------------------------------------------------------------
// <copyright file="MixParlay.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Services.Implement.MixParlayMonitor;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Interface.MixParlayMonitor;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.MixPalayMonitor;
    using ParlayRMS.ViewModel.Monitoring;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Models.Functions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonitorController : BaseController
    {
        private readonly IBetListService betlistService = new BetListService();
        private readonly IMixParlayMonitorService mixparlayMonitorService = new MixParlayMonitorService();
        private readonly IMixParlayMonitorService mixparlayMonitorCacheService = new MixParlayMonitorCacheService();

        #region Mix Parlay

        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = ePolicy.Read)]
        public ActionResult MixParlayFilter()
        {
            // Combo
            var combos = new List<SelectListItem>();
            for (int i = 2; i <= 5; i++)
            {
                combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
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
            var values = new List<int> { 10, 20, 50, 100 };
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString(), Selected = item == 20 ? true : false });
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

            return this.PartialView("MixParlay/Filter");
        }

        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = ePolicy.Read)]
        public ActionResult MixParlay()
        {
            return this.View("MixParlay/Index");
        }

        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = ePolicy.Read)]
        public ActionResult MixParlayData(DataModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var service = GetMixParlayService();
            var data = service.GetMixParlayMonitorTickets(model.Combos, model.SportsTypes, model.Levels, model.MaxPayout, model.SelectTop, model.MaxTransId);
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
                var sportsTypes = model.SportsTypes.Split(',');
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

                title = string.Format("Mix Parlay Monitor - {0} [{1}] - {2} [{3}] - {4} [{5}] - {6} [{7}]",
                    ViewLang.Combo,
                    combosTitle,
                    ViewLang.SportsType,
                    sportsTypesTitle,
                    ViewLang.Level,
                    levelsTitle,
                    ViewLang.SelectTop,
                    model.SelectTop);

                if (model.MaxPayout > 0)
                {
                    title += string.Format(" - {0} [{1}]", ViewLang.MaxPayout, Formatter.FormatNumber2(model.MaxPayout));
                }
            }

            return this.Json(new
            {
                code = 0,
                Records = data,
                Title = title,
                MaxTransId = maxTransId
            });
        }

        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = ePolicy.Read)]
        public ActionResult MixParlayBetDetails(MixParlayBetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper();
            var tickets = this.betlistService.CreateChoice(options.RefNo);
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

            return View("MixParlay/BetDetails", model);
        }

        private IMixParlayMonitorService GetMixParlayService()
        {
            return mixparlayMonitorCacheService;
            //if (AppSetting.IsCache)
            //{
            //    return mixparlayCacheService;
            //}

            //return mixparlayService;
        }

        #endregion Mix Parlay
    }
}