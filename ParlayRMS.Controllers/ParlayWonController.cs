using ParlayRMS.DTO.ParlayWon;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    using ExcelService.Writers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using Services;

    [NapAuth(FunctionKey = FunctionKeys.ParlayWon, Policy = Constant.Policy.Read)]
    public class ParlayWonController : BaseController
    {
        private readonly ParlayWonService _service = new ParlayWonService();
        private readonly CommonService commonService = new CommonService();
        private readonly BetDetailsService _betDetailsService = new BetDetailsService();

        public ActionResult Filter()
        {
            var levels = new List<SelectListItem> { new SelectListItem { Text = ViewLang.None, Value = "0" } };
            for (var i = 1; i <= 10; i++)
            {
                levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.Levels = levels;

            var currenciesList = commonService.GetCurrencies();
            var currencies = new List<SelectListItem>();
            foreach (var currency in currenciesList)
            {
                currencies.Add(new SelectListItem { Text = currency.CurrencyCode, Value = currency.Id.ToString() });
            }
            ViewBag.Currencies = currencies;

            ViewBag.HouseLoseAmountFrom = Formatter.FormatNumber3(AppSetting.HouseLoseAmountFrom);
            ViewBag.HouseLoseAmountTo = Formatter.FormatNumber3(AppSetting.HouseLoseAmountTo);

            return PartialView();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Data(ParlayWonOptions options)
        {
            try
            {
                ModelState.Remove("CustomerClass");
                ModelState.Remove("Ip");
				ModelState.Remove("HouseAmountFrom");
                ModelState.Remove("HouseAmountTo");
                ModelState.Remove("SortById");
                ModelState.Remove("IsSortAsc");

                if (options.FromDate > options.ToDate)
                {
                    ModelState.AddModelError("FromDate", "FromDate must be less than ToDate");
                }

                var lastMonth = DateTime.Now.AddMonths(-1).Date;
                if (options.FromDate.Date < lastMonth)
                {
                    ModelState.AddModelError("FromDate",
                        $"FromDate must be greater or equal than {lastMonth.ToShortDateString()}");
                }

                if (!ModelState.IsValid)
                {
                    return PartialView("_ErrorMessage", GetModelErrorMessage());
                }

                var isValidCustomerClass = !(options.CustomerClass.HasValue && options.CustomerClass.Value > short.MaxValue);
                var totalRows = 0;
                var model = isValidCustomerClass ? _service.GetParlayWonList(options, out totalRows) : new List<RMS_Rpt_Parlay_ParlayWon>();
                ViewBag.FilterOption = options;
                ViewBag.Title = options.GetTitle();
                ViewBag.TotalRows = totalRows;
                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
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
        [ValidateInput(false)]
        public void Excel(ParlayWonOptions options)
        {
            var isValidCustomerClass = !(options.CustomerClass.HasValue && options.CustomerClass.Value > short.MaxValue);
            var records = isValidCustomerClass ? _service.GetParlayWonList(options, out _) : new List<RMS_Rpt_Parlay_ParlayWon>();

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\ParlayWon.xls";
            var writer = new TemplateWriter(templatePath);

            var title = options.GetTitle();
            writer.WriteRow(0, new object[] { title });

            var header1 = new object[]
            {
                ViewLang.No,
                ViewLang.TransTime,
                ViewLang.BetID,
                ViewLang.Site,
                ViewLang.Username,
                ViewLang.CustomerClass,
                ViewLang.Currency,
                ViewLang.Odds,
                ViewLang.Stake,
                ViewLang.StakeAfterPT,
                ViewLang.WinningAmount,
                ViewLang.HouseLoseAmount,
                ViewLang.TotalComboBets,
                ViewLang.TotalSelections,
                ViewLang.SportType,
                ViewLang.IP
            };

            writer.WriteRow(1, header1);

            var id = 0;
            var select = records.Select(x => new
            {
                No = Formatter.FormatNumber3(++id),
                TransTime = Formatter.FormatDateTime(x.TransDate),
                x.TransId,
                Site = x.SiteGroup,
                x.Username,
                x.CustomerClass,
                x.Currency,
                Odds = Formatter.FormatOdds(x.Odds),
                Stake = Formatter.FormatNumber3(x.Stake),
                StakeAfterPT = Formatter.FormatNumber3(x.StakeAfterPT),
                WinningAmount = Formatter.FormatNumber3(x.WinningAmount),
                HouseLoseAmount = Formatter.FormatNumber3(x.HouseLoseAmount),
                x.TotalBet,
                x.TotalSelection,
                TotalBets = CacheRef.GetSportNameDropDown(x.SportType),
                x.IP
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(14);

            var ms = writer.Render("ParlayWon");
            Response.AddHeader("content-disposition", "attachment; filename=" + "ParlayWon.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
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
    }
}
