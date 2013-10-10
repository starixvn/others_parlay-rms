// --------------------------------------------------------------------------
// <copyright file="Extension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Common.Highcharts
{
    using DotNet.Highcharts;
    using DotNet.Highcharts.Helpers;
    using DotNet.Highcharts.Options;

    /// <summary>
    /// Extend High Chart functionalities.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Initializes High Chart values.
        /// </summary>
        /// <param name="chart">HighChart object that the method expected to extend to.</param>
        /// <param name="values">Based chart's values.</param>
        /// <returns>Initialized HighChart object.</returns>
        public static Highcharts InitValues(this Highcharts chart, BaseChartValues values)
        {
            chart.InitChart(values.Chart)
                .SetTitle(values.Title)
                .SetSubtitle(values.Subtitle)
                .SetXAxis(values.XAxis)
                .SetYAxis(values.YAxis)
                .SetTooltip(values.Tooltip)
                .SetLegend(values.Legend)
                .SetLoading(values.Loading)
                .SetCredits(values.Credits)
                .SetPlotOptions(values.PlotOptions)
                .AddJavascripVariable("colors", "Highcharts.getOptions().colors");

            if (values.Series.Data != null)
            {
                chart.SetSeries(values.Series);
            }

            return chart;
        }

        /// <summary>
        /// Sets the series for a HighChart.
        /// </summary>
        /// <param name="chart">HighChart object that the method expected to extend to.</param>
        /// <param name="listSeriesName">List of series' name.</param>
        /// <param name="listSeriesData">List of series' data.</param>
        /// <returns>Updated HighChart object.</returns>
        public static Highcharts SetSeries(this Highcharts chart, string[] listSeriesName, object[] listSeriesData)
        {
            Series[] series = new Series[listSeriesData.Length];
            int counter = 0;

            foreach (object item in listSeriesData)
            {
                series[counter] = new Series
                {
                    Name = listSeriesName[counter],
                    Data = new Data((object[])item)
                };
                counter++;
            }

            chart.SetSeries(series);

            return chart;
        }

        /// <summary>
        /// Sets the series for a HighChart.
        /// </summary>
        /// <param name="chart">HighChart object that the method expected to extend to.</param>
        /// <param name="listSeriesName">List of series' name.</param>
        /// <param name="listSeriesData">List of series' data.</param>
        /// <param name="listStates">List of states.</param>
        /// <returns>Updated HighChart object.</returns>
        public static Highcharts SetSeries(this Highcharts chart, string[] listSeriesName, object[] listSeriesData, bool[] listStates)
        {
            Series[] series = new Series[listSeriesData.Length];
            int counter = 0;

            foreach (object item in listSeriesData)
            {
                series[counter] = new Series
                {
                    Name = listSeriesName[counter],
                    PlotOptionsSeries = listStates[counter] ? null : new PlotOptionsSeries
                    {
                        Visible = listStates[counter]
                    },
                    Data = new Data((object[])item)
                };
                counter++;
            }

            chart.SetSeries(series);

            return chart;
        }
    }
}