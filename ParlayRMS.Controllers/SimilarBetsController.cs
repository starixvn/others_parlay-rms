using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.DTO.BetList;
using ParlayRMS.DTO.SimilarBets;
using ParlayRMS.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using Services;

    public class SimilarBetsController : BaseController
    {
        private readonly SimilarBetsService _similarbetsService = new SimilarBetsService();
        private readonly BetDetailsService _betDetailService = new BetDetailsService();

        [NapAuth(FunctionKey = FunctionKeys.SimilarBets, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.betid = Request["betid"];
            return View("Index");
        }

        [NapAuth(FunctionKey = FunctionKeys.SimilarBets, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult Data(long transId, bool isSimilar)
        {
            ViewBag.TransId = transId;
            var model = _similarbetsService.GetSimilarBets(transId, isSimilar);
            return model == null ? PartialView("NotFound") : PartialView("Data", model);
        }

        [NapAuth(FunctionKey = FunctionKeys.SimilarBets, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.BetId = options.TransId;
            ViewBag.Username = options.Username;

            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _betDetailService.CreateChoice(options.TransId, options.RefNo);
            var originalTickets = _betDetailService.CreateChoice(options.OriginalTransId, options.OriginalRefNo);
            var betInfo = _betDetailService.GetBetInfo(options.TransId);
            var ticketsCompare = new List<long>();
            for (var i = 0; i < tickets.Item2.Count; i++)
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
                    differences.Add(!originalSelections.Contains(s.SelectionType) ? "#" : string.Empty);
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
