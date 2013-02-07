// -----------------------------------------------------------------------
// <copyright file="TopSelectionController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Builder.ChoiceBuilder;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Implement;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Monitoring;
    using ParlayRMS.Web;
    using ParlayRMS.Web.Authorization;
    using ParlayRMS.Common.Highcharts;
    using DotNet.Highcharts;
    using DotNet.Highcharts.Enums;
    using RemotingRef;
    using System;
    using System.Drawing;
    using DotNet.Highcharts.Helpers;
    using ParlayRMS.Common;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Services.Implement.Charts;
    using ParlayRMS.Services.Interface.Charts;
    using ParlayRMS.Models.Functions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TopSelectionController : BaseController
    {
        private readonly IChartsService monitorService = new ChartsService();
        private readonly IChartsService monitorCacheService = new ChartsCacheService();

        private IChartsService GetService()
        {
            if (AppSetting.IsCache)
            {
                return monitorCacheService;
            }

            return monitorService;
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult Index()
        {
            ViewBag.IsCache = ChartsCacheService.IsCache.ToString().ToLower();
            return this.View();
        }

        [AuthorizationAttribute(Function = eFunction.Monitoring, Policy = ePolicy.Read)]
        public ActionResult DataSum(MonitoringSummaryModel model)
        {
            var service = GetService();
            int bettype = DbConverter.ToInt32(CurrentContext.Context.Request["bettype"]);
            DateTime winlostDate = model.WinlostDate;
            long maxTransId = 0;
            var dataSum = service.GetSumDataResult(bettype, winlostDate, out maxTransId);
            ViewBag.MaxTransId = maxTransId;

            return this.PartialView("DataSum", dataSum);
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTicket(MonitoringSummaryModel model)
        {
            var service = GetService();
            var dataByTicket = service.GetTopSelectionByTicket(model.WinlostDate, model.MaxTransId);

            string[] categories = dataByTicket.Select(x => x.Selection.ToString()).ToArray();
            object[] allSport = dataByTicket.Select(x => x.All_Sports.ToString()).ToArray();
            object[] soccerOnly = dataByTicket.Select(x => x.Soccer_Only.ToString()).ToArray();
            object[] combinedSport = dataByTicket.Select(x => x.Combined_Sports.ToString()).ToArray();
            object[] otherSport = dataByTicket.Select(x => x.Other_Sports.ToString()).ToArray();

            string[] listSeriesName = new string[] { ViewLang.Allsports, ViewLang.Socceronly, ViewLang.Combinedsports, ViewLang.Othersports };
            object[] listSeriesData = new object[] { allSport, soccerOnly, combinedSport, otherSport };

            string titleText = ViewLang.TopSelectionByTickets;

            string titleY = ViewLang.Tickets;
            var chartValues = new LineChartValues(titleText, string.Empty, categories, titleY);
            chartValues.Tooltip.Formatter = @"function() { return '<b>'+ this.series.name + '</b><br/>'+ '" + ViewLang.TotalOfSelections + ": ' + this.x + ' <br/>' + '" + ViewLang.TotalOfTickets + ": '+ this.y.format2d();}";
            chartValues.AddPointClickEvent(@"function() { ProcessChartByTicket(this.category, this.series.name); }");
            chartValues.LegendItemClick(@"function(){LegendItemClick.call(this);}");

            Highcharts highcharts = new Highcharts("TopSelectionByTicket")
                .InitValues(chartValues)
                .SetSeries(listSeriesName, listSeriesData, model.States);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTurnover(MonitoringSummaryModel model)
        {
            var service = GetService();
            var dataByTurnover = service.GetTopSelectionByTurnover(model.WinlostDate, model.MaxTransId);
            string[] categories = dataByTurnover.Select(x => x.Selection.ToString()).ToArray();
            object[] allSport = dataByTurnover.Select(x => x.All_Sports.ToString()).ToArray();
            object[] soccerOnly = dataByTurnover.Select(x => x.Soccer_Only.ToString()).ToArray();
            object[] combinedSport = dataByTurnover.Select(x => x.Combined_Sports.ToString()).ToArray();
            object[] otherSport = dataByTurnover.Select(x => x.Other_Sports.ToString()).ToArray();

            string[] listSeriesName = new string[] { ViewLang.Allsports, ViewLang.Socceronly, ViewLang.Combinedsports, ViewLang.Othersports };
            object[] listSeriesData = new object[] { allSport, soccerOnly, combinedSport, otherSport };

            string titleText = ViewLang.TopSelelctionByTurnover;

            string titleY = ViewLang.Turnover;
            var chartValues = new LineChartValues(titleText, string.Empty, categories, titleY);
            chartValues.Tooltip.Formatter = @"function() { return '<b>'+ this.series.name + '</b><br/>'+ '" + ViewLang.TotalOfSelections + ": ' + this.x + ' <br/>' + '" + ViewLang.TotalOfTurnover + ": '+ this.y.format2d();}";
            chartValues.AddPointClickEvent(@"function() { ProcessChartByTurnover(this.category, this.series.name); }");
            chartValues.LegendItemClick(@"function(){LegendItemClick.call(this);}");
            Highcharts highcharts = new Highcharts("TopSelectionByTurnover")
                .InitValues(chartValues)
                .SetSeries(listSeriesName, listSeriesData, model.States);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopLeagueByTicket(MonitoringDetailModel model)
        {
            // data
            var service = GetService();
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            var data = service.GetTopLeagueByTicket(model.Selection, sport, model.WinlostDate, model.MaxTransId);

            // chart data
            string titleText = string.Format(ViewLang.SelectionByLeague, model.Selection, ModelsHelper.GetSportTypeById(sport));
            string titleY = ViewLang.Tickets;
            string[] categories = data.Select(x => RefData.GetLeagueName(x.Leagueid.ToString(), "en", string.Empty)).ToArray();

            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point { Id = x.Leagueid.ToString(), Y = x.NoTickets, Color = Color.FromName("colors[ " + (index + 1) + "]") }).ToArray();

            var chartValues = new ColumnChartValues(titleText, string.Empty, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);
            chartValues.Tooltip.Formatter = "function() { return '<b>' + this.x + '</b><br />' + '" + ViewLang.Tickets + ": ' + this.y.format2d(); }";
            chartValues.AddPointClickEvent("function(){ ProcessChartEventLeagueByTicket(this.series.data[parseInt(this.x)].id, this.category); }");
            var highcharts = new Highcharts("TopLeagueByTicket")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopLeagueByTurnover(MonitoringDetailModel model)
        {
            var service = GetService();
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            var data = service.GetTopLeagueByTurnover(model.Selection, sport, model.WinlostDate, model.MaxTransId);

            string titleText = string.Format(ViewLang.SelectionByLeague, model.Selection, ModelsHelper.GetSportTypeById(sport));

            string titleY = ViewLang.Turnover;
            string[] categories = data.Select(x => RefData.GetLeagueName(x.Leagueid.ToString(), "en")).ToArray();
            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Y = DbConverter.ToDouble(x.Turnover),
                Color = Color.FromName("colors[ " + (index + 1) + "]"),
                Id = x.Leagueid.ToString()
            }).ToArray();

            var chartValues = new ColumnChartValues(titleText, string.Empty, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);
            chartValues.Tooltip.Formatter = "function() { return '<b>' + this.x + '</b><br />' + '" + ViewLang.Turnover + ": ' + this.y.format2d(); }";
            chartValues.AddPointClickEvent("function(){ ProcessChartEventLeagueByTurnover(this.series.data[parseInt(this.x)].id, this.category); }");
            var highcharts = new Highcharts("TopLeagueByTurnover")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopCurrencyByTicket(MonitoringDetailModel model)
        {
            // data
            var service = GetService();
            DateTime winlostDate = model.WinlostDate;
            int selection = model.Selection;
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(model.SportType);
            var dataByCurrency = service.GetTopCurrencyByTicket(selection, sport, winlostDate, model.MaxTransId);

            var dto = dataByCurrency.Where(o => o.CurrencyId == 0 && o.Margin > 0.0m);
            var data = dataByCurrency.Where(o => o.CurrencyId != 0).OrderByDescending(x => x.NoTickets).ToList();

            if (dto != null && dto.FirstOrDefault() != null)
            {
                data.Add(dto.First());
            }

            // chart data
            string titleText = string.Format(ViewLang.SelectionByCurrency, selection.ToString(), sportName);

            string titleY = ViewLang.Turnover;
            string[] categories = data.Select(x => x.Currency).ToArray();
            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Name = string.Format("{0}: {1}%", x.Currency, x.Margin.ToString("0.##")),
                Y = x.NoTickets,
                Color = Color.FromName("colors[ " + index + "]"),
                Id = x.CurrencyId.ToString(),
                Drilldown = new Drilldown() { Name = x.Currency }
            }).Where(y => y.Y > 0.0).ToArray();

            var topCurrency = string.Empty;

            dataByCurrency.Select(x => x.CurrencyId).Where(y => y != 0).ToList().ForEach(o =>
            {
                topCurrency += o + ",";
            });
            if (topCurrency != string.Empty)
            {
                topCurrency = topCurrency.Remove(topCurrency.LastIndexOf(','));
            }

            var chartValues = new PieChartValues(titleText, string.Empty, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);
            chartValues.Tooltip.Formatter = "function() { return '<b>'+ this.point.name +'</b>'+ '<br />' + '" + ViewLang.Tickets + ": ' + this.y.format2d(); }";
            chartValues.AddPointClickEvent("function(){ ProcessChartEventCurrencyByTicket(this.series.data[parseInt(this.x)].id, this.series.data[parseInt(this.x)].drilldown.name, '" + topCurrency + "'); }");
            var highcharts = new Highcharts("TopSelectionByCurrency")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopCurrencyByTurnover(MonitoringDetailModel model)
        {
            // data
            var service = GetService();
            DateTime winlostDate = model.WinlostDate;
            int selection = model.Selection;
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            var dataByCurrency = service.GetTopCurrencyByTurnover(selection, sport, winlostDate, model.MaxTransId);

            var dto = dataByCurrency.Where(o => o.CurrencyId == 0 && o.Margin > 0.0m);
            var data = dataByCurrency.Where(o => o.CurrencyId != 0).OrderByDescending(x => x.Turnover).ToList();

            if (dto != null && dto.FirstOrDefault() != null)
            {
                data.Add(dto.First());
            }

            // chart data
            string titleText = string.Format(ViewLang.SelectionByCurrency, selection.ToString(), sportName);
            string subtitleText = string.Empty;
            string titleY = ViewLang.Turnover;
            string[] categories = data.Select(x => string.Format("{0}: {1} %", x.Currency, x.Margin.ToString("0.##"))).ToArray();
            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Name = string.Format("{0}: {1}%", x.Currency, x.Margin.ToString("0.##")),
                Y = Convert.ToDouble(Math.Round(x.Turnover, 2)),
                Color = Color.FromName("colors[ " + index + "]"),
                Id = x.CurrencyId.ToString(),
                Drilldown = new Drilldown() { Name = x.Currency }
            }).Where(y => y.Y > 0.0).ToArray();

            var topCurrency = string.Empty;

            dataByCurrency.Select(x => x.CurrencyId).Where(y => y != 0).ToList().ForEach(o =>
            {
                topCurrency += o + ",";
            });

            if (topCurrency != string.Empty)
            {
                topCurrency = topCurrency.Remove(topCurrency.LastIndexOf(','));
            }

            var chartValues = new PieChartValues(titleText, subtitleText, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);
            chartValues.Tooltip.Formatter = "function() { return '<b>'+ this.point.name +'</b>'+ '<br />' + '" + ViewLang.Turnover + ": ' + this.y.format2d(); }";
            chartValues.AddPointClickEvent("function(){ ProcessChartEventCurrencyByTurnover(this.series.data[parseInt(this.x)].id, this.series.data[parseInt(this.x)].drilldown.name, '" + topCurrency + "'); }");
            var highcharts = new Highcharts("TopSelectionByCurrency")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopEventCurrencyByTicket(MonitoringEventModel model)
        {
            var service = GetService();
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            var data = service.GetTopSelectionTicketEventByCurrency(model.Selection, sport, model.CurrencyId, model.TopCurrency, model.WinlostDate, model.MaxTransId);

            string titleText = ViewLang.PopularEvents;

            string titleY = ViewLang.Tickets;
            string[] categories = data.Select(x => Utils.TruncateString(RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"), 55, Utils.TruncateOptions.IncludeEllipsis)).ToArray();
            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Y = Convert.ToDouble(x.Tickets),
                Color = Color.FromName("colors[ " + (index >= 9 ? index-- - 1 : index + 1) + "]"),
                Name = RefData.GetLeagueName(x.Leagueid.ToString(), "en"),
                DataLabels = RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en")
            }).ToArray();

            var chartValues = new BarChartValues(titleText, ViewLang.Currency + ": " + model.CurrencyName, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);
            chartValues.Tooltip.Formatter = "function() { return '<b>' + this.point.dataLabels.replace('<b>', ' ').replace('</b>', ' ') + '</b>'+ '<br />' + this.point.name + '<br/>' + '" + ViewLang.Tickets + ": ' + this.y.format2d(); }";
            chartValues.YAxis.AllowDecimals = false;

            var highcharts = new Highcharts("TopEventCurrencyByTicket")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopEventLeagueByTicket(MonitoringEventModel model)
        {
            var service = GetService();
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            var data = service.GetTopSelectionTicketEventByLeague(model.Selection, sport, model.LeagueId, model.WinlostDate, model.MaxTransId);

            string titleText = ViewLang.PopularEvents;

            string titleY = ViewLang.Tickets;
            string[] categories = data.Select(x => Utils.TruncateString(RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"), 55, Utils.TruncateOptions.IncludeEllipsis)).ToArray();
            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Y = Convert.ToDouble(x.Tickets),
                Color = Color.FromName("colors[ " + (index >= 9 ? index-- - 1 : index + 1) + "]"),
                Name = RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"),
            }).ToArray();

            var chartValues = new BarChartValues(titleText, ViewLang.League + ": " + model.LeagueName, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);
            chartValues.Tooltip.Formatter = "function() { return '<b>' + this.point.name.replace('<b>', ' ').replace('</b>', ' ') + '</b>'+ '<br />' + '" + ViewLang.Tickets + ": ' + this.y.format2d(); }";
            chartValues.YAxis.AllowDecimals = false;

            var highcharts = new Highcharts("TopEventLeagueByTicket")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopEventCurrencyByTurnover(MonitoringEventModel model)
        {
            var service = GetService();
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            var data = service.GetTopSelectionTurnoverEventByCurrency(model.Selection, sport, model.CurrencyId, string.Empty, model.WinlostDate, model.MaxTransId);

            string titleText = ViewLang.PopularEvents;

            string titleY = ViewLang.Turnover;
            string[] categories = data.Select(x => Utils.TruncateString(RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"), 55, Utils.TruncateOptions.IncludeEllipsis)).ToArray();
            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Y = Convert.ToDouble(x.Turnover),
                Color = Color.FromName("colors[ " + (index >= 9 ? index-- - 1 : index + 1) + "]"),
                Name = RefData.GetLeagueName(x.Leagueid.ToString(), "en"),
                DataLabels = RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en")
            }).ToArray();

            var chartValues = new BarChartValues(titleText, ViewLang.Currency + ": " + model.CurrencyName, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);
            chartValues.Tooltip.Formatter = "function() { return '<b>' + this.point.dataLabels.replace('<b>', ' ').replace('</b>', ' ') + '</b>'+ '<br />' + this.point.name + '<br/>' + '" + ViewLang.Turnover + ": ' + this.y.format2d(); }";
            var highcharts = new Highcharts("TopEventCurrencyByTurnover")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = eFunction.TopSelection, Policy = ePolicy.Read)]
        public ActionResult TopEventLeagueByTurnover(MonitoringEventModel model)
        {
            var service = GetService();
            string sportName = model.SportType;
            int sport = this.GetSportTypeIdByName(sportName);
            var data = service.GetTopSelectionTurnoverEventByLeague(model.Selection, sport, model.LeagueId, model.WinlostDate, model.MaxTransId);

            string titleText = ViewLang.PopularEvents;

            string titleY = ViewLang.Turnover;
            string[] categories = data.Select(x => Utils.TruncateString(RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"), 55, Utils.TruncateOptions.IncludeEllipsis)).ToArray();

            decimal max = data.Max(a => a.Turnover);

            long maxvalue = Convert.ToInt64(max);

            double morevalue = maxvalue * 0.05;
            maxvalue += Convert.ToInt64(morevalue) * 2;

            var listSeriesData = data.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Y = Convert.ToDouble(x.Turnover),
                Color = Color.FromName("colors[ " + (index >= 9 ? index-- - 1 : index + 1) + "]"),
                Name = RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en")
            }).ToArray();

            var chartValues = new BarChartValues(titleText, ViewLang.League + ": " + model.LeagueName, categories, titleY);
            chartValues.AddCloseButton();
            chartValues.Series.Data = new Data(listSeriesData);

            chartValues.Tooltip.Formatter = "function() { return '<b>' + this.point.name.replace('<b>', ' ').replace('</b>', ' ') + '</b>'+ '<br />' + '" + ViewLang.Turnover + ": ' + this.y.format2d(); }";
            chartValues.YAxis.Max = maxvalue;

            var highcharts = new Highcharts("TopEventLeagueByTurnover")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }
    }
}
