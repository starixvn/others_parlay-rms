using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.TotalOutstanding;
using ParlayRMS.Services.Implement.Users;
using ParlayRMS.Services.Interface.TotalOutstanding;
using ParlayRMS.Services.Interface.Users;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.TotalOutstanding;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.TotalOutstanding, Policy = UserPolicy.Read)]
    public class TotalOutstandingController : BaseController
    {
        private readonly ITotalOutstandingService _service = new TotalOutstandingService();
        private readonly IUsersService _userService = new UsersService();

        [HttpGet]
        public ActionResult Index(IndexModel model)
        {
            // Combo
            var combos = new List<SelectListItem>();
            for (int i = 2; i <= 9; i++)
            {
                combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = string.IsNullOrEmpty(model.Combo) || model.Combo.Contains(i.ToString())
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.TenUp, Value = "10", Selected = string.IsNullOrEmpty(model.Combo) || model.Combo.Contains("10") });
            ViewBag.Combos = combos;

            // Parlay type
            var parlayTypes = new List<SelectListItem>();
            parlayTypes.Add(new SelectListItem
            {
                Text = ViewLang.MixparLay,
                Value = ((int)ParlayType.MixParlay).ToString(),
                Selected = string.IsNullOrEmpty(model.ParlayTypes) || model.ParlayTypes.Contains(((int)ParlayType.MixParlay).ToString())
            });
            parlayTypes.Add(new SelectListItem
            {
                Text = ViewLang.ComboParlay,
                Value = ((int)ParlayType.ComboParlay).ToString(),
                Selected = string.IsNullOrEmpty(model.ParlayTypes) || model.ParlayTypes.Contains(((int)ParlayType.ComboParlay).ToString())
            });
            ViewBag.ParlayTypes = parlayTypes;

            // Sports Type
            var sportsTypes = new List<SelectListItem>();
            sportsTypes.Add(new SelectListItem
            {
                Text = ViewLang.Socceronly,
                Value = ((int)SportsType.SoccerOnly).ToString(),
                Selected = string.IsNullOrEmpty(model.SportsType) || model.SportsType.Contains(((int)SportsType.SoccerOnly).ToString())
            });
            sportsTypes.Add(new SelectListItem
            {
                Text = ViewLang.Othersports,
                Value = ((int)SportsType.OtherSports).ToString(),
                Selected = string.IsNullOrEmpty(model.SportsType) || model.SportsType.Contains(((int)SportsType.OtherSports).ToString())
            });

            // Select Top
            return View("Index", model);
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [HttpPost]
        public ActionResult Data(DataModel model)
        {
            ViewBag.Username = model.Username;
            var customerInfo = _userService.GetCustomerInfo(model.Username);
            if (customerInfo == null || customerInfo.CustId == 0)
            {
                return PartialView("_ErrorMessage", string.Format("{0} is not existed", model.Username));
            }

            if (customerInfo.RoleId == (int)RoleId.Member)
            {
                var data = _service.GetTotalOutstandingForMember(customerInfo.CustId, model.Combo, model.SportsType, model.ParlaysType);
                ViewBag.TotalData = null;
                if (data.Count > 0)
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
            else if (customerInfo.RoleId == (int)RoleId.Agent)
            {
                var data = _service.GetTotalOutstandingForAgent(customerInfo.CustId, model.Combo, model.SportsType, model.ParlaysType);
                ViewBag.TotalData = null;
                if (data.Count > 0)
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

                return PartialView("_ErrorMessage", string.Format("{0} is a {1}. Please input a member or agent username", model.Username, roleName));
            }
        }
    }
}