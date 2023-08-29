using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.Controllers;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.Services.Interface;
using ParlayRMS.Web.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.MixParlay.BetList
{
    [Authorization(Function = FunctionKeys.MonitorMixParlay, Policy = UserPolicy.Read)]
    public class BetListController : BaseController
    {
        private readonly IService _service = new Service();
        private readonly ICommonService _commonService = new CommonService();

        [HttpPost]
        public ActionResult BetDetails(ViewModel model)
        {
            ViewBag.BetId = model.BetId;
            ViewBag.Username = model.Username;

            var betTypeBelongsBetRadar = _commonService.GetBettypeBelongsBetRadar();

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = _service.CreateChoice(model.RefNo);
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

            BetDetails newModel = new BetDetails();
            newModel.TicketDetail = builder.GetBetList();

            return View("BetDetails", newModel);
        }
    }
}
