using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
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
    }
}