// --------------------------------------------------------------------------
// <copyright file="SimilarBetsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
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
        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.betid = Request["betid"] ?? null;
            return this.View("Index");
        }

        /// <summary>
        /// Gets similar bets of a specific transaction identifier.
        /// </summary>
        /// <param name="transId">The transaction identifier.</param>
        /// <returns>Partial view demonstrates the similar bet list.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult Data(long transId)
        {
            ViewBag.TransId = transId;
            var model = this._similarbetsService.GetSimilarBets(transId);
            if (model == null)
            {
                return this.PartialView("NotFound");
            }

            return this.PartialView("Data", model);
        }

        /// <summary>
        /// Gets the bet details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Partial view represents bet detail.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = this._betlistService.CreateChoice(options.RefNo);
            var originalTickets = this._betlistService.CreateChoice(options.OriginalRefNo);
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