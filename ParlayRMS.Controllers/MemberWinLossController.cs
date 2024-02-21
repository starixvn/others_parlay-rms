using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using ParlayRMS.Models;
using ParlayRMS.Models.MemberWinLoss;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Report;

namespace ParlayRMS.Controllers
{
    using ExcelService.Writers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Models.Functions;
    using Services;

    [NapAuth(FunctionKey = FunctionKeys.MemberWinLoss, Policy = Constant.Policy.Read)]
    public class MemberWinLossController : BaseController
    {
        private readonly MemberWinLossService _memberWinLossService = new MemberWinLossService();
        private readonly BetDetailsService _betDetailsService = new BetDetailsService();
        private readonly MembersService _membersService = new MembersService();

		[HttpGet]
        public ActionResult Index()
        {
            ViewBag.LimitDate = MemberWinLossOptions.GetLimitDate();
            return View();
        }

        [HttpPost]
        public ActionResult Data(MemberWinLossOptions options)
        {
            try
            {
                var limitDate = MemberWinLossOptions.GetLimitDate();

                ModelState.Remove("IsSortAsc");

                if (options.FromDate > options.ToDate)
                {
                    ModelState.AddModelError("FromDate", "FromDate must be less than ToDate");
                }

                if (options.FromDate.Date < limitDate)
                {
                    ModelState.AddModelError("FromDate",
                        $"FromDate must be greater or equal than {limitDate.ToShortDateString()}");
                }

                if (!ModelState.IsValid)
                {
                    return PartialView("_ErrorMessage", GetModelErrorMessage());
                }

                var model = _memberWinLossService.GetMemberWinLoss(
                    options.FromDate,
                    options.ToDate,
                    options.ParlayTypes,
                    options.SportsType,
                    options.LiveType,
                    options.Combos,
                    options.Username,
                    options.IsCached,
                    out var roleId,
                    options.ReportBy);


                if (options.SortById.HasValue)
                {
                    model.Records = _memberWinLossService.SortData(model.Records, options.SortById.Value, options.IsSortAsc);
                }
                var isAgent = roleId > (int)RoleId.Member;
                ViewBag.Title = options.GetTitle();
                ViewBag.FilterOption = options;
                ViewBag.GrandTotal = model.Total;
                ViewBag.CustomerClass = isAgent || !model.Records.Any() ? string.Empty : model.Records.FirstOrDefault().CustomerClass;
                ViewBag.SiteGroup = isAgent || !model.Records.Any() ? string.Empty : model.Records.FirstOrDefault().SiteGroup;
                    ViewBag.IsAgent = isAgent;
                ViewBag.ReportByDate = options.ReportBy == 2;
                if (isAgent)
                {
                    var page = options.PageIndex ?? 1;
                    ViewBag.Pager = Pager.Items(model.Records.Count()).PerPage(AppSetting.PageSize).Move(page).Segment(5).Center();
                    return PartialView("AgentData", model.Records.ToPagedList(page, AppSetting.PageSize));
                }

                return PartialView("MemberData", model);
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
        }

        [HttpGet]
        public void Excel(MemberWinLossOptions options)
        {
            var model = _memberWinLossService.GetMemberWinLoss(
                                                      options.FromDate,
                                                      options.ToDate,
                                                      options.ParlayTypes,
                                                      options.SportsType,
                                                      options.LiveType,
                                                      options.Combos,
                                                      options.Username,
                                                      options.IsCached,
                                                      out var roleId,
                                                      options.ReportBy);
            var isReportByDate = options.ReportBy == 2;
            var title = options.GetTitle();

            var ms = roleId > (int)RoleId.Member ? ExportAgentWinLossRpt(model, title, isReportByDate) : ExportMemberWinLossRpt(model, title, isReportByDate);

            Response.AddHeader("content-disposition", "attachment; filename=" + "MAMemberWinLoss.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        private MemoryStream ExportMemberWinLossRpt(MemberWinLossData model, string title, bool isReportByDate)
        {
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\MemberWinLoss.xls";
            var writer = new TemplateWriter(templatePath);

            writer.WriteRow(0, new object[] { title });

            var header1 = new object[]
            {
                isReportByDate ? ViewLang.Date : ViewLang.Month,
                ViewLang.Ticket,
                string.Empty,
                ViewLang.StakeAfterPT,
                string.Empty,
                ViewLang.WinLoss,
                string.Empty,
                ViewLang.OurPT,
                string.Empty,
                string.Empty,
				ViewLang.TotalBets
			};

            var header2 = new object[]
            {
                string.Empty,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.NetWinloss
            };

            var footer = new object[]
            {
                ViewLang.Total,
                model.Total.Tickets_Won,
                model.Total.Tickets_Lose,
                model.Total.Stake_Won,
                model.Total.Stake_Lose,
                model.Total.Member_Won,
                model.Total.Member_Lose,
                model.Total.OurPT_Won,
                model.Total.OurPT_Lose,
                model.Total.OurPT_NetWinloss,
                model.Total.TotalBets
            };

            writer.WriteRows(1, new List<object[]> { header1, header2 });

            foreach (var item in model.Records)
            {
                var record = new object[]
                {
                    isReportByDate ? item.Date.ToString("MM/dd/yyyy") : item.Month,
                    item.Tickets_Won,
                    item.Tickets_Lose,
                    item.Stake_Won,
                    item.Stake_Lose,
                    item.Member_Won,
                    item.Member_Lose,
                    item.OurPT_Won,
                    item.OurPT_Lose,
                    item.OurPT_NetWinloss,
                    item.TotalBets
                };
                writer.WriteRow(3, record);
            }

            writer.WriteRow(4, footer);

            writer.SyncColumnWidths(11);

            return writer.Render("MemberWinLoss");
        }

        private MemoryStream ExportAgentWinLossRpt(MemberWinLossData model, string title, bool isReportByDate)
        {
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\AgentWinLoss.xls";
            var writer = new TemplateWriter(templatePath);

            writer.WriteRow(0, new object[] { title });

            var header1 = new object[]
            {
                isReportByDate ? ViewLang.Date : ViewLang.Month,
                ViewLang.Site,
                ViewLang.Username,
                ViewLang.CustomerClass,
                ViewLang.Ticket,
                string.Empty,
                ViewLang.StakeAfterPT,
                string.Empty,
                ViewLang.WinLoss,
                string.Empty,
                ViewLang.OurPT,
                string.Empty,
                string.Empty,
                ViewLang.TotalBets
            };

            var header2 = new object[]
            {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.Won,
                ViewLang.Lose,
                ViewLang.NetWinloss,
                string.Empty
            };

            var footer = new object[]
            {
                ViewLang.Total,
                string.Empty,
                string.Empty,
                string.Empty,
                model.Total.Tickets_Won,
                model.Total.Tickets_Lose,
                model.Total.Stake_Won,
                model.Total.Stake_Lose,
                model.Total.Member_Won,
                model.Total.Member_Lose,
                model.Total.OurPT_Won,
                model.Total.OurPT_Lose,
                model.Total.OurPT_NetWinloss,
                model.Total.TotalBets
            };

            writer.WriteRows(1, new List<object[]> { header1, header2 });

            foreach (var item in model.Records)
            {
                if (item.IsSutotalRow)
                {
                    var subtotal = new object[]
                    {
                        ViewLang.Subtotal,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        item.Tickets_Won,
                        item.Tickets_Lose,
                        item.Stake_Won,
                        item.Stake_Lose,
                        item.Member_Won,
                        item.Member_Lose,
                        item.OurPT_Won,
                        item.OurPT_Lose,
                        item.OurPT_NetWinloss,
                        item.TotalBets
                    };
                    writer.WriteRow(4, subtotal);
                }
                else
                {
                    var record = new object[]
                    {
                        isReportByDate ? item.Date.ToString("MM/dd/yyyy") : item.Month,
                        item.SiteGroup,
                        item.Username,
                        item.CustomerClass,
                        item.Tickets_Won,
                        item.Tickets_Lose,
                        item.Stake_Won,
                        item.Stake_Lose,
                        item.Member_Won,
                        item.Member_Lose,
                        item.OurPT_Won,
                        item.OurPT_Lose,
                        item.OurPT_NetWinloss,
                        item.TotalBets
                    };
                    writer.WriteRow(3, record);
                }
            }

            writer.WriteRow(5, footer);

            writer.SyncColumnWidths(14);

            return writer.Render("AgentWinLoss");
        }

        [HttpPost]
        public ActionResult DataDetails(MemberWinLossDetailOptions options)
        {
	        var user = _membersService.GetCustomerInfo(options.Username);
			ViewBag.CustId = user.CustId;
	        ViewBag.Username = options.Username;
	        
			var records = _memberWinLossService.GetDetails(options.FromDate, options.ToDate, options.ParlayTypes,
		        options.SportsType, options.LiveType, options.Combos, user.CustId);
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
    }
}
