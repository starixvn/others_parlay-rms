using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using ParlayRMS.DTO.BetList;
using ParlayRMS.ViewModel.BetList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using Criteria.BetList;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using Services;

    public class BetListController : BaseController
    {
        private readonly BetListService _betlistService = new BetListService();
        private readonly CommonService _commonService = new CommonService();

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ParlayMonitor, Policy = Constant.Policy.Read)]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.TransId = options.TransId;
            ViewBag.Username = options.Username;

            var betTypeBelongsBetRadar = _commonService.GetBettypeBelongsBetRadar();

            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _betlistService.CreateChoice(options.TransId, options.RefNo);

            var betInfo = _betlistService.GetBetInfo(options.TransId);

            for (var i = 0; i < tickets.Item2.Count; i++)
            {
                var blackListLeague = tickets.Item1[i] ? "blackListClass blackListMargin" : string.Empty;
                var ticket = tickets.Item2[i];
                var usedClass = betTypeBelongsBetRadar.Contains(ticket.BetTypeId) ? "ticketDetail betRadar" : "ticketDetail";
                var toolTip = betTypeBelongsBetRadar.Contains(ticket.BetTypeId) ? "Betradar" : string.Empty;
                if (tickets.Item2.Last() == ticket)
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div  class='" + blackListLeague + "'></div><div class='" + usedClass + "' title='" + toolTip + "'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div>", false);
                }
                else
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id = '" + ticket.BetId + "'><div  class='" + blackListLeague + "'></div><div class='" + usedClass + "' title='" + toolTip + "'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div><div class='line'></div>", false);
                }
            }

            var comboTypeAndStake = GetComboTypeAndStake(tickets.Item2, betInfo);

            var model = new BetDetails
            {
                TicketDetail = builder.GetBetList(),
                ComboTypes = comboTypeAndStake
            };
            return View(model);
        }

        private static List<string> GetComboTypeAndStake(IEnumerable<ITicket> tickets, RMS_Monitor_Parlay_BetInfo betInfo)
        {
            if (betInfo == null)
            {
                return new List<string>();
            }    
            var comboTypes = Utility.CacheRef.GetCombinationTypeCombo(betInfo.BetTeam, betInfo.BetCheck, betInfo.Hdp1, betInfo.Hdp2);
            var comboTypesAndStake = new List<string>();
            var hasStaketickets = tickets.Where(m => m.Stake > 0).OrderBy(m => m.MatchId).ToList();
            var count = Math.Min(comboTypes?.Length ?? 0, hasStaketickets.Count);
            for (var i = 0; i < count; i++)
            {
                comboTypesAndStake.Add(comboTypes[i] + " - " + FormatStake(hasStaketickets[i].Stake));
            }

            return comboTypesAndStake;
        }

        private static string FormatStake(decimal stake)
        {
            return stake < 10
                ? $"{stake:#,0.00}"
                : $"{Math.Floor(stake):#,0}";
        }

        [HttpPost]
        public ActionResult SingleMatchParlay(BetList_BetBuilder_DetailCriteria criteria)
        {
            var result = new StringBuilder();
            var selections = _betlistService.RMS_BetList_BetBuilder_Detail(criteria);

            if (selections.Any())
            {
                var selectionIds = selections.Select(s => s.SelectionType).ToArray();
                var selectionNames = Utility.CacheRef.RemotingRefService.GetBetBuilderSelectionNames(selectionIds, "en-US");

                for (var i = 0; i < selectionNames.Length; i++)
                {
                    result.AppendFormat("<div class=\"match\">{0} - <span style=\"color:blue\">{1}</span></div>\r\n", selectionNames[i], selections[i].Result);
                }
            }

            var content = $"<div style=\"margin-right:10px; padding: 2px 5px; border-right: 3px solid black\">{result}</div> ";
            return Content(content);
        }


        [HttpPost]
        public ActionResult SingleMatchParlayDetailPopup(BetList_BetBuilder_DetailCriteria criteria)
        {
            ViewBag.TransId = criteria.TransId;
            ViewBag.Username = criteria.Username;

            var selections = _betlistService.RMS_BetList_BetBuilder_Detail(criteria);
            var selectionNames = new List<string>();
            var resultTexts = new List<string>();
            if (selections.Any())
            {
                foreach (var s in selections)
                {
                    var name = Utility.CacheRef.RemotingRefService.GetBetBuilderSelectionNames(new[] { s.SelectionType }, "en-US");
                    selectionNames.Add(name.FirstOrDefault());
                    resultTexts.Add(s.Result);
                }
            }

            var model = _betlistService.RMS_BetList_BetBuilder_MatchDetail(criteria.TransId).FirstOrDefault();
            var choice = GetChoiceForSingleMatchParlayDetailPopup(model);

            ViewBag.ResultTexts = resultTexts;
            ViewBag.SelectionNames = selectionNames;
            ViewBag.Choice = choice;
            ViewBag.IsBlackListLeague = model?.IsBlacklistLeague ?? false;

            return View();
        }

        [HttpPost]
        public ActionResult PenaltyShootoutParlayDetailPopup(BetList_PenaltyShootout_DetailCriteria criteria)
        {
            ViewBag.TransId = criteria.TransId;
            ViewBag.Username = criteria.Username;
            var model = _betlistService.RMS_BetList_PenaltyShootout_MatchDetail(criteria.TransId).FirstOrDefault();
            var choice = GetChoiceForPenaltyShootoutParlayDetailPopup(model);
            ViewBag.Choice = choice;
            ViewBag.IsBlackListLeague = model?.IsBlacklistLeague ?? false;

            return View();
        }


        private string GetChoiceForSingleMatchParlayDetailPopup(RMS_BetList_BetBuilder_MatchDetailModel model)
        {
            if (model == null)
            {
                return string.Empty;
            }

            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = false };
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            var ticket = new Ticket
            {
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                SportTypeId = model.SportType,
                LeagueId = model.LeagueId,
                EventDate = model.EventDate,
                TransId = model.TransId,
                BetTypeId = (int)Models.BetType.SingleMatchParlay
            };

            var choice = builder.BuildChoice(ticket, ticketHelper, null, false);
            var html = new HtmlDocument();
            html.LoadHtml(choice);
            var document = html.DocumentNode;
            var matchNode = document.QuerySelector(".match");
            var leagueNode = document.QuerySelector(".league");
            var eventDateNode = document.QuerySelector(".event-date");
            eventDateNode?.Remove();

            document.RemoveAllChildren();

            if (matchNode != null)
            {
                document.ChildNodes.Add(matchNode);
            }

            if (leagueNode != null)
            {
                document.ChildNodes.Add(leagueNode);
            }

            return document.OuterHtml;
        }

        private string GetChoiceForPenaltyShootoutParlayDetailPopup(RMS_BetList_PenaltyShootout_MatchDetailModel model)
        {
            if (model == null)
            {
                return string.Empty;
            }

            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = false };
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            var ticket = new Ticket
            {
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                SportTypeId = model.SportType,
                LeagueId = model.LeagueId,
                EventDate = model.EventDate,
                TransId = model.TransId,
                BetTypeId = 376,
                BetCheck = model.BetCheck,
                BetTeam = model.BetTeam
            };

            var choice = builder.BuildChoice(ticket, ticketHelper, null, false);
            var html = new HtmlDocument();
            html.LoadHtml(choice);
            var document = html.DocumentNode;
            var bettype = document.QuerySelector(".bettype");
            var eventDateNode = document.QuerySelector(".event-date");
            eventDateNode?.Remove();

            bettype?.Remove();

            return document.OuterHtml;
        }
    }
}
