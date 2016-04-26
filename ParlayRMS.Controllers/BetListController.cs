using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Interface.BetList;
using ParlayRMS.ViewModel.BetList;
using ParlayRMS.Web.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    public class BetListController : BaseController
    {
        private readonly IBetListService _betlistService = new BetListService();

        /// <summary>
        /// Gets bet details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Redirect to BetDetails view which will show the bet details.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Need to review here.")]
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _betlistService.CreateChoice(options.RefNo);
            foreach (var ticket in tickets)
            {
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

            return View("BetDetails", model);
        }
    }
}