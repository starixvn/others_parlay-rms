namespace ParlayRMS.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using PagedList;
    using Services.Agents;
    using Utility;
    using ViewModel.ManageAgent;

    public class ManageAgentController : BaseController
    {
        private readonly IUserService _userService = new UserService();
        private readonly AgentService _agentService = new AgentService();
        private readonly int _pageSize = AppSetting.PageSize;

        public ManageAgentController()
        {
            ViewBag.HasWritePermission = _userService.IsHavingPermission(
                CurrentSession.UserId,
                NapSettings.Settings.SiteId,
                FunctionKeys.ManageAgent,
                Constant.Policy.Host
                );
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageAgent, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchBar(SearchBarModel model)
        {
            return PartialView(model);
        }

        [NapAuth(FunctionKey = FunctionKeys.ManageAgent, Policy = Constant.Policy.Read)]
        public ActionResult AgentList(AgentListModel model)
        {
            var agentList = _agentService.GetDangerousAgents(model.UserName);
            var page = model.Page ?? 1;

            ViewBag.Pager = Pager.Items(agentList.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
            return PartialView(agentList.ToPagedList(page, _pageSize));
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageAgent, Policy = Constant.Policy.Host)]
        public ActionResult AddAgent()
        {
            var model = new AddAgentModel();
            return PartialView(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageAgent, Policy = Constant.Policy.Host)]
        public ActionResult AddAgent(AddAgentModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _agentService.AddDangerousAgent(model.UserName, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageAgent, Policy = Constant.Policy.Host)]
        public ActionResult DeleteAgent(DeleteAgentModel model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelJson();
            }

            var result = _agentService.DeleteDangerousAgent(model.AgentId, model.UserName, CurrentSession.UserId, CurrentContext.Ip4Address);
            return Json(result);
        }
    }
}
