// --------------------------------------------------------------------------
// <copyright file="TopWinnersController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Services.Implement.TopWinners;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Interface.TopWinners;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel;
    using ParlayRMS.ViewModel.Report;
    using ParlayRMS.Web.Authorization;

    /// <summary>
    /// Top Winners Controller.
    /// </summary>
    public class TopWinnersController : BaseController
    {
        private readonly ITopWinnersService _topwinnersService = new TopWinnersService();
        private readonly IBetListService _betlistService = new BetListService();
        private readonly IUsersService _userService = new UsersService();

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.SelectTops = Config.SelectTops;
            ViewBag.DateRanges = Config.DateRanges;
            ViewBag.SportsTypes = Config.SportsTypes;
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageCustomer);

            return this.View("Index");
        }

        /// <summary>
        /// Gets top winners.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>JSON result represents top winners data.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult Data(TopWinnersOptions options)
        {
            try
            {
                var records = this._topwinnersService.GetTopWinners(options.FromDate, options.ToDate, options.SportsType, options.SelectTop, options.Combos);
                var totalStake = records.Sum(x => x.Stake_BaseRM);
                var totalWinlost = records.Sum(x => x.Winlost_BaseRM);
                var totalCompanyWinlost = records.Sum(x => x.Company_Winlost_BaseRM);
                var totalBets = records.Sum(x => x.BetCount);
                string title = options.GetTile();
                return this.Json(new
                {
                    code = 0,
                    Records = records,
                    TotalStake = totalStake,
                    TotalWinlost = totalWinlost,
                    TotalCompanyWinlost = totalCompanyWinlost,
                    TotalBet = totalBets,
                    Title = title
                });
            }
            catch (Exception ex)
            {
                return this.Json(new { code = 1, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets top winners' details.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>Partial view represents the details of top winners.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = UserPolicy.Read)]
        [HttpPost]
        public ActionResult DataDetails(TopWinnerDetailsOptions options)
        {
            ViewBag.CustId = options.CustId;
            ViewBag.Username = options.Username;
            var records = this._topwinnersService.GetTopWinnerDetails(options.CustId, options.FromDate, options.ToDate, options.SportsType, options.Combos);
            return this.PartialView("DataDetails", records);
        }

        /// <summary>
        /// Gets detail of a bet.
        /// </summary>
        /// <param name="options">The options that are used to query data.</param>
        /// <returns>View represents the detail of a bet.</returns>
        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = UserPolicy.Read)]
        [HttpGet]
        public ActionResult BetDetail(BetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var data = this._topwinnersService.GetBetDetail(options.RefNo);
            var tickets = BetListService.Map(options.RefNo, data);
            var stringBuilder = new StringBuilder();
            foreach (var ticket in tickets)
            {
                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(
                        ticket, 
                        ticketHelper, 
                        null,
                        string.Format("<div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divstatus'>[{0}]</div></div></div>", ticket.Status),
                        false);
                }
                else
                {
                    builder.AddChoice(
                        ticket, 
                        ticketHelper, 
                        null, 
                        string.Format("<div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divstatus'>[{0}]</div></div></div><div class='line'></div>", ticket.Status),
                        false);
                }
            }

            var model = new BetDetails();
            model.TicketDetail = builder.GetBetList();
            return this.View("BetDetails", model);
        }

        /// <summary>
        /// Export top winners to Excel file.
        /// </summary>
        /// <param name="options">The options that are used to filter data.</param>
        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = UserPolicy.Read)]
        [HttpGet]
        [ValidateInput(false)]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public void Excel(TopWinnersOptions options)
        {
            var records = this._topwinnersService.GetTopWinners(options.FromDate, options.ToDate, options.SportsType, options.SelectTop, options.Combos);
            var totalStake = records.Sum(x => x.Stake_BaseRM);
            var totalWinlost = records.Sum(x => x.Winlost_BaseRM);
            var totalCompanyWinlost = records.Sum(x => x.Company_Winlost_BaseRM);
            var totalBets = records.Sum(x => x.BetCount);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\TopWinners.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            string title = options.GetTile();
            writer.WriteRow(0, new object[] { title });

            // header
            var header1 = new object[]
            {
                "#",
                ViewLang.Username,
                ViewLang.Currency,
                ViewLang.OriginalCurrency,
                string.Empty,
                string.Empty,
                ViewLang.BaseCurrency,
                string.Empty,
                string.Empty,
                ViewLang.TotalBets,
                ViewLang.AverageWinOdds
            };

            var header2 = new object[]
            {
                string.Empty,
                string.Empty,
                string.Empty,
                ViewLang.Stake,
                ViewLang.WinningAmount,
                ViewLang.CompanyLoseAmount,
                ViewLang.Stake,
                ViewLang.WinningAmount,
                ViewLang.CompanyLoseAmount
            };

            var footer = new object[] 
            { 
                ViewLang.Total, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                totalStake, 
                totalWinlost, 
                totalCompanyWinlost, 
                totalBets 
            };

            writer.WriteRows(1, new List<object[]>() { header1, header2 });

            // records
            int id = 0;
            var select = records.Select(x => new
            {
                No = ++id,
                Username = x.Username,
                Currency = x.Currency,
                Stake = x.Stake,
                Winlost = x.Winlost,
                Company_Winlost = x.Company_Winlost,
                Stake_BaseRM = x.Stake_BaseRM,
                Winlost_BaseRM = x.Winlost_BaseRM,
                Company_Winlost_BaseRM = x.Company_Winlost_BaseRM,
                TotalBets = x.BetCount,
                Avg_Odds = x.Avg_Odds
            });

            writer.WriteRows(3, select);

            // footer
            writer.WriteRow(4, footer);

            writer.SyncColumnWidths(11);

            MemoryStream ms = writer.Render("TopWinner");
            Response.AddHeader("content-disposition", "attachment; filename=" + "TopWinner.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}