// --------------------------------------------------------------------------
// <copyright file="BaseChartValues.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Common.Highcharts
{
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using DotNet.Highcharts.Options;

    /// <summary>
    /// Base Chart Values.
    /// </summary>
    public abstract class BaseChartValues
    {
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public BaseChartValues(string titleText, string subtitleText, string[] categories, string titleY)
        {
            this.Chart = new Chart()
            {
                ClassName = "chart"
            };

            this.Title = new Title
            {
                Text = titleText
            };

            this.Subtitle = new Subtitle
            {
                Text = subtitleText
            };

            this.XAxis = new XAxis
            {
                Categories = categories
            };

            this.YAxis = new YAxis
            {
                Title = new YAxisTitle
                {
                    Text = titleY,
                },
                PlotLines = new[]
                {
                    new YAxisPlotLines
                    {
                        Value = 0,
                        Width = 1,
                        Color = ColorTranslator.FromHtml("#808080")
                    }
                },
                Min = 0,
            };

            this.PlotOptions = this.PlotOptions ?? new PlotOptions();

            this.PlotOptions.Spline = this.PlotOptions.Spline ?? new PlotOptionsSpline
            {
                Marker = new PlotOptionsSeriesMarker
                {
                    Radius = 4,
                    LineColor = ColorTranslator.FromHtml("#666666"),
                    LineWidth = 1
                }
            };

            this.Credits = new Credits
            {
                Enabled = false
            };

            this.Tooltip = new Tooltip();

            this.Loading = new Loading
            {
                HideDuration = 1000,
                ShowDuration = 1000,
                LabelStyle = "top: '45%'"
            };
        }

        public Chart Chart
        {
            get;
            protected set;
        }

        public Title Title
        {
            get;
            protected set;
        }

        public Subtitle Subtitle
        {
            get;
            protected set;
        }

        public XAxis XAxis
        {
            get;
            protected set;
        }

        public YAxis YAxis
        {
            get;
            protected set;
        }

        public Tooltip Tooltip
        {
            get;
            protected set;
        }

        public Legend Legend
        {
            get;
            protected set;
        }

        public Credits Credits
        {
            get;
            protected set;
        }

        public PlotOptions PlotOptions
        {
            get;
            protected set;
        }

        public Loading Loading
        {
            get;
            protected set;
        }

        public Series Series
        {
            get;
            protected set;
        }

        /// <summary>
        /// Adds close button to the chart.
        /// </summary>
        /// <returns>Base Chart Values.</returns>
        public BaseChartValues AddCloseButton()
        {
            this.Chart.Events = new ChartEvents
            {
                Load = "function(){onLoad.call(this);}",
                Redraw = "function(){onRedraw.call(this);}"
            };

            return this;
        }

        /// <summary>
        /// Registers click event handler for points of chart.
        /// </summary>
        /// <param name="clickFunction">The name of JavaScript function that handles the event.</param>
        /// <returns>Base Chart Values.</returns>
        public BaseChartValues AddPointClickEvent(string clickFunction)
        {
            this.PlotOptions = this.PlotOptions ?? new PlotOptions();
            this.PlotOptions.Series = this.PlotOptions.Series ?? new PlotOptionsSeries();
            this.PlotOptions.Series.Point = new PlotOptionsSeriesPoint
                {
                    Events = new PlotOptionsSeriesPointEvents
                    {
                        Click = clickFunction
                    }
                };

            return this;
        }

        /// <summary>
        /// Registers Legend Click event handler.
        /// </summary>
        /// <param name="clickFunction">
        /// The name of JavaScript function which takes responsibility for handling
        /// Legend Click event.
        /// </param>
        /// <returns>Base Chart Value.</returns>
        public BaseChartValues LegendItemClick(string clickFunction)
        {
            this.PlotOptions = this.PlotOptions ?? new PlotOptions();
            this.PlotOptions.Series = this.PlotOptions.Series ?? new PlotOptionsSeries();
            this.PlotOptions.Series.Events = new PlotOptionsSeriesEvents
                {
                    LegendItemClick = clickFunction
                };

            return this;
        }

        /// <summary>
        /// Sets series of base chart.
        /// </summary>
        /// <param name="points">List of points.</param>
        /// <returns>Bar Char Value.</returns>
        public BaseChartValues SetSeries(DotNet.Highcharts.Options.Point[] points)
        {
            this.Series = this.Series ?? new Series();
            this.Series.Data = new DotNet.Highcharts.Helpers.Data(points);

            return this;
        }
    }
}