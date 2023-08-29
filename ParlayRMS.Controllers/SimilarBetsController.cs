using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.DTO.BetList;
using ParlayRMS.DTO.SimilarBets;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.BetDetails;
using ParlayRMS.Services.SimilarBets;
using ParlayRMS.ViewModel.Report;
using ParlayRMS.Web.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    public class SimilarBetsController : BaseController
    {
        private readonly SimilarBetsService _similarbetsService = new SimilarBetsService();
        private readonly BetDetailsService _betDetailService = new BetDetailsService();

        [Authorization(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.betid = Request["betid"] ?? null;
            return View("Index");
        }

        [Authorization(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult Data(long transId, bool isSimilar)
        {
            ViewBag.TransId = transId;
            var model = _similarbetsService.GetSimilarBets(transId, isSimilar);
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
            ViewBag.BetId = options.TransId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _betDetailService.CreateChoice(options.TransId, options.RefNo);
            var originalTickets = _betDetailService.CreateChoice(options.OriginalTransId, options.OriginalRefNo);
            var betInfo = _betDetailService.GetBetInfo(options.TransId);
            var ticketsCompare = new List<long>();
            for (int i = 0; i < tickets.Item2.Count; i++)
            {
                var blackListLeague = tickets.Item1[i] ? "blackListClass blackListMargin" : string.Empty;
                var ticket = tickets.Item2[i];
                var similar = originalTickets.Item2.Any(originalTicket =>
                        originalTicket.MatchId == ticket.MatchId &&
                        originalTicket.BetTypeId == ticket.BetTypeId &&
                        originalTicket.BetTeam == ticket.BetTeam);

                if (!similar)
                {
                    ticketsCompare.Add(ticket.TransId);
                }

                if (tickets.Item2.Last() == ticket)
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.TransId + "'><div  class='" + blackListLeague + "'></div><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div>", false);
                }
                else
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.TransId + "'><div  class='" + blackListLeague + "'></div><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div><div class='line'></div>", false);
                }
            }

            var comboTypeAndStake = GetComboTypeAndStake(tickets.Item2, betInfo);

            var model = new BetDetails
            {
                TicketDetail = builder.GetBetList(),
                TicketCompare = string.Join("-", ticketsCompare),
                ComboTypes = comboTypeAndStake
            };

            return PartialView("BetDetails", model);
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
            var count = Math.Min(comboTypes?.Length ?? 0, hasStaketickets.Count());
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

        public ActionResult SingleMatchParlayDetailPopup(SingleMatchParlaySimilarDetailOption options)
        {
            ViewBag.TransId = options.TransId;
            ViewBag.Username = options.Username;
            options.Selection = options.Selection ?? string.Empty;

            var originalSelections = _betDetailService.GetSingleMatchParlaySelection(options.OriginalCustId, options.OriginalTransId).Select(s => s.SelectionType).ToList();
            var selections = _betDetailService.GetSingleMatchParlaySelection(options.CustId, options.TransId);
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

            var choice = _betDetailService.GetChoiceForSingleMatchParlay(options.TransId);

            ViewBag.ResultTexts = resultTexts;
            ViewBag.SelectionNames = selectionNames;
            ViewBag.Differences = differences;
            ViewBag.Choice = choice.Item2;
            ViewBag.IsBlackListLeague = choice.Item1;

            return View();
        }
    }
}