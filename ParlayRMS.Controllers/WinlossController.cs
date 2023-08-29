using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using ParlayRMS.Models;
using ParlayRMS.Models.Functions;
using ParlayRMS.Services.Winloss;
using ParlayRMS.Utility;
using ParlayRMS.ViewModel.Report;
using ParlayRMS.Web.Authorization;

namespace ParlayRMS.Controllers
{
    using ExcelService.Writers;

    [Authorization(Function = FunctionKeys.Winloss, Policy = UserPolicy.Read)]
    public class WinlossController : BaseController
    {
        private readonly WinlossService _winlossService = new WinlossService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Data(WinlossOptions options)
        {
            try
            {
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
                    var model = _winlossService.GetWinloss(options);
                    ViewBag.Title = options.GetTitle();
                    ViewBag.FilterOption = options;
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
        }

        [HttpGet]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public void Excel(WinlossOptions options)
        {
            var model = _winlossService.GetWinloss(options);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\Winloss.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            var title = options.GetTitle();
            writer.WriteRow(0, new object[] { title });

            // header
            var header1 = new object[]
            {
                ViewLang.Date,
                options.ReportType == ReportType.Combo ? ViewLang.Combo : "Sport Type",
                ViewLang.Ticket,
                string.Empty,
                ViewLang.StakeAfterPT,
                string.Empty,
                ViewLang.WinLoss,
                string.Empty,
                ViewLang.OurPT,
                string.Empty,
                string.Empty,
                ViewLang.Margin
            };

            var header2 = new object[]
            {
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
                ViewLang.NetWinloss
            };

            var footer = new object[]
            {
                ViewLang.Total,
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
                model.Total.Margin
            };

            writer.WriteRows(1, new List<object[]>() { header1, header2 });

            // records
            foreach (var record in model.Records)
            {
                var totalRow = record.Records.Count;
                var rowData = new object[]
                {
                    ViewLang.Subtotal,
                    string.Empty,
                    record.Total.Tickets_Won,
                    record.Total.Tickets_Lose,
                    record.Total.Stake_Won,
                    record.Total.Stake_Lose,
                    record.Total.Member_Won,
                    record.Total.Member_Lose,
                    record.Total.OurPT_Won,
                    record.Total.OurPT_Lose,
                    record.Total.OurPT_NetWinloss,
                    record.Total.Margin
                };

                foreach (var item in record.Records)
                {
                    var values = new object[]
                    {
                        item.WinlostDate,
                        options.ReportType == ReportType.Combo ? item.Combo : CacheRef.GetSportNameDropDown(item.SportType),
                        item.Tickets_Won,
                        item.Tickets_Lose,
                        item.Stake_Won,
                        item.Stake_Lose,
                        item.Member_Won,
                        item.Member_Lose,
                        item.OurPT_Won,
                        item.OurPT_Lose,
                        item.OurPT_NetWinloss,
                        item.Margin
                    };
                    writer.WriteRow(3, values);
                }

                writer.WriteCellRange(writer.RowId - totalRow, totalRow, 0, 1, record.Records[0].WinlostDate);
                writer.WriteRow(4, rowData);
            }

            // footer
            writer.WriteRow(5, footer);

            writer.SyncColumnWidths(12);

            var ms = writer.Render("Winloss");
            Response.AddHeader("content-disposition", "attachment; filename=" + "Winloss.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}