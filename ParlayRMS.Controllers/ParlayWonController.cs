using ParlayRMS.ExcelService.Writers;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Implement.BetDetails;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.Services.Implement.ParlayWon;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Report;
using ParlayRMS.Web.Authorization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.ParlayWon, Policy = UserPolicy.Read)]
    public class ParlayWonController : BaseController
    {
        private readonly ParlayWonService _service = new ParlayWonService();
        private readonly CommonService commonService = new CommonService();
        private readonly BetDetailsService _betDetailsService = new BetDetailsService();

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public ActionResult Filter()
        {
            // Level
            var levels = new List<SelectListItem>();
            levels.Add(new SelectListItem { Text = ViewLang.None, Value = "0" });
            for (int i = 1; i <= 10; i++)
            {
                levels.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.Levels = levels;

            // Currency
            var currenciesList = commonService.GetCurrencies();
            var currencies = new List<SelectListItem>();
            foreach(var currency in currenciesList)
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
                Hashtable outputParams = null;
                ModelState.Remove("HouseAmountFrom");
                ModelState.Remove("HouseAmountTo");
                ModelState.Remove("SortById");
                ModelState.Remove("IsSortAsc");

                if (options.FromDate > options.ToDate)
                {
                    ModelState.AddModelError("FromDate", "FromDate must be less than ToDate");
                }

                DateTime lastMonth = DateTime.Now.AddMonths(-1).Date;
                if (options.FromDate.Date < lastMonth)
                {
                    ModelState.AddModelError("FromDate", string.Format("FromDate must be greater or equal than {0}", lastMonth.ToShortDateString()));
                }

                if (!ModelState.IsValid)
                {
                    return PartialView("_ErrorMessage", GetModelErrorMessage());
                }
                else
                {
                    var model = _service.GetParlayWonList(options, ref outputParams);
                    ViewBag.FilterOption = options;
                    ViewBag.Title = options.GetTitle();
                    ViewBag.TotalRows = outputParams == null ? 0 : Convert.ToInt32(outputParams["totalrows"]);
                    return PartialView(model);
                }
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
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public void Excel(ParlayWonOptions options)
        {
            Hashtable outputParams = null;
            var records = _service.GetParlayWonList(options, ref outputParams);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\ParlayWon.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            string title = options.GetTitle();
            writer.WriteRow(0, new object[] { title });

            // header
            var header1 = new object[]
            {
                ViewLang.No,
                ViewLang.TransTime,
                ViewLang.BetID,
                ViewLang.Username,
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

            // records
            int id = 0;
            var select = records.Select(x => new
            {
                No = Formatter.FormatNumber3(++id),
                TransTime = Formatter.FormatDate(x.TransDate),
                TransId = x.TransId,
                Username = x.Username,
                Currency = x.Currency,
                Odds = Formatter.FormatOdds(x.Odds),
                Stake = Formatter.FormatNumber3(x.Stake),
                StakeAfterPT = Formatter.FormatNumber3(x.StakeAfterPT),
                WinningAmount = Formatter.FormatNumber3(x.WinningAmount),
                HouseLoseAmount = Formatter.FormatNumber3(x.HouseLoseAmount),
                TotalBet = x.TotalBet,
                TotalSelection = x.TotalSelection,
                TotalBets = CacheRef.GetSportNameDropDown(x.SportType),
                IP = x.IP
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(13);

            MemoryStream ms = writer.Render("ParlayWon");
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
