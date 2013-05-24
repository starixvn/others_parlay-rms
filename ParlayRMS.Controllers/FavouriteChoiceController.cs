// -----------------------------------------------------------------------
// <copyright file="FavouriteChoiceController.cs" company="">
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
    using ParlayRMS.Services.Implement.Charts;
    using ParlayRMS.Services.Interface.Charts;
    using ParlayRMS.Models.Functions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FavouriteChoiceController : BaseController
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

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult Index(MonitoringEventModel model)
        {
            return this.View(model);
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopFavouriteByTurnover(MonitoringSummaryModel model)
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
            chartValues.AddPointClickEvent(@"function() { ProcessChartFavouriteByTurnover(this.category, this.series.name); }");
            chartValues.LegendItemClick(@"function(){LegendItemClick.call(this);}");
            var highcharts = new Highcharts("TopSelectionByTurnover")
                .InitValues(chartValues)
                .SetSeries(listSeriesName, listSeriesData, model.States);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopFavouriteByTicket(MonitoringSummaryModel model)
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
            chartValues.AddPointClickEvent(@"function() { ProcessChartFavouriteByTicket(this.category, this.series.name); }");
            chartValues.LegendItemClick(@"function(){LegendItemClick.call(this);}");

            var highcharts = new Highcharts("TopSelectionByTicket")
                .InitValues(chartValues)
                .SetSeries(listSeriesName, listSeriesData, model.States);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTicket_Event(MonitoringEventModel model)
        {
            var service = GetService();
            var sport = this.GetSportTypeIdByName(model.SportType);
            var dataByTicket = service.GetTopSelectionByTicket_Event(model.Selection, sport, model.WinlostDate, model.MaxTransId);

            string[] categories = dataByTicket.Select(x => Utils.TruncateString(RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"), 55, Utils.TruncateOptions.IncludeEllipsis)).ToArray();
            var listSeriesData = dataByTicket.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Y = Convert.ToDouble(x.Tickets),
                DataLabels = RefData.GetLeagueName(x.Leagueid.ToString(), "en"),
                Color = Color.FromName("colors[ " + (index >= 9 ? index-- - 1 : index + 1) + "]"),
                Drilldown = new DotNet.Highcharts.Helpers.Drilldown()
                {
                    Name = x.Matchid.ToString()
                },
                Name = RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"),
                Id = x.Homeid + ":" + x.Awayid
            }).ToArray();

            var titleText = ViewLang.PopularEvents;

            var titleY = ViewLang.Tickets;
            var chartValues = new BarChartValues(titleText, model.Selection + " " + ViewLang.Selection + " - " + model.SportType, categories, titleY);

            chartValues.Tooltip.Formatter = @"function() { return '<b>' + this.point.name.replace('<b>', ' ').replace('</b>', ' ') + '</b><br/>' + this.point.dataLabels + '<br/>' + '" + ViewLang.TotalOfTickets + ": '+ this.y.format2d();}";
            chartValues.PlotOptions.Bar.Cursor = Cursors.Pointer;
            chartValues.AddPointClickEvent(@"function() { ProcessChartByTicketEventBetType(this.series.data[parseInt(this.x)].drilldown.name, this.series.data[parseInt(this.x)].name, this.series.data[parseInt(this.x)].id.split(':')[0], this.series.data[parseInt(this.x)].id.split(':')[1]); }");
            chartValues.Series.Data = new DotNet.Highcharts.Helpers.Data(listSeriesData);

            chartValues.YAxis.AllowDecimals = false;

            var highcharts = new Highcharts("TopSelectionByTicket_Event")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTicket_Event_BetType(MonitoringEventModel model)
        {
            var service = GetService();
            var sport = this.GetSportTypeIdByName(model.SportType);
            var dataByTicket = service.GetTopSelectionByTicket_Event_Bettype(model.Selection, sport, model.MatchId, model.WinlostDate, model.MaxTransId);

            var dto = dataByTicket.Where(o => o.BetType == 0 && o.Margin > 0.0m);
            var data = dataByTicket.Where(o => o.BetType != 0).OrderByDescending(x => x.Tickets).ToList();
            var total = dataByTicket.Select(o => o.Tickets).Sum();

            if (dto != null && dto.FirstOrDefault() != null)
            {
                data.Add(dto.First());
            }

            var totalTicketsNotShow = data.Where(o => o.Margin == 0.0m).Sum(x => x.Tickets);
            var categories = data.Where(o => o.Margin > 0.0m).Select(x => RefData.GetBetTypeName(x.BetType.ToString(), "en")).ToArray();

            var listSeriesData = data.Where(o => o.Margin > 0.0m).Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Id = x.BetType.ToString(),
                Name = string.Format("{0}%", x.BetType == 0 ? this.CalculatePercent(totalTicketsNotShow + x.Tickets, total).ToStringFormat() : this.CalculatePercent(x.Tickets, total).ToStringFormat()),
                Y = x.BetType == 0 ? totalTicketsNotShow + x.Tickets : x.Tickets,
                Color = Color.FromName("colors[ " + (index + 1) + "]"),
                Drilldown = new DotNet.Highcharts.Helpers.Drilldown()
                {
                    Name = RefData.GetBetTypeName(x.BetType.ToString(), "en", ViewLang.Others).Replace("Both/One/Neither", "B/O/N")
                }
            }).ToArray();

            var titleText = ViewLang.PopularBetType;

            var titleY = ViewLang.Tickets;

            var matchName = RefData.GetTeamName(model.HomeId.ToString(), "en") + " vs " + RefData.GetTeamName(model.AwayId.ToString(), "en");
            var chartValues = new PieChartValues(titleText, Utils.TruncateString(matchName, 70, Utils.TruncateOptions.IncludeEllipsis), categories, titleY);

            chartValues.Tooltip.Formatter = @"function() { return '<b>'+ this.point.drilldown.name + ': ' + this.point.name + '</b><br/>'+ '" + ViewLang.TotalOfTickets + ": '+ this.y.format2d();}";
            chartValues.AddPointClickEvent(@"function() { ProcessChartByTicketEventBetTypeChoice(" + model.MatchId + ", this.series.data[parseInt(this.x)].id, this.series.data[parseInt(this.x)].drilldown.name); }");
            chartValues.Series.Data = new DotNet.Highcharts.Helpers.Data(listSeriesData);
            chartValues.AddCloseButton();
            chartValues.PlotOptions.Pie = new DotNet.Highcharts.Options.PlotOptionsPie
            {
                AllowPointSelect = true,
                Cursor = Cursors.Pointer,
                DataLabels = new DotNet.Highcharts.Options.PlotOptionsSeriesDataLabels
                {
                    Color = ColorTranslator.FromHtml("#3F4142"),

                    Formatter = "function() { return '<b>'+ this.point.name +'</b>'}",
                    Enabled = true,
                },
                ShowInLegend = true,
                InnerSize = "20%"
            };

            chartValues.Legend.LabelFormatter = @"function() { return this.drilldown.name; }";
            chartValues.Legend.Y = 80;
            chartValues.Legend.X = 0;
            chartValues.Legend.Layout = Layouts.Vertical;
            chartValues.Legend.Align = HorizontalAligns.Right;
            chartValues.Legend.VerticalAlign = VerticalAligns.Top;
            chartValues.Legend.BorderWidth = 0;

            var highcharts = new Highcharts("TopSelectionByTicket_Event_BetType")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTicket_Event_BetType_Choice(MonitoringEventModel model)
        {
            var service = GetService();
            var sport = this.GetSportTypeIdByName(model.SportType);
            var dataByTicket = service.GetTopSelectionByTicket_Event_Bettype_Choice(model.Selection, sport, model.MatchId, model.BetType2, model.WinlostDate, model.MaxTransId).OrderByDescending(x => x.Tickets);

            var dto = dataByTicket.Where(o => o.BetTeam == "Others");
            var data = dataByTicket.Where(o => o.BetTeam != "Others").OrderByDescending(x => x.Tickets).ToList();
            var total = dataByTicket.Select(x => x.Tickets).Sum();
            if (dto != null && dto.FirstOrDefault() != null)
            {
                data.Add(dto.First());
            }

            var categories = data.Select(x => x.Tickets.ToString()).ToArray();
            var listSeriesData = data.Where(o => o.Tickets > 0).Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Id = this.GetTeamName(x.BetTeam.ToString(), model.HomeId.ToString(), model.AwayId.ToString(), model.BetType2),
                Name = this.CalculatePercent(x.Tickets, total).ToStringFormat() + "%",
                Y = x.Tickets,
                Color = Color.FromName("colors[ " + this.ColorHomeAndAway(x.BetTeam, index + 1) + "]"),
            }).ToArray();

            var titleText = ViewLang.FavouriteChoice;

            var titleY = ViewLang.Tickets;
            var chartValues = new PieChartValues(titleText, ViewLang.Bettype + ": " + model.BetTypeName, categories, titleY);

            chartValues.Tooltip.Formatter = @"function() { return '<b>'+ this.point.id + ': ' + this.point.name + '</b><br/>'+ '" + ViewLang.TotalOfTickets + ": '+ this.y.format2d();}";
            chartValues.Series.Data = new DotNet.Highcharts.Helpers.Data(listSeriesData);
            chartValues.AddCloseButton();
            chartValues.PlotOptions.Pie = new DotNet.Highcharts.Options.PlotOptionsPie
            {
                AllowPointSelect = true,
                Cursor = Cursors.Default,
                DataLabels = new DotNet.Highcharts.Options.PlotOptionsSeriesDataLabels
                {
                    Color = ColorTranslator.FromHtml("#3F4142"),

                    Formatter = "function() { return '<b>'+ this.point.name +'</b>'}",
                    Enabled = true,
                },
                ShowInLegend = true,
                InnerSize = "20%"
            };

            chartValues.Legend.LabelFormatter = @"function() { return this.id; }";
            chartValues.Legend.Y = 14;

            var highcharts = new Highcharts("TopSelectionByTicket_Event_BetType_Choice")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTurnover_Event(MonitoringEventModel model)
        {
            var service = GetService();
            var sport = this.GetSportTypeIdByName(model.SportType);
            var dataByTurnover = service.GetTopSelectionByTurnover_Event(model.Selection, sport, model.WinlostDate, model.MaxTransId);

            string[] categories = dataByTurnover.Select(x => Utils.TruncateString(RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"), 55, Utils.TruncateOptions.IncludeEllipsis)).ToArray();
            var listSeriesData = dataByTurnover.Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Y = Convert.ToDouble(x.Turnover),
                DataLabels = RefData.GetLeagueName(x.Leagueid.ToString(), "en"),
                Color = Color.FromName("colors[ " + (index >= 9 ? index-- - 1 : index + 1) + "]"),
                Drilldown = new DotNet.Highcharts.Helpers.Drilldown()
                {
                    Name = x.Matchid.ToString()
                },
                Name = RefData.GetTeamName(x.Homeid.ToString(), "en") + " " + "<b>vs</b>" + " " + RefData.GetTeamName(x.Awayid.ToString(), "en"),
                Id = x.Homeid + ":" + x.Awayid
            }).ToArray();

            var titleText = ViewLang.PopularEvents;

            var titleY = ViewLang.Turnover;
            var chartValues = new BarChartValues(titleText, model.Selection + " " + ViewLang.Selection + " - " + model.SportType, categories, titleY);

            chartValues.Tooltip.Formatter = @"function() { return '<b>' + this.point.name.replace('<b>', ' ').replace('</b>', ' ') + '</b><br/>' + this.point.dataLabels + '<br/>' + '" + ViewLang.TotalOfTurnover + ": '+ this.y.format2d();}";
            chartValues.PlotOptions.Bar.Cursor = Cursors.Pointer;
            chartValues.AddPointClickEvent(@"function() { ProcessChartByTurnoverEventBetType(this.series.data[parseInt(this.x)].drilldown.name, this.series.data[parseInt(this.x)].name, this.series.data[parseInt(this.x)].id.split(':')[0], this.series.data[parseInt(this.x)].id.split(':')[1]); }");
            chartValues.Series.Data = new DotNet.Highcharts.Helpers.Data(listSeriesData);

            var highcharts = new Highcharts("TopSelectionByTurnover_Event")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTurnover_Event_BetType(MonitoringEventModel model)
        {
            var service = GetService();
            var sport = this.GetSportTypeIdByName(model.SportType);
            var dataByTurnover = service.GetTopSelectionByTurnover_Event_Bettype(model.Selection, sport, model.MatchId, model.WinlostDate, model.MaxTransId);

            var dto = dataByTurnover.Where(o => o.BetType == 0 && o.Margin > 0.0m);
            var data = dataByTurnover.Where(o => o.BetType != 0).OrderByDescending(x => x.Turnover).ToList();

            if (dto != null && dto.FirstOrDefault() != null)
            {
                data.Add(dto.First());
            }

            var totalTurnoverNotShow = data.Where(o => o.Margin == 0.0m).Sum(x => x.Turnover);
            var categories = data.Where(o => o.Margin > 0.0m).Select(x => RefData.GetBetTypeName(x.BetType.ToString(), "en")).ToArray();
            var total = dataByTurnover.Select(o => o.Turnover).Sum();

            var listSeriesData = data.Where(o => o.Margin > 0.0m).Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Id = x.BetType.ToString(),
                Name = string.Format("{0}%", x.BetType == 0 ? this.CalculatePercent(x.Turnover + totalTurnoverNotShow, total).ToStringFormat() : this.CalculatePercent(x.Turnover, total).ToStringFormat()),
                Y = x.BetType == 0 ? Convert.ToDouble(x.Turnover + totalTurnoverNotShow) : Convert.ToDouble(x.Turnover),
                Color = Color.FromName("colors[ " + (index + 1) + "]"),
                Drilldown = new DotNet.Highcharts.Helpers.Drilldown()
                {
                    Name = RefData.GetBetTypeName(x.BetType.ToString(), "en", ViewLang.Others).Replace("Both/One/Neither", "B/O/N")
                }
            }).ToArray();

            var titleText = ViewLang.PopularBetType;
            var matchName = RefData.GetTeamName(model.HomeId.ToString(), "en") + " vs " + RefData.GetTeamName(model.AwayId.ToString(), "en");
            var titleY = ViewLang.Turnover;
            var chartValues = new PieChartValues(titleText, Utils.TruncateString(matchName, 70, Utils.TruncateOptions.IncludeEllipsis), categories, titleY);

            chartValues.Tooltip.Formatter = @"function() { return '<b>'+ this.point.drilldown.name + ': ' + this.point.name + '</b><br/>'+ '" + ViewLang.TotalOfTurnover + ": '+ this.y.format2d();}";
            chartValues.AddPointClickEvent(@"function() { ProcessChartByTurnoverEventBetTypeChoice(" + model.MatchId + ", this.series.data[parseInt(this.x)].id, this.series.data[parseInt(this.x)].drilldown.name); }");
            chartValues.AddCloseButton();
            chartValues.PlotOptions.Pie = new DotNet.Highcharts.Options.PlotOptionsPie
            {
                AllowPointSelect = true,
                Cursor = Cursors.Pointer,
                DataLabels = new DotNet.Highcharts.Options.PlotOptionsSeriesDataLabels
                {
                    Color = ColorTranslator.FromHtml("#3F4142"),

                    Formatter = "function() { return '<b>'+ this.point.name +'</b>'}",
                    Enabled = true,
                },
                ShowInLegend = true,
                InnerSize = "20%"
            };

            chartValues.Legend.LabelFormatter = @"function() { return this.drilldown.name; }";
            chartValues.Legend.Y = 80;
            chartValues.Legend.X = 0;
            chartValues.Legend.Layout = Layouts.Vertical;
            chartValues.Legend.Align = HorizontalAligns.Right;
            chartValues.Legend.VerticalAlign = VerticalAligns.Top;
            chartValues.Legend.BorderWidth = 0;

            chartValues.Series.Data = new DotNet.Highcharts.Helpers.Data(listSeriesData);

            var highcharts = new Highcharts("TopSelectionByTurnover_Event_BetType")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        [AuthorizationAttribute(Function = FunctionKeys.FavouriteChoice, Policy = ePolicy.Read)]
        public ActionResult TopSelectionByTurnover_Event_BetType_Choice(MonitoringEventModel model)
        {
            var service = GetService();
            var sport = this.GetSportTypeIdByName(model.SportType);
            var dataByTurnover = service.GetTopSelectionByTurnover_Event_Bettype_Choice(model.Selection, sport, model.MatchId, model.BetType2, model.WinlostDate, model.MaxTransId).OrderByDescending(x => x.Turnover);

            var dto = dataByTurnover.Where(o => o.BetTeam == "Others");
            var data = dataByTurnover.Where(o => o.BetTeam != "Others").OrderByDescending(x => x.Turnover).ToList();
            var total = dataByTurnover.Select(x => x.Turnover).Sum();
            if (dto != null && dto.FirstOrDefault() != null)
            {
                data.Add(dto.First());
            }

            var categories = data.Select(x => x.Turnover.ToString()).ToArray();
            var listSeriesData = data.Where(o => o.Turnover > 0.0m).Select((x, index) => new DotNet.Highcharts.Options.Point
            {
                Id = this.GetTeamName(x.BetTeam.ToString(), model.HomeId.ToString(), model.AwayId.ToString(), model.BetType2),
                Name = this.CalculatePercent(x.Turnover, total).ToStringFormat() + "%",
                Y = Convert.ToDouble(x.Turnover),
                Color = Color.FromName("colors[ " + this.ColorHomeAndAway(x.BetTeam, index + 1) + "]"),
            }).ToArray();
            var titleText = ViewLang.FavouriteChoice;

            var titleY = ViewLang.Tickets;
            var chartValues = new PieChartValues(titleText, ViewLang.Bettype + ": " + model.BetTypeName, categories, titleY);

            chartValues.Tooltip.Formatter = @"function() { return '<b>'+ this.point.id + ': ' + this.point.name + '</b><br/>'+ '" + ViewLang.TotalOfTurnover + ": '+ this.y.format2d();}";
            chartValues.AddCloseButton();
            chartValues.PlotOptions.Pie.Cursor = Cursors.Default;
            chartValues.Series.Data = new DotNet.Highcharts.Helpers.Data(listSeriesData);
            chartValues.PlotOptions.Pie = new DotNet.Highcharts.Options.PlotOptionsPie
            {
                AllowPointSelect = true,
                Cursor = Cursors.Default,
                DataLabels = new DotNet.Highcharts.Options.PlotOptionsSeriesDataLabels
                {
                    Color = ColorTranslator.FromHtml("#3F4142"),

                    Formatter = "function() { return '<b>'+ this.point.name +'</b>'}",
                    Enabled = true,
                },
                ShowInLegend = true,
                InnerSize = "20%"
            };

            chartValues.Legend.LabelFormatter = @"function() { return this.id; }";
            chartValues.Legend.Y = 14;

            var highcharts = new Highcharts("TopSelectionByTurnover_Event_BetType_Choice")
                .InitValues(chartValues);

            return this.Content(highcharts.ToHtmlString());
        }

        private string GetTeamName(string teamHA, string homeId, string awayId, int betType2 = 0)
        {
            return this.TeamNameFormater(teamHA, homeId, awayId, betType2);
        }

        private int ColorHomeAndAway(string teamHA, int index)
        {
            if ((teamHA.Contains("h") || teamHA.Contains("a")) && !teamHA.Equals(ViewLang.Others, StringComparison.OrdinalIgnoreCase) && !teamHA.Contains("h:") && !teamHA.Contains("a:") && !IsCleanSheetBettype(teamHA) && !teamHA.Contains("/"))
            {
                if (this.IsHomeTeam(teamHA))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else if (teamHA.Equals("1") || teamHA.Equals("x") || teamHA.Equals("2"))
            {
                if (this.IsHomeTeam(teamHA))
                {
                    return 1;
                }
                else if (this.IsAwayTeam(teamHA))
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return index;
            }
        }

        private bool IsCleanSheetBettype(string teamName)
        {
            if (teamName.Contains("hy") || teamName.Contains("hn") || teamName.Contains("ay") || teamName.Contains("an")) return true;
            return false;
        }

        private bool IsHomeTeam(string teamHA, int betType2 = 0)
        {
            return (teamHA.Contains("h") || teamHA.Equals("1")) && !teamHA.Equals(ViewLang.Others, StringComparison.OrdinalIgnoreCase) && betType2 != 3 && betType2 != 8 && betType2 != 2 && betType2 != 13;
        }

        private bool IsAwayTeam(string teamHA, int betType2 = 0)
        {
            return (teamHA.Contains("a") || teamHA.Equals("2")) && betType2 != 3 && betType2 != 8 && betType2 != 2 && betType2 != 13;
        }

        private decimal CalculatePercent(decimal alone, decimal total)
        {
            return (alone * 100) / total;
        }

        private string TeamNameFormater(string teamName, string homeId, string awayId, int bettype = 0)
        {
            if (bettype == 24)
            {
                // Double Change
                switch (teamName)
                {
                    case "1x":
                        return ViewLang.HomeOrDraw;
                    case "2x":
                        return ViewLang.AwayOrDraw;
                    case "12":
                        return ViewLang.HomeOrAway;
                }
            }
            else if (bettype == 14)
            {
                // First Goal, Last Goal
                string[] fglg = teamName.ToString().Split(":".ToCharArray()); // 1:2
                string teamNameFormated = string.Empty;
                if (fglg[0].Equals("1"))
                {
                    teamNameFormated += ViewLang.Home;
                }
                else if (fglg[0].Equals("2"))
                {
                    teamNameFormated += ViewLang.Away;
                }
                else
                {
                    teamNameFormated += ViewLang.No;
                }

                if (fglg[1].Equals("1"))
                {
                    teamNameFormated += " " + ViewLang.FG;
                }
                else if (fglg[1].Equals("2"))
                {
                    teamNameFormated += " " + ViewLang.LG;
                }
                else
                {
                    teamNameFormated += " " + ViewLang.Goal;
                }

                return teamNameFormated;
            }
            else if (bettype == 3 || bettype == 8)
            {
                // Over and Under
                string[] ou = teamName.Split(":".ToCharArray());
                switch (ou[0].ToLower())
                {
                    case "h":
                        return ViewLang.Over + " " + string.Format("{0:0.##}", DbConverter.ToDecimal(ou[1]));
                    case "a":
                        return ViewLang.Under + " " + string.Format("{0:0.##}", DbConverter.ToDecimal(ou[1]));
                    default:
                        return teamName;
                }
            }
            else if (bettype == 2)
            {
                // Odd and Even
                if (this.IsHomeTeam(teamName))
                {
                    return ViewLang.Odd;
                }

                if (this.IsAwayTeam(teamName))
                {
                    return ViewLang.Even;
                }
            }
            else if (bettype == 13)
            {
                // Clean Sheet
                switch (teamName)
                {
                    case "hy":
                        return ViewLang.Home + " " + ViewLang.Yes;
                    case "hn":
                        return ViewLang.Home + " " + ViewLang.No;
                    case "ay":
                        return ViewLang.Away + " " + ViewLang.Yes;
                    case "an":
                        return ViewLang.Away + " " + ViewLang.No;
                }
            }
            else if (bettype == 16)
            {
                // HTFT
                switch (teamName)
                {
                    case "1:1":
                        return "HH";

                    case "1:0":
                        return "HD";

                    case "1:2":
                        return "HA";

                    case "0:1":
                        return "DH";

                    case "0:0":
                        return "DD";

                    case "0:2":
                        return "DA";

                    case "2:1":
                        return "AH";

                    case "2:0":
                        return "AD";

                    case "2:2":
                        return "AA";

                    default:
                        return ViewLang.Others;
                }
            }
            else if (bettype == 26)
            {
                //  Both/One/Neither
                switch (teamName)
                {
                    case "b":
                        return ViewLang.Both;

                    case "o":
                        return ViewLang.One;

                    case "n":
                        return ViewLang.NoGoal;
                }
            }
            else if (bettype == 30)
            { // 1st Correct Score
                switch (teamName)
                {
                    case "4:0":
                    case "0:4":
                        return "[" + teamName + " UP]";
                    default:
                        return "[" + teamName + "]";
                }
            }
            else if (bettype == 4)
            {  // Correct Score
                switch (teamName)
                {
                    case "5:0":
                    case "0:5":
                        return "[" + teamName + " UP]";
                    default:
                        return "[" + teamName + "]";
                }
            }
            else if (bettype == 28)
            {
                // 3-way handicap
                switch (teamName)
                {
                    case "1":
                        return ViewLang.Home + "(-1)";

                    case "x":
                        return ViewLang.Draw + "(" + ViewLang.Away + " +1)";

                    case "2":
                        return ViewLang.Away + "(+1)";
                }
            }
            else if (bettype == 5 || bettype == 15)
            { // FT 1x2 and 1st 1x2
                switch (teamName)
                {
                    case "1":
                        return "[1] " + RefData.GetTeamName(homeId, "en");
                    case "2":
                        return "[2] " + RefData.GetTeamName(awayId, "en");
                    case "x":
                        return "[x] " + ViewLang.Both;
                }
            }
            else
            {
                switch (teamName)
                {
                    case "h":
                        return RefData.GetTeamName(homeId, "en");
                    case "a":
                        return RefData.GetTeamName(awayId, "en");
                }
            }

            return teamName;
        }
    }
}
