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
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.MemberWinLoss;
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
                int roleId = 0;

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
                    var model = this.memberWinLossService.GetMemberWinLoss(
                                                              options.FromDate, 
                                                              options.ToDate, 
                                                              options.BetType, 
                                                              options.SportsType, 
                                                              options.Combos,
                                                              options.Username,
                                                              options.IsCached,
                                                              out roleId);

                    ViewBag.Title = options.GetTitle();
                    ViewBag.FilterOption = options;
                    ViewBag.GrandTotal = model.Total;

                    if (roleId > (int)RoleId.Member)
                    {
                        int page = options.PageIndex ?? 1;
                        ViewBag.Pager = Pager.Items(model.Records.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
                        return this.PartialView("AgentData", model.Records.ToPagedList(page, AppSetting.PageSize));
                    }
                    else
                    {
                        return this.PartialView("MemberData", model);
                    }
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
            int roleId = 0;
            var model = this.memberWinLossService.GetMemberWinLoss(
                                                      options.FromDate,
                                                      options.ToDate,
                                                      options.BetType,
                                                      options.SportsType,
                                                      options.Combos,
                                                      options.Username,
                                                      options.IsCached,
                                                      out roleId);
            var title = options.GetTitle();
            MemoryStream ms = null;

            if (roleId > (int)RoleId.Member)
            {
                ms = ExportAgentWinLossRpt(model, title);
            }
            else
            {
                ms = ExportMemberWinLossRpt(model, title);
            }

            Response.AddHeader("content-disposition", "attachment; filename=" + "AgentMemberWinLoss.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        /// <summary>
        /// Export report data to Excel file in memory stream.
        /// </summary>
        /// <param name="model">Model contains the data of report.</param>
        /// <param name="title">The title of report.</param>
        /// <returns>Memory contains the Excel file.</returns>
        private MemoryStream ExportMemberWinLossRpt(MemberWinLossData model, string title)
        {
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\MemberWinLoss.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            writer.WriteRow(0, new object[] { title });

            // header
            var header1 = new object[]
            {
                ViewLang.Month,
                ViewLang.Ticket,
                string.Empty,
                ViewLang.StakeAfterPT,
                string.Empty,
                ViewLang.WinLoss,
                string.Empty,
                ViewLang.OurPT
            };

            var header2 = new object[]
            {
                string.Empty,
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

            // Footer
            var footer = new object[] 
            {
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
            };

            writer.WriteRows(1, new List<object[]>() { header1, header2 });

            // records
            foreach (var item in model.Records)
            {
                var record = new object[]
                {
                    item.Month,
                    item.Tickets_Won,
                    item.Tickets_Lose,
                    item.Stake_Won,
                    item.Stake_Lose,
                    item.Member_Won,
                    item.Member_Lose,
                    item.OurPT_Won,
                    item.OurPT_Lose,
                    item.OurPT_NetWinloss
                };
                writer.WriteRow(3, record);
            }

            // footer
            writer.WriteRow(4, footer);

            writer.SyncColumnWidths(10);

            return writer.Render("MemberWinLoss");
        }

        /// <summary>
        /// Export report data to Excel file in memory stream.
        /// </summary>
        /// <param name="model">Model contains the data of report.</param>
        /// <param name="title">The title of report.</param>
        /// <returns>Memory contains the Excel file.</returns>
        private MemoryStream ExportAgentWinLossRpt(MemberWinLossData model, string title)
        {
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\AgentWinLoss.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            writer.WriteRow(0, new object[] { title });

            // header
            var header1 = new object[]
            {
                ViewLang.Month,
                ViewLang.Username,
                ViewLang.Ticket,
                string.Empty,
                ViewLang.StakeAfterPT,
                string.Empty,
                ViewLang.WinLoss,
                string.Empty,
                ViewLang.OurPT
            };

            var header2 = new object[]
            {
                string.Empty,
                string.Empty,
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

            // Footer
            var footer = new object[] 
            {
                ViewLang.Total,
                string.Empty,
                model.Total.Tickets_Won,
                model.Total.Tickets_Lose,
                model.Total.Stake_Won,
                model.Total.Stake_Lose,
                model.Total.Member_Won,
                model.Total.Member_Lose,
                model.Total.OurPT_Won,
                model.Total.OurPT_Lose,
                model.Total.OurPT_NetWinloss
            };

            writer.WriteRows(1, new List<object[]>() { header1, header2 });

            // records
            foreach (var item in model.Records)
            {
                if (item.IsSutotalRow)
                {
                    var subtotal = new object[]
                    {
                        ViewLang.Subtotal,
                        string.Empty,
                        item.Tickets_Won,
                        item.Tickets_Lose,
                        item.Stake_Won,
                        item.Stake_Lose,
                        item.Member_Won,
                        item.Member_Lose,
                        item.OurPT_Won,
                        item.OurPT_Lose,
                        item.OurPT_NetWinloss
                    };
                    writer.WriteRow(4, subtotal);
                    writer.WriteCellRange(writer.RowId - item.NumOfSubrows - 1, item.NumOfSubrows, 0, 1, item.Month);
                }
                else
                {
                    var record = new object[]
                    {
                        item.Month,
                        item.Username,
                        item.Tickets_Won,
                        item.Tickets_Lose,
                        item.Stake_Won,
                        item.Stake_Lose,
                        item.Member_Won,
                        item.Member_Lose,
                        item.OurPT_Won,
                        item.OurPT_Lose,
                        item.OurPT_NetWinloss
                    };
                    writer.WriteRow(3, record);
                }
            }
            
            // footer
            writer.WriteRow(5, footer);

            writer.SyncColumnWidths(11);

            return writer.Render("AgentWinLoss");
        }
    }
}