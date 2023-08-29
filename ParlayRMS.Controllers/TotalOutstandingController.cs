using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.TotalOutstanding;
using ParlayRMS.Services.Users;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.TotalOutstanding;
using ParlayRMS.Web.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.TotalOutstanding, Policy = UserPolicy.Read)]
    public class TotalOutstandingController : BaseController
    {
        private readonly TotalOutstandingService _service = new TotalOutstandingService();
        private readonly UsersService _userService = new UsersService();

        [HttpGet]
        public ActionResult Index(IndexModel model)
        {
            var combos = new List<SelectListItem>();
            for (var i = 2; i <= 9; i++)
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

            var parlayTypes = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = ViewLang.MixparLay,
                    Value = ((int)ParlayType.MixParlay).ToString(),
                    Selected = string.IsNullOrEmpty(model.ParlayTypes) || model.ParlayTypes.Contains(((int)ParlayType.MixParlay).ToString())
                },
                new SelectListItem
                {
                    Text = ViewLang.GodOfFortune,
                    Value = ((int)ParlayType.GodOfFortune).ToString(),
                    Selected = string.IsNullOrEmpty(model.ParlayTypes) || model.ParlayTypes.Contains(((int)ParlayType.GodOfFortune).ToString())
                },
                new SelectListItem
                {
                    Text = ViewLang.ComboParlay,
                    Value = ((int)ParlayType.ComboParlay).ToString(),
                    Selected = string.IsNullOrEmpty(model.ParlayTypes) || model.ParlayTypes.Contains(((int)ParlayType.ComboParlay).ToString())
                },
                new SelectListItem
                {
                    Text = ViewLang.PenaltyShootoutCombo,
                    Value = ((int)ParlayType.PenaltyShootoutCombo).ToString(),
                    Selected = string.IsNullOrEmpty(model.ParlayTypes) || model.ParlayTypes.Contains(((int)ParlayType.PenaltyShootoutCombo).ToString())
                },
                new SelectListItem
                {
                    Text = ViewLang.SingleMatchParlay,
                    Value = ((int)ParlayType.SingleMatchParlay).ToString(),
                    Selected = string.IsNullOrEmpty(model.ParlayTypes) || model.ParlayTypes.Contains(((int)ParlayType.SingleMatchParlay).ToString())
                }
            };

            ViewBag.ParlayTypes = parlayTypes;

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Data(DataModel model)
        {
            ViewBag.Username = model.Username;
            var customerInfo = _userService.GetCustomerInfo(model.Username);
            ViewBag.CustId = customerInfo.CustId;
            if (customerInfo == null || customerInfo.CustId == 0)
            {
                return PartialView("_ErrorMessage", string.Format("{0} is not existed", model.Username));
            }

            if (customerInfo.RoleId == (int)RoleId.Member)
            {
                var data = _service.GetTotalOutstandingForMember(customerInfo.CustId, model.Combo, model.SportsType, model.ParlaysType, model.IsCornerParlay);
                ViewBag.CustomerClass = string.Empty;
                ViewBag.TotalData = null;
                if (data.Count > 0)
                {
                    decimal totalStackPt = 0;
                    decimal totalMaxPayoutPt = 0;
                    decimal totalPotentialWinningAmount = 0;
                    foreach (var item in data)
                    {
                        totalStackPt += item.StakePt;
                        totalMaxPayoutPt += item.MaxPayoutPt;
                        totalPotentialWinningAmount += item.PotentialWinningAmount;
                    }
                    ViewBag.CustomerClass = data.FirstOrDefault().CustomerClass;
                    ViewBag.TotalData = new TotalDataModel
                    {
                        StakePt = totalStackPt,
                        PotentialWinningAmount = totalPotentialWinningAmount,
                        MaxPayoutPt = totalMaxPayoutPt
                    };
                }

                return PartialView("MemberResult", data);
            }
            else if (customerInfo.RoleId == (int)RoleId.Agent)
            {
                var data = _service.GetTotalOutstandingForAgent(customerInfo.CustId, model.Combo, model.SportsType, model.ParlaysType, model.IsCornerParlay);
                ViewBag.TotalData = null;
                ViewBag.CustomerClass = string.Empty;
                if (data.Count > 0)
                {
                    decimal totalStackPt = 0;
                    decimal totalMaxPayoutPt = 0;
                    var totalTotalBets = 0;
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
                var roleName = string.Empty;
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
