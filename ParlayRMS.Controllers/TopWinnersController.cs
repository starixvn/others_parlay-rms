using ParlayRMS.DTO.TopWinners;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel;
using ParlayRMS.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using ExcelService.Writers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using Services;

    [NapAuth(FunctionKey = FunctionKeys.TopWinners, Policy = Constant.Policy.Read)]
    public class TopWinnersController : BaseController
    {
        private readonly TopWinnersService _topwinnersService = new TopWinnersService();
        private readonly IUserService _userService = new UserService();
        private readonly BetDetailsService _betDetailsService = new BetDetailsService();
        private readonly CommonService commonService = new CommonService();

        [HttpGet]
        public ActionResult Index()
        {
	        ViewBag.SelectTops = Config.SelectTops;

            var currenciesList = commonService.GetCurrencies();
            var currencies = new List<SelectListItem>();
            foreach (var currency in currenciesList)
            {
                currencies.Add(new SelectListItem { Text = currency.CurrencyCode, Value = currency.Id.ToString() });
            }
            ViewBag.Currencies = currencies;

            ViewBag.HasWritePermission = _userService.IsHavingPermission(
                CurrentSession.UserId,
                NapSettings.Settings.SiteId,
                FunctionKeys.ManageMember,
                Constant.Policy.Host);

            return View();
        }

        [HttpPost]
        public ActionResult Data(TopWinnersOptions options)
        {
            try
            {
                var isValidCustomerClass = !(options.CustomerClass.HasValue && options.CustomerClass.Value > short.MaxValue);
                var records = isValidCustomerClass ? _topwinnersService.GetTopWinners(options) : new List<RMS_Rpt_Parlay_TopWinner>();
                var totalStake = records.Sum(x => x.Stake_BaseRM);
                var totalWinlost = records.Sum(x => x.Winlost_BaseRM);
                var totalCompanyWinlost = records.Sum(x => x.Company_Winlost_BaseRM);
                var totalBets = records.Sum(x => x.BetCount);
                var title = options.GetTile();
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
            ViewBag.BetId = options.TransId;
            ViewBag.Username = options.Username;
            var data = _betDetailsService.GetBetDetail(options.TransId, options.RefNo);

            var model = new BetDetails
            {
                TicketDetail = data.TicketDetail,
                ComboTypes = data.ComboTypes
            };

            return View(model);
        }

        [HttpGet]
        [ValidateInput(false)]
        public void Excel(TopWinnersOptions options)
        {
            var isValidCustomerClass = !(options.CustomerClass.HasValue && options.CustomerClass.Value > short.MaxValue);
            var records = isValidCustomerClass ? _topwinnersService.GetTopWinners(options) : new List<RMS_Rpt_Parlay_TopWinner>();
            var totalStake = records.Sum(x => x.Stake_BaseRM);
            var totalWinlost = records.Sum(x => x.Winlost_BaseRM);
            var totalCompanyWinlost = records.Sum(x => x.Company_Winlost_BaseRM);
            var totalBets = records.Sum(x => x.BetCount);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\TopWinners.xls";
            var writer = new TemplateWriter(templatePath);

            var title = options.GetTile();
            writer.WriteRow(0, new object[] { title });

            var header1 = new object[]
            {
                ViewLang.No,
                ViewLang.Site,
                ViewLang.Username,
                ViewLang.CustomerClass,
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
                string.Empty,
                string.Empty,
                totalStake,
                totalWinlost,
                totalCompanyWinlost,
                totalBets
            };

            writer.WriteRows(1, new List<object[]> { header1, header2 });

            var id = 0;
            var select = records.Select(x => new
            {
                No = ++id,
                Site = x.SiteGroup,
                x.Username,
                x.CustomerClass,
                x.Currency,
                x.Stake,
                x.Winlost,
                x.Company_Winlost,
                x.Stake_BaseRM,
                x.Winlost_BaseRM,
                x.Company_Winlost_BaseRM,
                TotalBets = x.BetCount,
                Avg_Odds = Formatter.FormatOdds(x.Avg_Odds)
            });

            writer.WriteRows(3, select);

            writer.WriteRow(4, footer);

            writer.SyncColumnWidths(11);

            var ms = writer.Render("TopWinner");
            Response.AddHeader("content-disposition", "attachment; filename=" + "TopWinner.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        [HttpGet]
        public ActionResult SingleMatchParlayBetDetails(int custId, long transId, string username)
        {
            ViewBag.TransId = transId;
            ViewBag.Username = username;

            var selectionAndResult = _betDetailsService.GetSelectionAndResult(custId, transId, username);
            var choice = _betDetailsService.GetChoiceForSingleMatchParlay(transId);

            ViewBag.SelectionNames = selectionAndResult.Item1;
            ViewBag.ResultTexts = selectionAndResult.Item2;
            ViewBag.Choice = choice.Item2;
            ViewBag.IsBlackListLeague = choice.Item1;

            return View();
        }

        [HttpGet]
        public ActionResult PenaltyShootoutParlayBetDetails(long transId, string username)
        {
            ViewBag.TransId = transId;
            ViewBag.Username = username;

            var detail = _betDetailsService.GetPenaltyShootoutDetail(transId);
            ViewBag.Choice = detail.Item2;

            ViewBag.IsBlackListLeague = detail.Item1;

            return View();
        }

        [HttpGet]
        public void ExcelForDataDetails(TopWinnerDetailsOptions options)
        {
            var records = _topwinnersService.GetTopWinnerDetails(options);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\TopWinners-DataDetails.xls";
            var writer = new TemplateWriter(templatePath);

            var title = options.GetTile();
            writer.WriteRow(0, new object[] { title });

            var header = new object[]
            {
                ViewLang.No,
                ViewLang.BetID,
                ViewLang.Stake,
                ViewLang.CustomerWinLoss,
                ViewLang.CompanyWinLoss,
                ViewLang.Stake,
                ViewLang.CustomerWinLoss,
                ViewLang.CompanyWinLoss,
                ViewLang.TotalComboBets,
                ViewLang.TotalSelections,
                ViewLang.SportType
            };

            writer.WriteRow(1, header);

            var id = 0;
            var select = records.Select(x => new
            {
                No = ++id,
                BetId = x.TransId.ToString(),
                x.Stake,
                x.Winlost,
                x.Company_Winlost,
                x.Stake_BaseRM,
                x.Winlost_BaseRM,
                x.Company_Winlost_BaseRM,
                TotalComboBets = _topwinnersService.GetTotalComboBets(x),
                TotalSelection = x.Selection.ToString("#,##0"),
                SportType = CacheRef.GetSportName(x.SportType)
            });

            writer.WriteRows(2, select);


            writer.SyncColumnWidths(11);

            var ms = writer.Render("TopWinner-DataDetails");
            Response.AddHeader("content-disposition", "attachment; filename=" + "TopWinner-DataDetails.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}
