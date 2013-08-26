// -----------------------------------------------------------------------
// <copyright file="TopWinnersController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using ParlayRMS.Common.Models;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.BetList;
    using ParlayRMS.Services.Implement.TopWinners;
    using ParlayRMS.Services.Implement.Users;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Interface.TopWinners;
    using ParlayRMS.Services.Interface.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Report;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.ViewModel;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TopWinnersController : BaseController
    {
        private readonly ITopWinnersService topwinnersService = new TopWinnersService();
        private readonly IBetListService betlistService = new BetListService();
        private readonly IUsersService userService = new UsersService();

        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = ePolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.SelectTops = Config.SelectTops;
            ViewBag.DateRanges = Config.DateRanges;
            ViewBag.SportsTypes = Config.SportsTypes;
            ViewBag.HasWritePermission = userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.MemberManagement);

            return this.View("Index");
        }

        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult Data(TopWinnersOptions options)
        {
            try
            {
                var records = this.topwinnersService.GetTopWinners(options.FromDate, options.ToDate, options.SportsType, options.SelectTop, options.Combos);
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

        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult DataDetails(TopWinnerDetailsOptions options)
        {
            ViewBag.CustId = options.CustId;
            ViewBag.Username = options.Username;
            var records = this.topwinnersService.GetTopWinnerDetails(options.CustId, options.FromDate, options.ToDate, options.SportsType, options.Combos);
            return this.PartialView("DataDetails", records);
        }

        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = ePolicy.Read)]
        [HttpGet]
        public ActionResult BetDetail(BetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var data = this.topwinnersService.GetBetDetail(options.RefNo);
            var tickets = BetListService.Map(options.RefNo, data);
            var stringBuilder = new StringBuilder();
            foreach (var ticket in tickets)
            {
                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div>", false);
                }
                else
                {
                    builder.AddChoice(ticket, ticketHelper, null, "<div id='" + ticket.BetId + "'><div class='ticketDetail'>", "</div><div class='ticketStatus'><div class='divstatus'>[" + ticket.Status + "]</div></div></div><div class='line'></div>", false);
                }
            }

            var model = new BetDetails();
            model.TicketDetail = builder.GetBetList();
            return this.View("BetDetails", model);
        }

        [AuthorizationAttribute(Function = FunctionKeys.TopWinners, Policy = ePolicy.Read)]
        [HttpGet]
        [ValidateInput(false)]
        public void Excel(TopWinnersOptions options)
        {
            var records = this.topwinnersService.GetTopWinners(options.FromDate, options.ToDate, options.SportsType, options.SelectTop, options.Combos);
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
            var header1 = new object[]{
                "#",
                ViewLang.Username,
                ViewLang.Currency,
                ViewLang.OriginalCurrency,
                "",
                "",
                ViewLang.BaseCurrency,
                "",
                "",
                ViewLang.TotalBets,
                ViewLang.AverageWinOdds
            };

            var header2 = new object[]{
                "",
                "",
                "",
                ViewLang.Stake,
                ViewLang.WinningAmount,
                ViewLang.CompanyLoseAmount,
                ViewLang.Stake,
                ViewLang.WinningAmount,
                ViewLang.CompanyLoseAmount
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
            writer.WriteRow(4, new object[] { ViewLang.Total, "", "", "", "", "", totalStake, totalWinlost, totalCompanyWinlost, totalBets });

            writer.SyncColumnWidths(11);

            MemoryStream ms = writer.Render("TopWinner");
            Response.AddHeader("content-disposition", "attachment; filename=" + "TopWinner.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}