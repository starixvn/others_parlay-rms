using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Implement.SimilarBets;
using ParlayRMS.Services.Interface.BetList;
using ParlayRMS.Services.Interface.SimilarBets;
using ParlayRMS.ViewModel.Report;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    /// <summary>
    /// Similar Bets Controller.
    /// </summary>
    public class SimilarBetsController : BaseController
    {
        private readonly ISimilarBetsService _similarbetsService = new SimilarBetsService();
        private readonly IBetListService _betlistService = new BetListService();

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [Authorization(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.betid = Request["betid"] ?? null;
            return View("Index");
        }

        /// <summary>
        /// Gets similar bets of a specific transaction identifier.
        /// </summary>
        /// <param name="transId">The transaction identifier.</param>
        /// <returns>Partial view demonstrates the similar bet list.</returns>
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

        /// <summary>
        /// Gets the bet details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Partial view represents bet detail.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
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

            var ticketsCompare = new List<long>();
            foreach (var ticket in tickets)
            {
                var similar = originalTickets.Any(originalTicket =>
                        originalTicket.LeagueId == ticket.LeagueId &&
                        originalTicket.SportTypeId == ticket.SportTypeId &&
                        originalTicket.HomeId == ticket.HomeId &&
                        originalTicket.AwayId == ticket.AwayId &&
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

            var model = new BetDetails();
            model.TicketDetail = builder.GetBetList();
            model.TicketCompare = string.Join("-", ticketsCompare);

            return PartialView("BetDetails", model);
        }
    }
}