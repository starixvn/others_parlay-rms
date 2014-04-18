// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using System.Web.Mvc;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Services.Implement.ViewIPInfo;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Interface.ViewIPInfo;
    using ParlayRMS.ViewModel.Report;
    using ParlayRMS.Web.Authorization;
    
    /// <summary>
    /// IPInfo Controller.
    /// </summary>
    public class ViewIPInfoController : BaseController
    {
        private readonly IViewIPInfoService _ipinfoService = new ViewIPInfoService();
        private readonly IBetListService _betlistService = new BetListService();

        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ip = Request["ip"] ?? null;
            ViewBag.date = Request["date"] ?? null;
            return this.View("Index");
        }

        [AuthorizationAttribute(Function = FunctionKeys.SimilarBets, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult Data(IPInfoOptions options)
        {
            try
            {
                var records = this._ipinfoService.GetIPInfo(options.IP, options.Date);
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
    }
}
