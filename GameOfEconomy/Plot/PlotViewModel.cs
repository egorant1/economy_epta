﻿using OxyPlot;
using OxyPlot.Series;

namespace GameOfEconomy
{
    class PlotViewModel
    {
        /*

        public PlotViewModel()
        {
            var tmp = new PlotModel { Title = "Simple example", Subtitle = "using OxyPlot" };

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);

            // Axes are created automatically if they are not defined

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            this.Model = tmp;
        }
        */

        public PlotViewModel(DataGridRow row)
        {
            var tmp = new PlotModel { Title = row.Name, Subtitle = row.LongName};

            var series = new LineSeries {MarkerType = MarkerType.Circle };

            int i = 0;
            while (row[i] != null)
            {
                series.Points.Add(new DataPoint (i, double.Parse(row[i])));
                i++;
            }

            tmp.Series.Add(series);

            this.Model = tmp;
        }


        /// <summary>
        /// Gets the plot model.
        /// </summary>
        public PlotModel Model { get; private set; }

    }
}
