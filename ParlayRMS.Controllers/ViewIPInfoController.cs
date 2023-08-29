using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using ParlayRMS.DTO.ViewIPInfo;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.BetList;
using ParlayRMS.Services.Common;
using ParlayRMS.Services.ViewIPInfo;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.ViewIPInfo;
using ParlayRMS.Web.Authorization;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.ViewIP, Policy = UserPolicy.Read)]
    public class ViewIPInfoController : BaseController
    {
        private readonly ViewIPInfoService _ipinfoService = new ViewIPInfoService();
        private readonly BetListService _betlistService = new BetListService();
        private readonly CommonService _commonService = new CommonService();
        private readonly int _pageSize = 100;

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
                var records = _ipinfoService.GetIPInfo(options.IP, options.Date);

                return Json(new
                {
                    code = 0,
                    Records = records,
                });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult IpStatsData(ViewIPStatsOptions options)
        {
            var data = _ipinfoService.GetIPStats(
                                                                            options.FromDate,
                                                                            options.PageIndex,
                                                                            options.PageSize,
                                                                            options.TotalItems,
                                                                            options.MaxTransId);
            var page = options.PageIndex;

            ViewBag.StartCounter = (page - 1) * _pageSize;
            ViewBag.Pager = Pager.Items(data.TotalItems).PerPage(_pageSize).Move(page).Segment(5).Center();
            ViewBag.TotalItems = data.TotalItems;
            ViewBag.MaxTransId = data.MaxTransId;
            return PartialView("IpStats", data);
        }

        [HttpGet]
        public ActionResult Drilldown(DrillDownOptions model)
        {
            ViewBag.SportType = model.SportType;
            ViewBag.UserName = model.UserName;

            var models = _ipinfoService.GetIPInfoByUser(model.IP, model.Date, model.UserId, model.SportType);
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
                CustId = userId,
            };
            index++;
            ticketHelper.Index = index;

            var choice = builder.BuildChoice(ticket, ticketHelper, null, false);
            var html = new HtmlDocument();
            html.LoadHtml(choice);
            var document = html.DocumentNode;
            var eventDateNode = document.QuerySelector(".event-date");
            if (eventDateNode != null)
            {
                eventDateNode.Remove();
            }

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
                var count = comboTypes == null ? 0 : comboTypes.Length;
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
            if (eventDateNode != null)
            {
                eventDateNode.Remove();
            }

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
