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
    using System.Web.Mvc;
    using ParlayRMS.DTO.ViewIPInfo;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Services.Implement.ViewIPInfo;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Interface.ViewIPInfo;
    using ParlayRMS.ViewModel.Report;
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
        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
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
        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult MainReportData(ViewIPInfoOptions options)
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
        /// Drilldown report view.
        /// </summary>
        /// <returns>Drilldown view.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Drilldown()
        {
            return this.View("Drilldown");
        }
    }
}