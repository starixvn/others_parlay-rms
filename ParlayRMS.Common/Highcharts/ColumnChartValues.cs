// --------------------------------------------------------------------------
// <copyright file="ColumnChartValues.cs" company="Nexcel Solutions Vietnam">
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
    /// Column Chart Values.
    /// </summary>
    public class ColumnChartValues : BaseChartValues
    {
        public ColumnChartValues(string titleText, string subtitleText, string[] categories, string titleY)
            : base(titleText, subtitleText, categories, titleY)
        {
            this.Chart = this.Chart ?? new Chart();
            this.Chart.DefaultSeriesType = ChartTypes.Column;

            this.Legend = this.Legend ?? new Legend();
            this.Legend.Enabled = false;

            this.PlotOptions = this.PlotOptions ?? new PlotOptions();
            double pointWidth = 0.2;
            if (categories.Length == 1)
            {
                pointWidth = 0.4;
            }

            if (categories.Length == 2)
            {
                pointWidth = 0.3;
            }

            this.PlotOptions.Column = new PlotOptionsColumn
            {
                AllowPointSelect = true,
                Cursor = Cursors.Pointer,
                DataLabels = new PlotOptionsSeriesDataLabels
                {
                    Enabled = true,
                    Formatter = "function(){return this.y.format2d();}",
                    Style = "fontWeight: 'bold'"
                },
                PointPadding = pointWidth,
                BorderWidth = 0,
            };

            ////this.Legend.Enabled = true;
            this.Series = this.Series ?? new Series();
            this.Series.Type = ChartTypes.Column;
        }
    }
}