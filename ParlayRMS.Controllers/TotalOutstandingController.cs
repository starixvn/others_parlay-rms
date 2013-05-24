// -----------------------------------------------------------------------
// <copyright file="TotalOutstandingController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ParlayRMS.DTO;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.TotalOutstanding;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.TotalOutstanding;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.TotalOutstanding;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TotalOutstandingController : BaseController
    {
        private readonly ITotalOutstandingService service = new TotalOutstandingService();
        private readonly IUsersService userService = new UsersService();

        [AuthorizationAttribute(Function = FunctionKeys.TotalOutstanding, Policy = ePolicy.Read)]
        [HttpGet]
        public ActionResult Index(IndexModel model)
        {
            // Combo
            var combos = new List<SelectListItem>();
            for (int i = 2; i <= 5; i++)
            {
                combos.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = string.IsNullOrEmpty(model.Combo) || model.Combo.Contains(i.ToString())
                });
            }

            combos.Add(new SelectListItem { Text = ViewLang.SixUp, Value = "6", Selected = string.IsNullOrEmpty(model.Combo) || model.Combo.Contains("6") });
            ViewBag.Combos = combos;

            // Sports Type
            var sportsTypes = new List<SelectListItem>();
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Socceronly, Value = ((int)SportsType.SoccerOnly).ToString(), Selected = string.IsNullOrEmpty(model.SportsType) || model.SportsType.Contains(((int)SportsType.SoccerOnly).ToString()) });
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Othersports, Value = ((int)SportsType.OtherSports).ToString(), Selected = string.IsNullOrEmpty(model.SportsType) || model.SportsType.Contains(((int)SportsType.OtherSports).ToString()) });
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Combinedsports, Value = ((int)SportsType.CombinedSports).ToString(), Selected = string.IsNullOrEmpty(model.SportsType) || model.SportsType.Contains(((int)SportsType.CombinedSports).ToString()) });
            ViewBag.SportsTypes = sportsTypes;

            // Select Top
            return this.View("Index", model);
        }

        //[ValidateInput(false)]
        [AuthorizationAttribute(Function = FunctionKeys.TotalOutstanding, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult Data(DataModel model)
        {
            ViewBag.Username = model.Username;
            var customerInfo = userService.GetCustomerInfo(model.Username);
            if (customerInfo == null || customerInfo.CustId == 0)
            {
                return this.PartialView("_ErrorMessage", string.Format("{0} is not existed", model.Username));
            }

            if (customerInfo.RoleId == (int)RoleId.Member)
            {
                var data = service.GetTotalOutstandingForMember(customerInfo.CustId, model.Combo, model.SportsType);
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

                return this.PartialView("MemberResult", data);
            }
            else if (customerInfo.RoleId == (int)RoleId.Agent)
            {
                var data = service.GetTotalOutstandingForAgent(customerInfo.CustId, model.Combo, model.SportsType);
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

                return this.PartialView("AgentResult", data);
            }
            else
            {
                string roleName = "";
                if (customerInfo.RoleId == (int)RoleId.Master)
                {
                    roleName = "master";
                }
                else
                {
                    roleName = "super";
                }

                return this.PartialView("_ErrorMessage", string.Format("{0} is a {1}. Please input a member or agent username", model.Username, roleName));
            }
        }
    }
}