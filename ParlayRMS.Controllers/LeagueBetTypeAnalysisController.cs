
namespace ParlayRMS.Controllers
{
    using Models.Functions;
    using Services.Implement.LeagueBetTypeAnalysis;
    using Services.Implement.Leagues;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ViewModel.Report;
    using Web.Authorization;
    using Utility;
    using PagedList;
    using System.Diagnostics.CodeAnalysis;
    using ExcelService.Writers;
    using System.Linq;
    using System.IO;

    [Authorization(Function = FunctionKeys.LeagueBetTypeDataAnalysis, Policy = UserPolicy.Read)]
    public class LeagueBetTypeAnalysisController : BaseController
    {
        private readonly LeaguesService leagueService = new LeaguesService();
        private readonly LeagueBetTypeAnalysisService service = new LeagueBetTypeAnalysisService();
        private readonly int pageSize = AppSetting.PageSize;

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Filter()
        {
            var leagueList = this.leagueService.GetListLeague("", 0);
            var leaguesListItem = new List<SelectListItem>();
            foreach (var item in leagueList)
            {
                leaguesListItem.Add(new SelectListItem { Text = item.LeagueName, Value = item.LeagueId.ToString() });
            }
            ViewBag.Leagues = leaguesListItem;

            return PartialView();
        }

        [HttpPost]
        public ActionResult Data(LeagueBetTypeAnalysisOptions options)
        {
            try
            {
                if (options.FromDate > options.ToDate)
                {
                    ModelState.AddModelError("FromDate", "FromDate must be less than ToDate");
                }

                if (!ModelState.IsValid)
                {
                    return PartialView("_ErrorMessage", GetModelErrorMessage());
                }
                else
                {
                    var model = service.GetLeagueBetTypeList(options);
                    ViewBag.FilterOption = options;
                    int page = options.Page <= 0 ? 1 : options.Page;
                    ViewBag.FilterOption = options;
                    ViewBag.TotalRows = model.Count;
                    ViewBag.Pager = Pager.Items(model.Count).PerPage(pageSize).Move(page).Segment(5).Center();
                    return PartialView(model.ToPagedList(page, pageSize));
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public void ExportExcelForBetType(LeagueBetTypeAnalysisOptions options)
        {
            var model = service.GetLeagueBetTypeList(options);
            int page = options.Page <= 0 ? 1 : options.Page;
            var records = model.ToPagedList(page, pageSize);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\BetTypeDataAnalysis.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            string title = "League & Bet Type Data Analysis (Soccer)";
            writer.WriteRow(0, new object[] { title });

            // header
            var header = new object[]
            {
                ViewLang.No,
                ViewLang.Bettype,
                ViewLang.TotalTickets,
                ViewLang.WonTickets,
                ViewLang.DrawTickets,
                ViewLang.Ratio,
                ViewLang.ParlayWonTickets,
                ViewLang.Turnover,
                ViewLang.MemberWinLoss,
                ViewLang.HouseWinLoss
            };

            writer.WriteRow(1, header);

            // records
            var select = records.Select(x => new
            {
                No = x.No,
                Bettype = x.Bettype,
                TotalTickets = Formatter.FormatNumber3(x.TotalTickets),
                WonTickets = Formatter.FormatNumber3(x.WonTickets),
                DrawTickets = Formatter.FormatNumber3(x.DrawTickets),
                Ratio = x.Ratio,
                ParlayWonTickets = Formatter.FormatNumber3(x.ParlayWonTickets),
                Turnover = Formatter.FormatNumber3(x.TurnOver),
                MemberWinLoss = Formatter.FormatNumber3(x.MemberWinloss),
                HouseWinLoss = Formatter.FormatNumber3(x.MainHouseWinloss)
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(10);

            MemoryStream ms = writer.Render("BetTypeDataAnalysis");
            Response.AddHeader("content-disposition", "attachment; filename=" + "BetTypeDataAnalysis.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        [HttpGet]
        [ValidateInput(false)]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public void ExportExcelForLeague(LeagueBetTypeAnalysisOptions options)
        {
            var model = service.GetLeagueBetTypeList(options);
            int page = options.Page <= 0 ? 1 : options.Page;
            var records = model.ToPagedList(page, pageSize);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\LeagueDataAnalysis.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            string title = "League & Bet Type Data Analysis (Soccer)";
            writer.WriteRow(0, new object[] { title });

            // header
            var header = new object[]
            {
                ViewLang.No,
                ViewLang.Group,
                ViewLang.LeagueName,
                ViewLang.TotalTickets,
                ViewLang.WonTickets,
                ViewLang.DrawTickets,
                ViewLang.Ratio,
                ViewLang.ParlayWonTickets,
                ViewLang.Turnover,
                ViewLang.MemberWinLoss,
                ViewLang.HouseWinLoss
            };

            writer.WriteRow(1, header);

            // records
            var select = records.Select(x => new
            {
                No = x.No,
                Group = x.GroupName,
                League = x.League,
                TotalTickets = Formatter.FormatNumber3(x.TotalTickets),
                WonTickets = Formatter.FormatNumber3(x.WonTickets),
                DrawTickets = Formatter.FormatNumber3(x.DrawTickets),
                Ratio = x.Ratio,
                ParlayWonTickets = Formatter.FormatNumber3(x.ParlayWonTickets),
                Turnover = Formatter.FormatNumber3(x.TurnOver),
                MemberWinLoss = Formatter.FormatNumber3(x.MemberWinloss),
                HouseWinLoss = Formatter.FormatNumber3(x.MainHouseWinloss)
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(11);

            MemoryStream ms = writer.Render("LeagueDataAnalysis");
            Response.AddHeader("content-disposition", "attachment; filename=" + "LeagueDataAnalysis.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}
