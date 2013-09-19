// --------------------------------------------------------------------------
// <copyright file="MemberWinLossController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.09.16">Created</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.MemberWinLoss;
    using ParlayRMS.Services.Interface.MemberWinLoss;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel;
    using ParlayRMS.ViewModel.Report;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// Member Win Loss Controller.
    /// </summary>
    public class MemberWinLossController : BaseController
    {
        private readonly IMemberWinLossService memberWinLossService = new MemberWinLossService();

        /// <summary>
        /// Redirect to Index view.
        /// </summary>
        /// <returns></returns>
        [AuthorizationAttribute(Function = FunctionKeys.MemberWinLoss, Policy = ePolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.BetTypes = Config.BetTypes;
            ViewBag.SportsTypes = Config.SportsTypes;
            ViewBag.LimitDate = MemberWinLossOptions.GetLimitDate();
            return this.View();
        }

        /// <summary>
        /// Get data based on filtered conditions.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [AuthorizationAttribute(Function = FunctionKeys.MemberWinLoss, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult Data(MemberWinLossOptions options)
        {
            try
            {
                DateTime limitDate = MemberWinLossOptions.GetLimitDate();

                if (options.FromDate > options.ToDate)
                {
                    ModelState.AddModelError("FromDate", "FromDate must be less than ToDate");
                }

                //Minimum date that support to query data
                if (options.FromDate.Date < limitDate)
                {
                    ModelState.AddModelError("FromDate", string.Format("FromDate must be greater or equal than {0}", limitDate.ToShortDateString()));
                }

                if (!ModelState.IsValid)
                {
                    return this.PartialView("_ErrorMessage", GetModelErrorMessage());
                }
                else
                {
                    var model = this.memberWinLossService.GetMemberWinLoss(options.FromDate, 
                                                                           options.ToDate, 
                                                                           options.BetType, 
                                                                           options.SportsType, 
                                                                           options.Combos,
                                                                           options.Username);
                    ViewBag.Title = options.GetTitle();
                    ViewBag.FilterOption = options;
                    return this.PartialView(model);
                }
            }
            catch (Exception ex)
            {
                return this.PartialView("_ErrorMessage", ex.Message);
            }
        }

        /// <summary>
        /// Export Member Win Loss Report to Excel file.
        /// </summary>
        /// <param name="options"></param>
        [AuthorizationAttribute(Function = FunctionKeys.MemberWinLoss, Policy = ePolicy.Read)]
        [HttpGet]
        public void Excel(MemberWinLossOptions options)
        {
            var model = this.memberWinLossService.GetMemberWinLoss(options.FromDate,
                                                                           options.ToDate,
                                                                           options.BetType,
                                                                           options.SportsType,
                                                                           options.Combos,
                                                                           options.Username);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\MemberWinLoss.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            var title = options.GetTitle();
            writer.WriteRow(0, new object[] { title });

            // header
            var header1 = new object[]{
                ViewLang.Month,
                ViewLang.Ticket,
                "",
                ViewLang.StakeAfterPT,
                "",
                ViewLang.WinLoss,
                "",
                ViewLang.OurPT
            };

            var header2 = new object[]{
                "",
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.NetWinloss
            };

            writer.WriteRows(1, new List<object[]>() { header1, header2 });

            // records
            writer.WriteRows(3, model.Records);

            // footer
            writer.WriteRow(4, new object[] {
                ViewLang.Total,
                model.Total.Tickets_Won,
                model.Total.Tickets_Lose,
                model.Total.Stake_Won,
                model.Total.Stake_Lose,
                model.Total.Member_Won,
                model.Total.Member_Lose,
                model.Total.OurPT_Won,
                model.Total.OurPT_Lose,
                model.Total.OurPT_NetWinloss
            });

            writer.SyncColumnWidths(10);

            var ms = writer.Render("MemberWinLoss");
            Response.AddHeader("content-disposition", "attachment; filename=" + "MemberWinLoss.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}
