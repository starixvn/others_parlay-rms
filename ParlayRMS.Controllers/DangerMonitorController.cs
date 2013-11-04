// --------------------------------------------------------------------------
// <copyright file="DangerMonitorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.DangerMonitor;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.DangerMonitor;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel;
    using ParlayRMS.ViewModel.DangerMonitor;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// Danger monitor controller.
    /// </summary>
    public class DangerMonitorController : BaseController
    {
        private readonly IDangerMonitorService _dangerMonitorService = new DangerMonitorService();
        private readonly IUsersService _usersService = new UsersService();

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.DangerMonitor, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            var combos = Config.Combos;

            foreach (var combo in combos)
            {
                combo.Selected = combo.Value.Equals("2") || combo.Value.Equals("3");
            }

            ViewBag.Combos = combos;
            ViewBag.DangerousAgents = GetDangerousAgents();

            return this.View("Index");
        }

        /// <summary>
        /// Gets summary betting data of a dangerous agent.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Partial view which shows the table of betting data.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.DangerMonitor, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult Data(DangerMonitorOptions options)
        {
            long maxTransId = 0;
            int total = 0;

            if (options.IsFromConfigList && !string.IsNullOrEmpty(options.AgentIdList))
            {
                var user = this._usersService.GetCustomerInfo(options.AgentIdList);
                options.AgentIdList = user.CustId.ToString();
            }

            var records = this._dangerMonitorService.GetDangerMonitorData(options.AgentIdList, options.Combos, out maxTransId);

            records.ToList().ForEach(obj =>
            {
                total += obj.HdpHome;
                total += obj.HdpAway;
                total += obj.Hdp1stHome;
                total += obj.Hdp1stAway;
                total += obj.OUHome;
                total += obj.OUAway;
                total += obj.OU1stHome;
                total += obj.OU1stAway;
            });
            ViewBag.Total = total;
            ViewBag.MaxTransId = maxTransId;
            ViewBag.AgentIdList = options.AgentIdList;

            return this.PartialView("Data", records);
        }

        /// <summary>
        /// Gets details of a specific match.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Partial view represents the details of a match.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.DangerMonitor, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult Details(DangerMonitorDetailOptions options)
        {
            var records = this._dangerMonitorService.GetDangerMonitorDetails(options.MatchId, options.AgentIdList, options.Combos, options.MaxTransId);

            return this.PartialView("Details", records);
        }

        /// <summary>
        /// Gets the dangerous agent list.
        /// </summary>
        /// <returns>List of dangerous agent names.</returns>
        private List<SelectListItem> GetDangerousAgents()
        {
            var agentList = AppSetting.DangerousAgents;
            var dangerousAgents = new List<SelectListItem>();

            dangerousAgents.AddRange(agentList.Select(obj => new SelectListItem
            {
                Value = obj.Key.ToString(),
                Text = obj.Value
            }));

            return dangerousAgents;
        }
    }
}