// -----------------------------------------------------------------------
// <copyright file="SimilarBetsController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Implement.SimilarBets;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.SimilarBets;
    using ParlayRMS.ViewModel.Report;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Models.Functions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimilarBetsController : BaseController
    {
        private readonly ISimilarBetsService similarbetsService = new SimilarBetsService();
        private readonly IBetListService betlistService = new BetListService();

        [AuthorizationAttribute(Function = eFunction.SimilarBets, Policy = ePolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.betid = Request["betid"] ?? null;
            return this.View("Index");
        }

        [AuthorizationAttribute(Function = eFunction.SimilarBets, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult Data(long transId)
        {
            ViewBag.TransId = transId;
            var model = this.similarbetsService.GetSimilarBets(transId);
            if (model == null)
            {
                return this.PartialView("NotFound");
            }

            return this.PartialView("Data", model);
        }

        [AuthorizationAttribute(Function = eFunction.SimilarBets, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper();
            var tickets = this.betlistService.CreateChoice(options.RefNo);
            var originalTickets = this.betlistService.CreateChoice(options.OriginalRefNo);
            StringBuilder ticketsCompare = new StringBuilder();
            foreach (var ticket in tickets)
            {
                bool flag = false;
                foreach (var originalTicket in originalTickets)
                {
                    if (originalTicket.LeagueId == ticket.LeagueId &&
                        originalTicket.SportTypeId == ticket.SportTypeId &&
                        originalTicket.HomeId == ticket.HomeId &&
                        originalTicket.AwayId == ticket.AwayId &&
                        originalTicket.BetTypeId == ticket.BetTypeId &&
                        originalTicket.BetTeam == ticket.BetTeam)
                    {
                        flag = true;
                    }
                }

                if (!flag)
                {
                    ticketsCompare.Append(ticket.BetId);
                    ticketsCompare.Append("-");
                }

                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div>", false);
                }
                else
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div><div class='line'></div>", false);
                }
            }

            var model = new BetDetails();
            model.TicketDetail = builder.GetBetList();
            model.TicketCompare = string.IsNullOrEmpty(ticketsCompare.ToString()) ? "null" : ticketsCompare.ToString().Substring(0, ticketsCompare.ToString().Length - 1);

            return this.PartialView("BetDetails", model);
        }
    }
}