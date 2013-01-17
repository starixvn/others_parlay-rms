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
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Implement.TopWinners;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.TopWinners;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Report;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Services.Interface.BetList;
    using ParlayRMS.Services.Implement.BetList;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TopWinnersController : BaseController
    {
        private readonly ITopWinnersService topwinnersService = new TopWinnersService();
        private readonly IBetListService betlistService = new BetListService();

        [AuthorizationAttribute(Function = eFunction.TopWinners, Policy = ePolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            // Select Top
            var values = new List<int> { 10, 20, 50, 100 };
            var selectTops = new List<SelectListItem>();
            foreach (var item in values)
            {
                selectTops.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString(), Selected = item == 100 ? true : false });
            }

            ViewBag.SelectTops = selectTops;

            // Date Range
            var dateRanges = new List<SelectListItem>();
            dateRanges.Add(new SelectListItem { Text = ViewLang.Yesterday, Value = ((int)DateRange.Yesterday).ToString() });
            dateRanges.Add(new SelectListItem { Text = ViewLang.CurrentWeek, Value = ((int)DateRange.CurrentWeek).ToString() });
            dateRanges.Add(new SelectListItem { Text = ViewLang.CurrentMonth, Value = ((int)DateRange.CurrentMonth).ToString() });
            dateRanges.Add(new SelectListItem { Text = ViewLang.LastWeek, Value = ((int)DateRange.LastWeek).ToString() });
            dateRanges.Add(new SelectListItem { Text = ViewLang.LastMonth, Value = ((int)DateRange.LastMonth).ToString() });
            dateRanges.Add(new SelectListItem { Text = ViewLang.SinceLastMonth, Value = ((int)DateRange.SinceLastMonth).ToString() });
            dateRanges.Add(new SelectListItem { Text = ViewLang.SpecificDate, Value = ((int)DateRange.SpecificDate).ToString() });
            ViewBag.DateRanges = dateRanges;

            // Sports Type
            var sportsTypes = new List<SelectListItem>();
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Allsports, Value = ((int)SportsType.AllSports).ToString(), Selected = true });
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Socceronly, Value = ((int)SportsType.SoccerOnly).ToString() });
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Combinedsports, Value = ((int)SportsType.CombinedSports).ToString() });
            sportsTypes.Add(new SelectListItem { Text = ViewLang.Othersports, Value = ((int)SportsType.OtherSports).ToString() });
            ViewBag.SportsTypes = sportsTypes;

            return this.View("Index");
        }

        [AuthorizationAttribute(Function = eFunction.TopWinners, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult Data(TopWinnersOptions options)
        {
            try
            {
                var records = this.topwinnersService.GetTopWinners(options.FromDate, options.ToDate, options.SportsType, options.SelectTop);
                var totalStake = records.Sum(x => x.Stake_BaseRM);
                var totalWinlost = records.Sum(x => x.Winlost_BaseRM);
                var totalCompanyWinlost = records.Sum(x => x.Company_Winlost_BaseRM);
                var totalBets = records.Sum(x => x.BetCount);
                string title = string.Format("Top {0} winners - from {1} to {2} - {3}",
                    options.SelectTop,
                    options.FromDate.ToString("MMMM dd, yyyy"),
                    options.ToDate.ToString("MMMM dd, yyyy"),
                    ModelsHelper.GetSportTypeById(options.SportsType));

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

        [AuthorizationAttribute(Function = eFunction.TopWinners, Policy = ePolicy.Read)]
        [HttpPost]
        public ActionResult DataDetails(TopWinnerDetailsOptions options)
        {
            ViewBag.CustId = options.CustId;
            ViewBag.Username = options.Username;
            var records = this.topwinnersService.GetTopWinnerDetails(options.CustId, options.FromDate, options.ToDate, options.SportsType);
            return this.PartialView("DataDetails", records);
        }

        [AuthorizationAttribute(Function = eFunction.TopWinners, Policy = ePolicy.Read)]
        [HttpGet]
        [ValidateInput(false)]
        public void Excel(TopWinnersOptions options)
        {
            var records = this.topwinnersService.GetTopWinners(options.FromDate, options.ToDate, options.SportsType, options.SelectTop);
            var totalStake = records.Sum(x => x.Stake_BaseRM);
            var totalWinlost = records.Sum(x => x.Winlost_BaseRM);
            var totalCompanyWinlost = records.Sum(x => x.Company_Winlost_BaseRM);
            var totalBets = records.Sum(x => x.BetCount);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\TopWinners.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            string title = string.Format("Top {0} winners - from {1} to {2} - {3}",
                    options.SelectTop,
                    options.FromDate.ToString("MMMM dd, yyyy"),
                    options.ToDate.ToString("MMMM dd, yyyy"),
                    ModelsHelper.GetSportTypeById(options.SportsType));
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

            MemoryStream ms = writer.Render("TopWinners");
            Response.AddHeader("content-disposition", "attachment; filename=" + "TopWinners.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}