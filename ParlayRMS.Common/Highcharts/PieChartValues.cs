// --------------------------------------------------------------------------
// <copyright file="PieChartValues.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Common.Highcharts
{
    using System.Drawing;
    using DotNet.Highcharts.Enums;
    using DotNet.Highcharts.Options;

    /// <summary>
    /// Pie Chart Values.
    /// </summary>
    public class PieChartValues : BaseChartValues
    {
        public PieChartValues(string titleText, string subtitleText, string[] categories, string titleY)
            : base(titleText, subtitleText, categories, titleY)
        {
            this.Legend = this.Legend ?? new Legend();
            this.Chart = this.Chart ?? new Chart();
            this.Chart.DefaultSeriesType = ChartTypes.Pie;
            this.Chart.PlotShadow = true;

            this.PlotOptions = this.PlotOptions ?? new PlotOptions();
            this.PlotOptions.Pie = new PlotOptionsPie
            {
                AllowPointSelect = true,
                Cursor = Cursors.Pointer,
                DataLabels = new PlotOptionsPieDataLabels
                {
                    Color = ColorTranslator.FromHtml("#3F4142"),
                    ConnectorColor = ColorTranslator.FromHtml("#000000"),
                    Formatter = "function() { return '<b>'+ this.point.name +'</b>'}"
                },
                ShowInLegend = false,
                InnerSize = "20%",
                Size = "150"
            };

            this.Series = this.Series ?? new Series();
            this.Series.Type = ChartTypes.Pie;
        }
    }
}