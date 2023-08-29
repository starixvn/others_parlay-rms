namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using Models.Functions;
    using Services.Agents;
    using Services.Users;
    using Utility;
    using ViewModel.ManageAgent;
    using Web.Authorization;

    public class ManageAgentController : BaseController
    {
        private readonly UsersService _userService = new UsersService();
        private readonly AgentService _agentService = new AgentService();
        private readonly int _pageSize = AppSetting.PageSize;

        public ManageAgentController()
        {
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageAgent);
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchBar(SearchBarModel model)
        {
            return PartialView(model);
        }

        [Authorization(Function = FunctionKeys.ManageCustomer, Policy = UserPolicy.Read)]
        public ActionResult AgentList(AgentListModel model)
        {
            var agentList = _agentService.GetDangerousAgents(model.UserName);
            var page = model.Page == null ? 1 : model.Page.Value;

            ViewBag.Pager = Pager.Items(agentList.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return PartialView(agentList.ToPagedList(page, _pageSize));
        }

        [HttpGet]
        [Authorization(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Create)]
        public ActionResult AddAgent()
        {
            var model = new AddAgentModel();
            return PartialView(model);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Create)]
        public ActionResult AddAgent(AddAgentModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _agentService.AddDangerousAgent(model.UserName, CurrentSession.CustId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpPost]
        [Authorization(Function = FunctionKeys.ManageAgent, Policy = UserPolicy.Create)]
        public ActionResult DeleteAgent(DeleteAgentModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _agentService.DeleteDangerousAgent(model.AgentId, model.UserName, CurrentSession.CustId, CurrentContext.Ip4Address);
            return Json(result);
        }
    }
}
