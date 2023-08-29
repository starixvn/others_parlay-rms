using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.Controllers;
using ParlayRMS.Models.Functions;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.MixParlay.SimilarBet
{
    [Authorization(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
    public class SimilarBetController : BaseController
    {
        private readonly IService _service = new Service();
        private readonly BetList.IService _betlistService = new BetList.Service();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.betid = Request["betid"] ?? null;
            return View();
        }

        [HttpPost]
        public ActionResult Data(ViewModel viewModel)
        {
            ViewBag.TransId = viewModel.TransId;
            var model = _service.GetTicket(viewModel.TransId);
            if (model == null)
            {
                return PartialView("NotFound");
            }

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult BetDetails(BetDetailsViewModel viewModel)
        {
            ViewBag.BetId = viewModel.BetId;
            ViewBag.Username = viewModel.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = this._betlistService.CreateChoice(viewModel.RefNo);
            var originalTickets = this._betlistService.CreateChoice(viewModel.OriginalRefNo);

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

            var model = new BetDetails
            {
                TicketDetail = builder.GetBetList(),
                TicketCompare = string.Join("-", ticketsCompare)
            };
            return PartialView(model);
        }
    }
}
