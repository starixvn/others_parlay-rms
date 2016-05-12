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
using RemotingRef;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.TopWinners, Policy = UserPolicy.Read)]
    public class TopWinnersController : BaseController
    {
        private readonly ITopWinnersService _topwinnersService = new TopWinnersService();
        private readonly IBetListService _betlistService = new BetListService();
        private readonly IUsersService _userService = new UsersService();

        [HttpGet]
        public ActionResult Index()
        {
            SelectListItem[] tempSelectTop = new SelectListItem[4];
            List<SelectListItem> selectTop = new List<SelectListItem>();
            List<int> moreSelection = new List<int> { 200, 500, 1000 };
            Config.SelectTops.CopyTo(tempSelectTop);
            selectTop.AddRange(tempSelectTop);
            foreach (int option in moreSelection)
            {
                selectTop.Add(new SelectListItem
                {
                    Text = option.ToString(),
                    Value = option.ToString()
                });
            }

            ViewBag.SelectTops = selectTop;
            ViewBag.HasWritePermission = _userService.HasWritePermission(CurrentSession.UserId, FunctionKeys.ManageCustomer);

            return View();
        }

        [HttpPost]
        public ActionResult Data(TopWinnersOptions options)
        {
            try
            {
                var records = _topwinnersService.GetTopWinners(options);
                var totalStake = records.Sum(x => x.Stake_BaseRM);
                var totalWinlost = records.Sum(x => x.Winlost_BaseRM);
                var totalCompanyWinlost = records.Sum(x => x.Company_Winlost_BaseRM);
                var totalBets = records.Sum(x => x.BetCount);
                string title = options.GetTile();
                return Json(new
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
                return Json(new { code = 1, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DataDetails(TopWinnerDetailsOptions options)
        {
            ViewBag.CustId = options.CustId;
            ViewBag.Username = options.Username;
            var records = _topwinnersService.GetTopWinnerDetails(options);
            return PartialView(records);
        }

        [HttpGet]
        public ActionResult BetDetails(BetDetailsOptions options)
        {
            ViewBag.BetId = options.BetId;
            ViewBag.Username = options.Username;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var data = _topwinnersService.GetBetDetail(options.RefNo);
            var tickets = _betlistService.Map(options.RefNo, data);
            var betInfo = _topwinnersService.GetBetInfo(options.BetId);
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

            var model = new BetDetails
            {
                TicketDetail = builder.GetBetList(),
                ComboTypes = RefData.GetCombinationTypeCombo(betInfo.BetTeam, betInfo.BetCheck, betInfo.Hdp1, betInfo.Hdp2, "en")
            };
            return View(model);
        }

        [HttpGet]
        [ValidateInput(false)]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public void Excel(TopWinnersOptions options)
        {
            var records = _topwinnersService.GetTopWinners(options);
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