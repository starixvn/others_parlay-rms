using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.ViewModel.BetList;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Controllers
{
    public class BetListController : BaseController
    {
        private readonly BetListService _betlistService = new BetListService();
        private readonly CommonService _commonService = new CommonService();

        /// <summary>
        /// Gets bet details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Redirect to BetDetails view which will show the bet details.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.BetId = options.TransId;
            ViewBag.Username = options.Username;

            var betTypeBelongsBetRadar = _commonService.GetBettypeBelongsBetRadar();

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _betlistService.CreateChoice(options.RefNo);
            var betInfo = _betlistService.GetBetInfo(options.TransId);
            foreach (var ticket in tickets)
            {
                var usedClass = betTypeBelongsBetRadar.Contains(ticket.BetTypeId) ? "ticketDetail betRadar" : "ticketDetail";
                var toolTip = betTypeBelongsBetRadar.Contains(ticket.BetTypeId) ? "Betradar" : string.Empty;
                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='" + usedClass + "' title='" + toolTip + "'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div>", false);
                }
                else
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='" + usedClass + "' title='" + toolTip + "'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div><div class='line'></div>", false);
                }
            }

            var model = new BetDetails
            {
                TicketDetail = builder.GetBetList(),
                ComboTypes = Utility.CacheRef.GetCombinationTypeCombo(betInfo.BetTeam, betInfo.BetCheck, betInfo.Hdp1, betInfo.Hdp2)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult SingleMatchParlay(RMS_BetList_BetBuilder_DetailCriteria criteria)
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

            var content = $"<div style=\"margin-right:10px; padding: 2px 5px; border-right: 3px solid black\">{result.ToString()}</div> ";
            return Content(content);
        }

        /// <summary>
        /// Gets bet details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Redirect to BetDetails view which will show the bet details.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
        public ActionResult SingleMatchParlayDetailPopup(RMS_BetList_BetBuilder_DetailCriteria criteria)
        {
            ViewBag.TransId = criteria.TransId;
            ViewBag.Username = criteria.Username;

            var result = new StringBuilder();

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
            var choice = GetChoiceForSingleMatchParlayDetailPopup(criteria.TransId);

            ViewBag.ResultTexts = resultTexts;
            ViewBag.SelectionNames = selectionNames;
            ViewBag.Choice = choice;

            return View();
        }

        private string GetChoiceForSingleMatchParlayDetailPopup(long transId)
        {
            var model = _betlistService.RMS_BetList_BetBuilder_MatchDetail(transId).FirstOrDefault();

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
            if (eventDateNode != null)
            {
                eventDateNode.Remove();
            }

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
    }
}