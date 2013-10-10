// --------------------------------------------------------------------------
// <copyright file="BarChartValues.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Common.Highcharts
{
    using DotNet.Highcharts.Enums;
    using DotNet.Highcharts.Options;

    /// <summary>
    /// Bar Chart Values.
    /// </summary>
    public class BarChartValues : BaseChartValues
    {
        public BarChartValues(string titleText, string subtitleText, string[] categories, string titleY)
            : base(titleText, subtitleText, categories, titleY)
        {
            this.Chart = this.Chart ?? new Chart();
            this.Chart.DefaultSeriesType = ChartTypes.Bar;

            this.Legend = this.Legend ?? new Legend();
            this.Legend.Enabled = false;
            this.Legend.Layout = Layouts.Vertical;
            this.Legend.Align = HorizontalAligns.Right;
            this.Legend.VerticalAlign = VerticalAligns.Top;
            this.Legend.X = -100;
            this.Legend.Y = 100;
            this.Legend.Floating = true;
            this.Legend.BorderWidth = 1;
            this.Legend.Shadow = true;

            this.PlotOptions = this.PlotOptions ?? new PlotOptions();
            this.PlotOptions.Bar = this.PlotOptions.Bar ?? new PlotOptionsBar();
            this.PlotOptions.Bar.DataLabels = this.PlotOptions.Bar.DataLabels ?? new PlotOptionsBarDataLabels()
            {
                Enabled = true,
                Formatter = "function() { return this.y.format2d(); }",
                X = -1,
                Y = 3,
            };

            this.Tooltip = this.Tooltip ?? new Tooltip();
            this.Tooltip.Formatter = "function() { return ''+ this.x +': '+ this.y ; }";

            this.XAxis = this.XAxis ?? new XAxis();
            this.YAxis = this.YAxis ?? new YAxis();

            this.Series = this.Series ?? new Series();
        }
    }
}