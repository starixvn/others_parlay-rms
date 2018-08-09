using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.Services.Interface;
using ParlayRMS.Services.Interface.BetList;
using ParlayRMS.ViewModel.BetList;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Controllers
{
    public class BetListController : BaseController
    {
        private readonly IBetListService _betlistService = new BetListService();
        private readonly ICommonService _commonService = new CommonService();

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
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            var betTypeBelongsBetRadar = _commonService.GetBettypeBelongsBetRadar();

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _betlistService.CreateChoice(options.RefNo);
            var betInfo = _betlistService.GetBetInfo(options.BetId);
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
    }
}