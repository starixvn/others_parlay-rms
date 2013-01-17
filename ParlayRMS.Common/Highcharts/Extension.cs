// -----------------------------------------------------------------------
// <copyright file="HighchartsExtension.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Common.Highcharts
{
    using DotNet.Highcharts;
    using DotNet.Highcharts.Helpers;
    using DotNet.Highcharts.Options;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Extension
    {
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