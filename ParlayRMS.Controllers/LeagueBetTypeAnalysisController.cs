namespace ParlayRMS.Controllers
{
	using ExcelService.Writers;
	using Fanex.NAP.Core.Business;
	using Fanex.NAP.Core.Web.Attribute;
	using Models.Functions;
	using PagedList;
	using Services;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using Utility;
	using ParlayRMS.ViewModel.LeagueBetTypeAnalysis;

	[NapAuth(FunctionKey = FunctionKeys.LeagueBetTypeDataAnalysis, Policy = Constant.Policy.Read)]
    public class LeagueBetTypeAnalysisController : BaseController
    {
        private readonly LeaguesService leagueService = new LeaguesService();
        private readonly LeagueBetTypeAnalysisService service = new LeagueBetTypeAnalysisService();
        private const int SOCCER = 1;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Filter()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult GroupsAndLeagues(int sportType, DateTime fromDate, DateTime toDate)
        {
            var leagueGroups = service.GetLeagueGroups(sportType);
            var leagueList = sportType == SOCCER ? leagueService.GetListLeague("", 0, sportType) : service.GetListLeague("", 0, sportType, fromDate, toDate);    
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
                var page = options.Page <= 0 ? 1 : options.Page;
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
        public ActionResult LeagueDetail(LeagueDetailOptions options)
        {
	        try
	        {
		        var model = service.GetLeagueDetails(options);
		        ViewBag.FilterOption = options;
		        var page = options.Page <= 0 ? 1 : options.Page;
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
        public ActionResult BetTypeDetail(BetTypeDetailOptions options)
        {
	        try
	        {
		        var model = service.GetBetTypeDetails(options);
		        ViewBag.FilterOption = options;
		        var page = options.Page <= 0 ? 1 : options.Page;
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

            var title = ViewLang.LeagueReport;
            writer.WriteRow(0, new object[] { title });

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

            var select = records.Select(x => new
            {
                x.No,
                x.Bettype,
                TotalTickets = Formatter.FormatNumber3(x.TotalTickets),
                WonTickets = Formatter.FormatNumber3(x.WonTickets),
                DrawTickets = Formatter.FormatNumber3(x.DrawTickets),
                x.Ratio,
                ParlayWonTickets = Formatter.FormatNumber3(x.ParlayWonTickets),
                Turnover = x.TurnOver.ToString("#,##0.00"),
                MemberWinLoss = (Math.Floor(x.MemberWinloss * 100) / 100).ToString("#,##0.00"),
                HouseWinLoss = (Math.Floor(x.MainHouseWinloss * 100) / 100).ToString("#,##0.00")
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(10);

            var ms = writer.Render("BetTypeDataAnalysis");
            ms.Seek(0, 0);
            return File(ms, "application/vnd.ms-excel", "LeagueBetTypeDataAnalysis.xls");
        }

        [HttpPost]
        public ActionResult ExportExcelForLeague(LeagueBetTypeAnalysisOptions options)
        {
            var records = service.GetLeagueBetTypeList(options);

            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\LeagueDataAnalysis.xls";
            var writer = new TemplateWriter(templatePath);

            var title = ViewLang.LeagueReport;
            writer.WriteRow(0, new object[] { title });

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

            var select = records.Select(x => new
            {
                x.No,
                Group = x.GroupName,
                x.League,
                TotalTickets = Formatter.FormatNumber3(x.TotalTickets),
                WonTickets = Formatter.FormatNumber3(x.WonTickets),
                DrawTickets = Formatter.FormatNumber3(x.DrawTickets),
                x.Ratio,
                ParlayWonTickets = Formatter.FormatNumber3(x.ParlayWonTickets),
                Turnover = x.TurnOver.ToString("#,##0.00"),
                MemberWinLoss = (Math.Floor(x.MemberWinloss * 100) / 100).ToString("#,##0.00"),
                HouseWinLoss = (Math.Floor(x.MainHouseWinloss * 100) / 100).ToString("#,##0.00")
            });

            writer.WriteRows(2, select);

            writer.SyncColumnWidths(11);

            var ms = writer.Render("LeagueDataAnalysis");
            ms.Seek(0, 0);
            return File(ms, "application/vnd.ms-excel", "LeagueBetTypeDataAnalysis.xls");
        }

        [HttpPost]
        public ActionResult ExportExcelForLeagueDetail(LeagueDetailOptions options)
        {
	        var records = service.GetLeagueDetails(options);

	        var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\LeagueDetail.xls";
	        var writer = new TemplateWriter(templatePath);

	        var title = ViewLang.LeagueReport;
	        writer.WriteRow(0, new object[] { title });

	        var header = new object[]
	        {
		        ViewLang.No,
		        ViewLang.Event,
		        ViewLang.Bettype,
		        ViewLang.BetChoice,
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

	        var select = records.Select(x => new
	        {
		        x.No,
		        x.Event,
		        x.Bettype,
		        x.BetChoice,
				TotalTickets = Formatter.FormatNumber3(x.TotalTickets),
		        WonTickets = Formatter.FormatNumber3(x.WonTickets),
		        DrawTickets = Formatter.FormatNumber3(x.DrawTickets),
		        x.Ratio,
		        ParlayWonTickets = Formatter.FormatNumber3(x.ParlayWonTickets),
		        Turnover = x.TurnOver.ToString("#,##0.00"),
		        MemberWinLoss = (Math.Floor(x.MemberWinloss * 100) / 100).ToString("#,##0.00"),
		        HouseWinLoss = (Math.Floor(x.MainHouseWinloss * 100) / 100).ToString("#,##0.00")
	        });

	        writer.WriteRows(2, select);

	        writer.SyncColumnWidths(12);

	        var ms = writer.Render("LeagueDetail");
	        ms.Seek(0, 0);
	        return File(ms, "application/vnd.ms-excel", "LeagueDetail.xls");
        }

        [HttpPost]
        public ActionResult ExportExcelForBetTypeDetail(BetTypeDetailOptions options)
        {
	        var records = service.GetBetTypeDetails(options);

	        var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\BetTypeDetail.xls";
	        var writer = new TemplateWriter(templatePath);

	        var title = ViewLang.LeagueReport;
	        writer.WriteRow(0, new object[] { title });

	        var header = new object[]
	        {
		        ViewLang.No,
		        ViewLang.GroupName,
		        ViewLang.LeagueName,
		        ViewLang.Event,
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

	        var select = records.Select(x => new
	        {
		        x.No,
		        x.GroupName,
		        x.League,
		        x.Event,
		        TotalTickets = Formatter.FormatNumber3(x.TotalTickets),
		        WonTickets = Formatter.FormatNumber3(x.WonTickets),
		        DrawTickets = Formatter.FormatNumber3(x.DrawTickets),
		        x.Ratio,
		        ParlayWonTickets = Formatter.FormatNumber3(x.ParlayWonTickets),
		        Turnover = x.TurnOver.ToString("#,##0.00"),
		        MemberWinLoss = (Math.Floor(x.MemberWinloss * 100) / 100).ToString("#,##0.00"),
		        HouseWinLoss = (Math.Floor(x.MainHouseWinloss * 100) / 100).ToString("#,##0.00")
	        });

	        writer.WriteRows(2, select);

	        writer.SyncColumnWidths(12);

	        var ms = writer.Render("BetTypeDetail");
	        ms.Seek(0, 0);
	        return File(ms, "application/vnd.ms-excel", "BetTypeDetail.xls");
        }
	}
}
