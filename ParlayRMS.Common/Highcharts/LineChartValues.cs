namespace ParlayRMS.Common.Highcharts
{
    using DotNet.Highcharts.Enums;
    using DotNet.Highcharts.Options;

    /// <summary>
    /// Line Chart Values.
    /// </summary>
    public class LineChartValues : BaseChartValues
    {
        public LineChartValues(string titleText, string subtitleText, string[] categories, string titleY)
            : base(titleText, subtitleText, categories, titleY)
        {
            this.Chart = this.Chart ?? new Chart();
            this.Chart.DefaultSeriesType = ChartTypes.Line;
            this.Chart.MarginRight = 130;
            this.Chart.MarginBottom = 25;
            this.PlotOptions.Series = this.PlotOptions.Series ?? new PlotOptionsSeries
            {
                Marker = new PlotOptionsSeriesMarker
                {
                    LineWidth = 1
                },
                Cursor = Cursors.Pointer
            };
            this.Title = this.Title ?? new Title();
            this.Title.X = -20;

            this.Subtitle = this.Subtitle ?? new Subtitle();
            this.Subtitle.X = -20;

            this.Legend = this.Legend ?? new Legend();
            this.Legend.Layout = Layouts.Vertical;
            this.Legend.Align = HorizontalAligns.Right;
            this.Legend.VerticalAlign = VerticalAligns.Top;
            this.Legend.X = -10;
            this.Legend.Y = 100;
            this.Legend.BorderWidth = 0;

            this.Series = this.Series ?? new Series();
            this.Series.Type = ChartTypes.Line;
            this.Loading = this.Loading;
        }
    }
}