// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.ViewIpInfo;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Services.Implement.ViewIPInfo;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Interface.ViewIPInfo;
    using ParlayRMS.ViewModel.Monitoring;
    using ParlayRMS.ViewModel.ViewIPInfo;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// Controller for View-IP-Info report.
    /// </summary>
    public class ViewIPInfoController : BaseController
    {
        private readonly IViewIPInfoService _ipinfoService = new ViewIPInfoService();
        private readonly IBetListService _betlistService = new BetListService();

        /// <summary>
        /// ViewIPInfo Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.ViewIP, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ip = Request["ip"];
            ViewBag.date = Request["date"];

            return this.View("Index");
        }

        /// <summary>
        /// Get data for View IP Info main report.
        /// </summary>
        /// <param name="options">Model contain filter conditions.</param>
        /// <returns>JSON String for ViewIP Info main report.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.ViewIP, Policy = UserPolicy.Read)]
        [HttpPost]
        public JsonResult MainReportData(ViewIPInfoOptions options)
        {
            try
            {
                IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> records = _ipinfoService.GetIPInfo(options.IP, options.Date);

                return this.Json(new
                {
                    code = 0,
                    Records = records,
                });
            }
            catch (Exception ex)
            {
                return this.Json(new { code = 1, message = ex.Message });
            }
        }

        /// <summary>
        /// View IP info drill down view.
        /// </summary>
        /// <param name="model">Model option for Drilldown view.</param>
        /// <returns>Drilldown view.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.ViewIP, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Drilldown(DrillDownOptions model)
        {
            ViewBag.SportType = model.SportType;
            ViewBag.UserName = model.UserName;

            IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> models = _ipinfoService.GetIPInfoByUser(model.IP, model.Date, model.UserId, model.SportType);

            return this.View("Drilldown", models);
        }

        /// <summary>
        /// Gets bet sub tickets.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>List of sub tickets.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.ViewIP, Policy = UserPolicy.Read)]
        [HttpPost]
        public JsonResult ParlayBetSubTickets(MixParlayBetDetailsOptions options)
        {
            try
            {
                string ticketString = BuildTickets(options.RefNo);

                return this.Json(new
                {
                    code = 0,
                    data = ticketString
                });
            }
            catch (Exception ex)
            {
                return this.Json(new { code = 1, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets bet details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Redirect to BetDetails view which will show the bet details.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ViewIP, Policy = UserPolicy.Read)]
        public ActionResult ParlayBetDetails(ParlayBetDetailsOptions options)
        {
            ViewBag.ComboType = options.ComboType;
            CombinationDetailsByDateViews model = _ipinfoService.GetBetCombinationDetails(options.RefNo);

            return View("BetDetails", model);
        }

        #region FunctionUtility
        /// <summary>
        /// Build sub ticket HTML string.
        /// </summary>
        /// <param name="refNo">Bet REFNO.</param>
        /// <returns>HTML string for ticket.</returns>
        private string BuildTickets(long refNo)
        {
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var tickets = this._betlistService.CreateChoice(refNo).ToList();
            int countTicket = 1;
            string endTag = "<hr class = 'hr'>";
            foreach (var ticket in tickets)
            {
                if (countTicket == tickets.Count)
                {
                    endTag = string.Empty;
                }

                builder.AddChoice(ticket, ticketHelper, null, null, endTag, false);
                countTicket++;
            }

            return builder.GetBetList();
        }
        #endregion
    }
}