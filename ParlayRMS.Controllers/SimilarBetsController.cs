using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using ParlayRMS.DTO.BetList;
using ParlayRMS.DTO.SimilarBets;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Implement.SimilarBets;
using ParlayRMS.ViewModel.Report;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Controllers
{
    public class SimilarBetsController : BaseController
    {
        private readonly SimilarBetsService _similarbetsService = new SimilarBetsService();
        private readonly BetListService _betlistService = new BetListService();

        [Authorization(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.betid = Request["betid"] ?? null;
            return View("Index");
        }

        [Authorization(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult Data(long transId)
        {
            ViewBag.TransId = transId;
            var model = _similarbetsService.GetSimilarBets(transId);
            if (model == null)
            {
                return PartialView("NotFound");
            }

            return PartialView("Data", model);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        [Authorization(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _betlistService.CreateChoice(options.RefNo);
            var originalTickets = _betlistService.CreateChoice(options.OriginalRefNo);
            var betInfo = _betlistService.GetBetInfo(options.BetId);
            var ticketsCompare = new List<long>();
            foreach (var ticket in tickets)
            {
                var similar = originalTickets.Any(originalTicket =>
                        originalTicket.MatchId == ticket.MatchId &&
                        originalTicket.BetTypeId == ticket.BetTypeId &&
                        originalTicket.BetTeam == ticket.BetTeam);

                if (!similar)
                {
                    ticketsCompare.Add(ticket.TransId);
                }

                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.TransId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div>", false);
                }
                else
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.TransId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div><div class='line'></div>", false);
                }
            }

            var model = new BetDetails
            {
                TicketDetail = builder.GetBetList(),
                TicketCompare = string.Join("-", ticketsCompare),
                ComboTypes = Utility.CacheRef.GetCombinationTypeCombo(betInfo.BetTeam, betInfo.BetCheck, betInfo.Hdp1, betInfo.Hdp2)
            };

            return PartialView("BetDetails", model);
        }

        public ActionResult SingleMatchParlayDetailPopup(SingleMatchParlaySimilarDetailOption options)
        {
            ViewBag.TransId = options.TransId;
            ViewBag.Username = options.Username;
            options.Selection = options.Selection ?? string.Empty;

            var originalSelections = GetSingleMatchParlaySelection(options.OriginalCustId, options.OriginalTransId).Select(s => s.SelectionType).ToList();
            var selections = GetSingleMatchParlaySelection(options.CustId, options.TransId);
            var selectionNames = new List<string>();
            var resultTexts = new List<string>();
            var differences = new List<string>();
            if (selections.Any())
            {
                foreach (var s in selections)
                {
                    var name = Utility.CacheRef.RemotingRefService.GetBetBuilderSelectionNames(new[] { s.SelectionType }, "en-US").FirstOrDefault();
                    selectionNames.Add(name);
                    resultTexts.Add(s.Result);
                    if (!originalSelections.Contains(s.SelectionType))
                    {
                        differences.Add("#");
                    }
                    else
                    {
                        differences.Add(string.Empty);
                    }
                }
            }

            var choice = GetChoiceForSingleMatchParlayDetailPopup(options.TransId);

            ViewBag.ResultTexts = resultTexts;
            ViewBag.SelectionNames = selectionNames;
            ViewBag.Differences = differences;
            ViewBag.Choice = choice;

            return View();
        }

        private List<RMS_BetList_BetBuilder_DetailModel> GetSingleMatchParlaySelection(int custId, long transId)
        {
            var criteria = new RMS_BetList_BetBuilder_DetailCriteria
            {
                CustId = custId,
                TransId = transId
            };
            return _betlistService.RMS_BetList_BetBuilder_Detail(criteria);
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