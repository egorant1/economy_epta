using System;
using System.Collections.Generic;
using System.Windows;
using GameOfEconomy.MainLogic;
using OxyPlot;
using OxyPlot.Series;

namespace GameOfEconomy
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public ChartWindow()
        {
            InitializeComponent();
        }

        public ChartWindow(DataGridRow row)
        {

            InitializeComponent();

            this.DataContext = new PlotViewModel(row);
        }
    }
}
