
namespace ParlayRMS.Controllers
{
    using ExcelService.Writers;
    using Models.Functions;
    using PagedList;
    using ParlayRMS.DTO.LeagueBetTypeAnalysis;
    using ParlayRMS.DTO.Leagues;
    using Services.Implement.LeagueBetTypeAnalysis;
    using Services.Implement.Leagues;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Utility;
    using ViewModel.Report;
    using Web.Authorization;

    [Authorization(Function = FunctionKeys.LeagueBetTypeDataAnalysis, Policy = UserPolicy.Read)]
    public class LeagueBetTypeAnalysisController : BaseController
    {
        private readonly LeaguesService leagueService = new LeaguesService();
        private readonly LeagueBetTypeAnalysisService service = new LeagueBetTypeAnalysisService();
        private const int SOCCER = 1;

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Filter()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult GroupsAndLeagues(int sportType, DateTime fromDate, DateTime toDate)
        {
            var leagueGroups = service.GetLeagueGroups(sportType);
            List<RMS_LeagueSel> leagueList;
            if (sportType == SOCCER)
            {
                leagueList = leagueService.GetListLeague("", 0, sportType);
            }
            else
            {
                leagueList = service.GetListLeague("", 0, sportType, fromDate, toDate);
            }    
            return Json(new
            {
                Groups = leagueGroups,
                Leagues = leagueList
            });
        }

        [HttpPost]
        public ActionResult Data(LeagueBetTypeAnalysisOptions options)
        {
            try
            {
                var model = service.GetLeagueBetTypeList(options);
                ViewBag.FilterOption = options;
                int page = options.Page <= 0 ? 1 : options.Page;
                ViewBag.FilterOption = options;
                ViewBag.Title = options.GetTitle();
                ViewBag.TotalRows = model.Count;
                ViewBag.Pager = Pager.Items(model.Count).PerPage(options.PageSize).Move(page).Segment(5).Center();
                var pageSizesValues = new List<int> { 100, 200, 500 };
                var pageSizes = new List<SelectListItem>();
                foreach (var item in pageSizesValues)
                {
                    pageSizes.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString(), Selected = item == options.PageSize });
                }

                ViewBag.PageSizes = pageSizes;
                return PartialView(model.ToPagedList(page, options.PageSize));
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ExportExcelForBetType(LeagueBetTypeAnalysisOptions options)
        {
            var records = service.GetLeagueBetTypeList(options);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\BetTypeDataAnalysis.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            string title = ViewLang.LeagueReport;
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
                Turnover = x.TurnOver.ToString("#,##0.00"),
                MemberWinLoss = (Math.Floor(x.MemberWinloss * 100) / 100).ToString("#,##0.00"),
                HouseWinLoss = (Math.Floor(x.MainHouseWinloss * 100) / 100).ToString("#,##0.00")
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(10);

            MemoryStream ms = writer.Render("BetTypeDataAnalysis");
            ms.Seek(0, 0);
            return this.File(ms, "application/vnd.ms-excel", "LeagueBetTypeDataAnalysis.xls");
        }

        [HttpPost]
        public ActionResult ExportExcelForLeague(LeagueBetTypeAnalysisOptions options)
        {
            var records = service.GetLeagueBetTypeList(options);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\LeagueDataAnalysis.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            string title = ViewLang.LeagueReport;
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
                Turnover = x.TurnOver.ToString("#,##0.00"),
                MemberWinLoss = (Math.Floor(x.MemberWinloss * 100) / 100).ToString("#,##0.00"),
                HouseWinLoss = (Math.Floor(x.MainHouseWinloss * 100) / 100).ToString("#,##0.00")
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(11);

            MemoryStream ms = writer.Render("LeagueDataAnalysis");
            ms.Seek(0, 0);
            return this.File(ms, "application/vnd.ms-excel", "LeagueBetTypeDataAnalysis.xls");
        }
    }
}
