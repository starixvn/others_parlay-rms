using ParlayRMS.Controllers;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.Users;
using ParlayRMS.Services.Interface.Users;
using ParlayRMS.Utility;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.MixParlay.TotalOutstanding
{
    [Authorization(Function = FunctionKeys.TotalOutstanding, Policy = UserPolicy.Read)]
    public class TotalOutstandingController : BaseController
    {
        private readonly IService _service = new Service();
        private readonly IUsersService _userService = new UsersService();

        [HttpGet]
        public ActionResult Index(ViewModel viewModel)
        {
            // Combo
            var combos = new List<SelectListItem>();
            for (int i = 2; i <= 5; i++)
            {
                combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = string.IsNullOrEmpty(viewModel.Combo) || viewModel.Combo.Contains(i.ToString())
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.SixUp, Value = "6", Selected = string.IsNullOrEmpty(viewModel.Combo) || viewModel.Combo.Contains("6") });
            ViewBag.Combos = combos;

            // Sports Type
            var sportsTypes = new List<SelectListItem>();
            sportsTypes.Add(new SelectListItem
            {
                Text = ViewLang.Socceronly,
                Value = ((int)SportsType.SoccerOnly).ToString(),
                Selected = string.IsNullOrEmpty(viewModel.SportsType) || viewModel.SportsType.Contains(((int)SportsType.SoccerOnly).ToString())
            });
            sportsTypes.Add(new SelectListItem
            {
                Text = ViewLang.Othersports,
                Value = ((int)SportsType.OtherSports).ToString(),
                Selected = string.IsNullOrEmpty(viewModel.SportsType) || viewModel.SportsType.Contains(((int)SportsType.OtherSports).ToString())
            });
            sportsTypes.Add(new SelectListItem
            {
                Text = ViewLang.Combinedsports,
                Value = ((int)SportsType.CombinedSports).ToString(),
                Selected = string.IsNullOrEmpty(viewModel.SportsType) || viewModel.SportsType.Contains(((int)SportsType.CombinedSports).ToString())
            });
            ViewBag.SportsTypes = sportsTypes;

            // Select Top
            return View(viewModel);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [HttpPost]
        public ActionResult Data(ViewModel viewModel)
        {
            ViewBag.Username = viewModel.Username;
            var customerInfo = _userService.GetCustomerInfo(viewModel.Username);
            if (customerInfo == null || customerInfo.CustId == 0)
            {
                return PartialView("_ErrorMessage", string.Format("{0} is not existed", viewModel.Username));
            }

            if (customerInfo.RoleId == (int)RoleId.Member)
            {
                return DataForMember(customerInfo.CustId, viewModel);
            }
            else if (customerInfo.RoleId == (int)RoleId.Agent)
            {
                return DataForAgent(customerInfo.CustId, viewModel);
            }
            else
            {
                string roleName = string.Empty;
                if (customerInfo.RoleId == (int)RoleId.Master)
                {
                    roleName = "master";
                }
                else
                {
                    roleName = "super";
                }

                return PartialView("_ErrorMessage", string.Format("{0} is a {1}. Please input a member or agent username", viewModel.Username, roleName));
            }
        }

        private ActionResult DataForMember(long custId, ViewModel viewModel)
        {
            var criteria = new MemberCriteria
            {
                CustId = custId,
                Combos = viewModel.Combo,
                SportTypes = viewModel.SportsType
            };
            var data = _service.GetMemberTickets(criteria);
            ViewBag.TotalData = null;
            if (data.Count() > 0)
            {
                decimal totalStackPt = 0;
                decimal totalMaxPayoutPt = 0;
                foreach (var item in data)
                {
                    totalStackPt += item.StakePt;
                    totalMaxPayoutPt += item.MaxPayoutPt;
                }

                ViewBag.TotalData = new TotalDataModel
                {
                    StakePt = totalStackPt,
                    MaxPayoutPt = totalMaxPayoutPt
                };
            }

            return PartialView("MemberResult", data);
        }

        private ActionResult DataForAgent(long custId, ViewModel viewModel)
        {
            var criteria = new AgentCriteria
            {
                CustId = custId,
                Combos = viewModel.Combo,
                SportTypes = viewModel.SportsType
            };
            var data = _service.GetAgentTickets(criteria);
            ViewBag.TotalData = null;
            if (data.Count() > 0)
            {
                decimal totalStackPt = 0;
                decimal totalMaxPayoutPt = 0;
                int totalTotalBets = 0;
                foreach (var item in data)
                {
                    totalStackPt += item.StakePt;
                    totalMaxPayoutPt += item.MaxPayoutPt;
                    totalTotalBets += item.BetCount;
                }

                ViewBag.TotalData = new TotalDataModel
                {
                    StakePt = totalStackPt,
                    MaxPayoutPt = totalMaxPayoutPt,
                    TotalBets = totalTotalBets
                };
            }

            return PartialView("AgentResult", data);
        }
    }
}