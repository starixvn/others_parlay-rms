// -----------------------------------------------------------------------
// <copyright file="WinlossController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ParlayRMS.Common.Models;
    using ParlayRMS.ExcelService.Writers;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Services.Implement.Winloss;
    using ParlayRMS.Services.Interface.Winloss;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Report;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.ViewModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinlossController : BaseController
    {
        private readonly IWinlossService winlossService = new WinlossService();

        [AuthorizationAttribute(Function = FunctionKeys.Winloss, Policy = ePolicy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.BetTypes = Config.BetTypes;
            ViewBag.SportsTypes = Config.SportsTypes;
            return this.View();
        }

        [AuthorizationAttribute(Function = FunctionKeys.Winloss, Policy = ePolicy.Read)]
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
                    return this.PartialView("_ErrorMessage", GetModelErrorMessage());
                }
                else
                {
                    var model = this.winlossService.GetWinloss(options.FromDate, options.ToDate, options.BetType, options.SportsType, options.Combos);
                    ViewBag.Title = options.GetTitle();
                    ViewBag.FilterOption = options;
                    return this.PartialView(model);
                }
            }
            catch (Exception ex)
            {
                return this.PartialView("_ErrorMessage", ex.Message);
            }
        }

        [AuthorizationAttribute(Function = FunctionKeys.Winloss, Policy = ePolicy.Read)]
        [HttpGet]
        public void Excel(WinlossOptions options)
        {
            var model = this.winlossService.GetWinloss(options.FromDate, options.ToDate, options.BetType, options.SportsType, options.Combos);
            var templatePath = CurrentContext.Context.Server.MapPath("~") + "\\ExcelTemplates\\Winloss.xls";
            var writer = new TemplateWriter(templatePath);

            // title
            var title = options.GetTitle();
            writer.WriteRow(0, new object[] { title });

            // header
            var header1 = new object[]{
                ViewLang.Date,
                ViewLang.Ticket,
                "",
                ViewLang.StakeAfterPT,
                "",
                ViewLang.WinLoss,
                "",
                ViewLang.OurPT
            };

            var header2 = new object[]{
                "",
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

            writer.WriteRows(1, new List<object[]>() { header1, header2 });

            // records
            writer.WriteRows(3, model.Records);

            // footer
            writer.WriteRow(4, new object[] {
                ViewLang.Total,
                model.Total.Tickets_Won,
                model.Total.Tickets_Lose,
                model.Total.Stake_Won,
                model.Total.Stake_Lose,
                model.Total.Member_Won,
                model.Total.Member_Lose,
                model.Total.OurPT_Won,
                model.Total.OurPT_Lose,
                model.Total.OurPT_NetWinloss
            });

            writer.SyncColumnWidths(10);

            var ms = writer.Render("Winloss");
            Response.AddHeader("content-disposition", "attachment; filename=" + "Winloss.xls");
            Response.AddHeader("content-type", "application/vnd.ms-excel");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}