using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using ParlayRMS.DTO.Common;
using ParlayRMS.DTO.ViewIPInfo;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Models.ViewIpInfo;
using ParlayRMS.Services.Implement.BetList;
using ParlayRMS.Services.Implement.Common;
using ParlayRMS.Services.Implement.ViewIPInfo;
using ParlayRMS.Services.Interface;
using ParlayRMS.Services.Interface.BetList;
using ParlayRMS.Services.Interface.ViewIPInfo;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Monitoring;
using ParlayRMS.ViewModel.ViewIPInfo;
using ParlayRMS.Web.Authorization;
using RemotingRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ParlayRMS.Controllers
{
    [Authorization(Function = FunctionKeys.ViewIP, Policy = UserPolicy.Read)]
    public class ViewIPInfoController : BaseController
    {
        private readonly IViewIPInfoService _ipinfoService = new ViewIPInfoService();
        private readonly IBetListService _betlistService = new BetListService();
        private readonly ICommonService _commonService = new CommonService();
        private int _pageSize = 100;

        [HttpGet]
        public ActionResult Index()
        {
            RMS_Rpt_Parlay_SelMinTransdate info = _commonService.GetMinTransDate();
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
                IEnumerable<RMS_Rpt_Parlay_ViewIPInfo> records = _ipinfoService.GetIPInfo(options.IP, options.Date);

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
            RMS_Rpt_Parlay_UsersStatisticsByIP data = _ipinfoService.GetIPStats(
                                                                            options.FromDate,
                                                                            options.PageIndex,
                                                                            options.PageSize,
                                                                            options.TotalItems,
                                                                            options.MaxTransId);
            int page = options.PageIndex;

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

            IEnumerable<RMS_Rpt_Parlay_ViewIPInfoByUser> models = _ipinfoService.GetIPInfoByUser(model.IP, model.Date, model.UserId, model.SportType);

            return View("Drilldown", models);
        }

        [HttpPost]
        public JsonResult ParlayBetSubTickets(ParlayBetTypeViewModel options)
        {
            try
            {
                string ticketString = BuildTickets(options);

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
            CombinationDetailsByDateViews model = _ipinfoService.GetBetCombinationDetails(options.RefNo);
            return View("BetDetails", model);
        }

        private string BuildTickets(ParlayBetTypeViewModel options)
        {
            var factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };
            var data = _betlistService.GetBetListData(options.RefNo);
            var tickets = _betlistService.Map(options.RefNo, data.Tickets);
            int countTicket = 1;
            string openTag = string.Empty;
            string endTag = "<hr class = 'hr'>";
            if (options.BetType == (int)BetType.SystemParlay)
            {
                var comboTypes = RefData.GetCombinationTypeCombo(options.BetTeam, options.BetCheck, options.Hdp1, options.Hdp2, "en");
                var filteredTickets = tickets.Where(x => x.Stake != 0).OrderBy(x => x.MatchId).ToList();
                var count = comboTypes == null ? 0 : comboTypes.Length;
                for (var i = 0; i < count; i++)
                {
                    openTag += "<span class=\"underdog\"> " + comboTypes[i] + " x <span class=\"odds\">" + data.CurrencyName + " " + filteredTickets[i].Stake.ToString("#,##0") + "</span></span><br />";
                }
            }
            else
            {
                openTag += "<br />";
            }

            foreach (var ticket in tickets)
            {
                if (countTicket != 1)
                {
                    openTag = string.Empty;
                }

                if (countTicket == tickets.Count())
                {
                    endTag = string.Empty;
                }

                builder.AddChoice(ticket, ticketHelper, null, openTag, endTag, false);
                countTicket++;
            }

            return builder.GetBetList();
        }
    }
}