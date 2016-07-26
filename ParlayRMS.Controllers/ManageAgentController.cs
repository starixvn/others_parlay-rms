// --------------------------------------------------------------------------
// <copyright file="ManageAgentController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.04">Created</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.Agents;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.Agents;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.ManageAgent;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// Agent Management Controller.
    /// </summary>
    public class ManageAgentController : BaseController
    {
        private readonly IUsersService _userService = new UsersService();
        private readonly IAgentService _agentService = new AgentService();
        private int _pageSize = AppSetting.PageSize;

        public ManageAgentController()
        {
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageAgent);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Gets partial view of Search bar.
        /// </summary>
        /// <param name="model">Model that is used to bind with the search bar.</param>
        /// <returns>Partial view of Search bar.</returns>
        public ActionResult SearchBar(SearchBarModel model)
        {
            return this.PartialView(model);
        }

        /// <summary>
        /// Gets dangerous agent list.
        /// </summary>
        /// <param name="model">Filter conditions.</param>
        /// <returns>List of agents.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Read)]
        public ActionResult AgentList(AgentListModel model)
        {
            var agentList = this._agentService.GetDangerousAgents(model.UserName);
            int page = model.Page == null ? 1 : model.Page.Value;

            ViewBag.Pager = Pager.Items(agentList.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return this.PartialView(agentList.ToPagedList(page, _pageSize));
        }

        /// <summary>
        /// GET method: redirects to Add New Agent form.
        /// </summary>
        /// <returns>Partial view represents the Add New Agent form.</returns>
        [HttpGet]
        [AuthorizationAttribute(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Create)]
        public ActionResult AddAgent()
        {
            var model = new AddAgentModel();
            return this.PartialView(model);
        }

        /// <summary>
        /// POST method: adds a new dangerous agent.
        /// </summary>
        /// <param name="model">The model contains the information of new dangerous agent.</param>
        /// <returns>Whether the new dangerous agent is added successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Create)]
        public ActionResult AddAgent(AddAgentModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._agentService.AddDangerousAgent(model.UserName, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }

        /// <summary>
        /// POST method: deletes a dangerous agent.
        /// </summary>
        /// <param name="model">The model contains the information of an existing dangerous agent.</param>
        /// <returns>Whether the new dangerous agent is deleted successfully or not.</returns>
        [HttpPost]
        [AuthorizationAttribute(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Create)]
        public ActionResult DeleteAgent(DeleteAgentModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.InvalidModelJson();
            }

            var result = this._agentService.DeleteDangerousAgent(model.AgentId, model.UserName, CurrentSession.CustId, CurrentContext.Ip4Address);
            return this.Json(result);
        }
    }
}
