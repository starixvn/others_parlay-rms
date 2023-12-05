using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using ParlayRMS.DTO.ViewIPInfo;
using ParlayRMS.Models;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.ViewIPInfo;
using System;
using System.Linq;
using System.Web.Mvc;
using ExcelService.Writers;

namespace ParlayRMS.Controllers
{
	using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
	using Services;
	using System.Collections.Generic;

    [NapAuth(FunctionKey = FunctionKeys.ViewIP, Policy = Constant.Policy.Read)]
    public class ViewIPInfoController : BaseController
    {
        private readonly ViewIPInfoService _ipinfoService = new ViewIPInfoService();
        private readonly BetListService _betlistService = new BetListService();
        private readonly CommonService _commonService = new CommonService();
        private const int _pageSize = 100;

        [HttpGet]
        public ActionResult Index()
        {
            var info = _commonService.GetMinTransDate();
            ViewBag.ip = Request["ip"];
            ViewBag.date = Request["date"];
            ViewBag.minDate = info.MinTransdate;
            return View("Index");
        }

        [HttpPost]
        public JsonResult MainReportData(ViewIPInfoOptions options)
        {
            try
            {
                var records = _ipinfoService.GetIPInfo(options.IP, options.Date, options.TicketStatus);

                return Json(new
                {
                    code = 0,
                    Records = records
                });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = ex.Message });
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public void ExcelForIpInfo(ViewIPInfoOptions options)
        {
	        var data = _ipinfoService.GetIPInfo(options.IP, options.Date, options.TicketStatus);

	        var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\IpInfo.xls";
	        var writer = new TemplateWriter(templatePath);
	        writer.WriteRow(0, new object[] { "View IP Info" });

	        var header = new object[]
	        {
		        ViewLang.No,
		        ViewLang.Site,
		        ViewLang.Username,
		        ViewLang.Socceronly,
		        ViewLang.Others,
		        ViewLang.Total,
		        ViewLang.Stake,
		        ViewLang.WinningAmount,
		        ViewLang.HouseLoseAmount,
			};

	        writer.WriteRows(1, new List<object[]> { header });

	        var id = 0;
	        var select = data.Select(x => new
	        {
		        No = ++id,
		        x.SiteGroup,
		        x.Username,
		        SoccerOnly = x.Soccer == 0 ? "-" : x.Soccer.ToString(),
		        Others = x.Others == 0 ? "-" : x.Others.ToString(),
                Total = x.Soccer + x.Others,
				Stake = Formatter.FormatNumber3(x.Stake),
				WinningAmount = Formatter.FormatNumber3(x.WinningAmount),
				HouseLoseAmount = Formatter.FormatNumber3(x.HouseLoseAmount)
			});
	        writer.WriteRows(2, select);

            var total = new RMS_Rpt_Parlay_ViewIPInfo();
            data.ToList().ForEach(x => total.Add(x));
            var footer = new object[]
            {
                ViewLang.Total,
                string.Empty,
                string.Empty,
				total.Soccer,
                total.Others,
                total.Soccer + total.Others,
                Formatter.FormatNumber3(total.Stake),
                Formatter.FormatNumber3(total.WinningAmount),
                Formatter.FormatNumber3(total.HouseLoseAmount)
            };
            writer.WriteRow(3, footer);

            writer.SyncColumnWidths(9);

	        var ms = writer.Render("IpInfo");
	        Response.AddHeader("content-disposition", "attachment; filename=" + "IpInfo.xls");
	        Response.AddHeader("content-type", "application/vnd.ms-excel");
	        Response.BinaryWrite(ms.ToArray());
	        Response.End();
        }

		[HttpPost]
        public ActionResult IpStatsData(ViewIPStatsOptions options)
        {
            var data = _ipinfoService.GetIPStats(
                                                                            options.FromDate,
                                                                            options.PageIndex,
                                                                            options.PageSize,
                                                                            options.TotalItems,
                                                                            options.MaxTransId,
                                                                            options.TicketStatus);
            var page = options.PageIndex;

            ViewBag.StartCounter = (page - 1) * _pageSize;
            ViewBag.Pager = Pager.Items(data.TotalItems).PerPage(_pageSize).Move(page).Segment(5).Center();
            ViewBag.TotalItems = data.TotalItems;
            ViewBag.MaxTransId = data.MaxTransId;
            return PartialView("IpStats", data);
        }

		[HttpGet]
		[ValidateInput(false)]
		public void ExcelForIpStats(ViewIPStatsOptions options)
		{
			var data = _ipinfoService.GetIPStats(
				options.FromDate,
				options.PageIndex,
				options.PageSize,
				options.TotalItems,
				options.MaxTransId,
				options.TicketStatus);

			var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\IpStats.xls";
			var writer = new TemplateWriter(templatePath);

			var title = "IP Statistics";
			writer.WriteRow(0, new object[] { title });

			var header = new object[]
			{
				ViewLang.No,
				ViewLang.IP,
				ViewLang.TotalMembers,
				ViewLang.Stake,
				ViewLang.WinningAmount,
				ViewLang.HouseLoseAmount,
			};

			writer.WriteRows(1, new List<object[]> { header });

			var id = 0;
			var select = data.IPStats.Select(x => new
			{
				No = ++id,
				x.IP,
				x.CntUsers,
				Stake = Formatter.FormatNumber3(x.Stake),
				WinningAmount = Formatter.FormatNumber3(x.WinningAmount),
				HouseLoseAmount = Formatter.FormatNumber3(x.HouseLoseAmount)
			});

			writer.WriteRows(2, select);

			writer.SyncColumnWidths(6);

			var ms = writer.Render("IpStats");
			Response.AddHeader("content-disposition", "attachment; filename=" + "IpStats.xls");
			Response.AddHeader("content-type", "application/vnd.ms-excel");
			Response.BinaryWrite(ms.ToArray());
			Response.End();
		}

		[HttpGet]
        public ActionResult Drilldown(DrillDownOptions model)
        {
            ViewBag.SportType = model.SportType;
            ViewBag.UserName = model.UserName;

            var models = _ipinfoService.GetIPInfoByUser(model.IP, model.Date, model.UserId, model.SportType, model.TicketStatus);
            var singleMatchParlayTickets = models.Where(m => m.BetType == (int)BetType.SingleMatchParlay).ToArray();
            if (singleMatchParlayTickets.Any())
            {
                UpdateChoiceForSingleMatchParlay(model, singleMatchParlayTickets);
            }

            var penaltyShootoutParlayTickets = models.Where(m => m.BetType == (int)BetType.PenaltyShootout).ToArray();
            if (penaltyShootoutParlayTickets.Any())
            {
                UpdateChoiceForPenaltyShootoutParlay(model, penaltyShootoutParlayTickets);
            }

            var gOFParlayTickets = models.Where(m => m.IsGOF).ToArray();
            if (gOFParlayTickets.Any())
            {
                UpdateStakeForGoFParlay(model, gOFParlayTickets);
            }

            return View("Drilldown", models);
        }

        private static void UpdateChoiceForSingleMatchParlay(DrillDownOptions model, RMS_Rpt_Parlay_ViewIPInfoByUser[] singleMatchParlayTickets)
        {
            var index = 0;
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = false };
            var factory = new BetListHTMLFactory();
            foreach (var m in singleMatchParlayTickets)
            {
                if (m.BetType == (int)BetType.SingleMatchParlay)
                {
                    m.Choice = BuildSingleMatchParlayChoice(ticketHelper, factory, m, model.UserId, index);
                    index++;
                }
            }
        }

        private static string BuildSingleMatchParlayChoice(ITicketHelper ticketHelper, BetListHTMLFactory factory, RMS_Rpt_Parlay_ViewIPInfoByUser model, int userId, int index)
        {
            var builder = factory.CreateBuilder();
            var ticket = new Ticket
            {
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                SportTypeId = model.SportTypeId,
                LeagueId = model.LeagueId,
                EventDate = model.EventDate,
                BetTypeId = model.BetType,
                TransId = model.TransID,
                IsNeutral = model.IsNeutral,
                CustId = userId
            };
            index++;
            ticketHelper.Index = index;

            var choice = builder.BuildChoice(ticket, ticketHelper, null, false);
            var html = new HtmlDocument();
            html.LoadHtml(choice);
            var document = html.DocumentNode;
            var eventDateNode = document.QuerySelector(".event-date");
            eventDateNode?.Remove();

            return document.OuterHtml;
        }

        [HttpPost]
        public JsonResult ParlayBetSubTickets(ParlayBetTypeViewModel options)
        {
            try
            {
                var ticketString = BuildTickets(options);

                return Json(new
                {
                    code = 0,
                    data = ticketString
                });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ParlayBetDetails(ParlayBetDetailsOptions options)
        {
            var model = _ipinfoService.GetBetCombinationDetails(options.RefNo);
            model.BetTypeName = options.BetTypeName;
            return View("BetDetails", model);
        }

        private string BuildTickets(ParlayBetTypeViewModel options)
        {
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var data = _betlistService.GetBetListData(options.TransId, options.RefNo);
            var tickets = _betlistService.Map(options.RefNo, data.Tickets);
            var countTicket = 1;
            var openTag = string.Empty;
            var endTag = "<hr class = 'hr'>";
            if (options.BetType == (int)BetType.SystemParlay)
            {
                var comboTypes = CacheRef.GetCombinationTypeCombo(options.BetTeam, options.BetCheck, options.Hdp1, options.Hdp2);
                var filteredTickets = tickets.Item2.Where(x => x.Stake != 0).OrderBy(x => x.MatchId).ToList();
                var count = comboTypes?.Length ?? 0;
                for (var i = 0; i < count && i < filteredTickets.Count; i++)
                {
                    openTag += "<span class=\"underdog\"> " + comboTypes[i] + " x <span class=\"odds\">" + data.CurrencyName + " " + filteredTickets[i].Stake.ToString("#,##0") + "</span></span><br />";
                }
            }
            else
            {
                openTag += "<br />";
            }

            foreach (var ticket in tickets.Item2)
            {
                if (countTicket != 1)
                {
                    openTag = string.Empty;
                }

                if (countTicket == tickets.Item2.Count)
                {
                    endTag = string.Empty;
                }

                builder.AddChoice(ticket, ticketHelper, null, openTag, endTag, false);
                countTicket++;
            }

            return builder.GetBetList();
        }

        private static void UpdateChoiceForPenaltyShootoutParlay(DrillDownOptions model, RMS_Rpt_Parlay_ViewIPInfoByUser[] singleMatchParlayTickets)
        {
            var index = 0;
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = false };
            var factory = new BetListHTMLFactory();
            foreach (var m in singleMatchParlayTickets)
            {
                if (m.BetType == (int)BetType.PenaltyShootout)
                {
                    m.Choice = BuildPenaltyShootoutParlayChoice(ticketHelper, factory, m, model.UserId, index);
                    index++;
                }
            }
        }

        private static string BuildPenaltyShootoutParlayChoice(ITicketHelper ticketHelper, BetListHTMLFactory factory, RMS_Rpt_Parlay_ViewIPInfoByUser model, int userId, int index)
        {
            var builder = factory.CreateBuilder();
            var ticket = new Ticket
            {
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                SportTypeId = model.SportTypeId,
                LeagueId = model.LeagueId,
                EventDate = model.EventDate,
                BetTypeId = model.BetType,
                TransId = model.TransID,
                IsNeutral = model.IsNeutral,
                CustId = userId,
                BetCheck = model.BetCheck,
                BetTeam = model.BetTeam
            };

            index++;
            ticketHelper.Index = index;

            var choice = builder.BuildChoice(ticket, ticketHelper, null, false);
            var html = new HtmlDocument();
            html.LoadHtml(choice);
            var document = html.DocumentNode;
            var eventDateNode = document.QuerySelector(".event-date");
            eventDateNode?.Remove();

            return document.OuterHtml;
        }

        private static void UpdateStakeForGoFParlay(DrillDownOptions model, RMS_Rpt_Parlay_ViewIPInfoByUser[] gOFParlayTickets)
        {
            var index = 0;
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = false };
            var factory = new BetListHTMLFactory();
            foreach (var m in gOFParlayTickets)
            {
                m.StakeString = BuildStakeForGoF(ticketHelper, factory, m, model.UserId, index);
                index++;
            }
        }

        private static string BuildStakeForGoF(ITicketHelper ticketHelper, BetListHTMLFactory factory, RMS_Rpt_Parlay_ViewIPInfoByUser model, int userId, int index)
        {
            var builder = factory.CreateBuilder();
            var ticket = new Ticket
            {
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                SportTypeId = model.SportTypeId,
                LeagueId = model.LeagueId,
                EventDate = model.EventDate,
                BetTypeId = model.BetType,
                TransId = model.TransID,
                IsNeutral = model.IsNeutral,
                CustId = userId,
                BetCheck = model.BetCheck,
                BetTeam = model.BetTeam,
                OddsSpread = model.OddsSpread,
                ExtraInfo = model.ExtraInfo,
                Stake = model.Stake,
                ActualStake = model.ActualStake
            };

            index++;
            ticketHelper.Index = index;

            return builder.BuildStake(ticket, ticketHelper);
        }
    }
}
