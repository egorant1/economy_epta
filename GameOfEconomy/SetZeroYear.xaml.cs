using GameOfEconomy.MainLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameOfEconomy
{
    /// <summary>
    /// Interaction logic for SetZeroYear.xaml
    /// </summary>
    public partial class SetZeroYear : Window
    {

        MainWindow mainWindow;

        public SetZeroYear()
        {
            InitializeComponent();

            mainWindow = new MainWindow();
            InstrList.ItemsSource = mainWindow.game.instrVars;
        }

        public SetZeroYear(Game game)
        {
            InitializeComponent();

            InstrList.ItemsSource = game.instrVars;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<float> vs = new List<float>();

            foreach (object elem in InstrList.ItemsSource)
            {
                float.TryParse(((EInstrVar)elem).Value, out float tmp);
                vs.Add(tmp);
            }
            mainWindow.game.SetZeroYearInstrVariables(vs);
            mainWindow.Show();
            Close();
        }
    }
}
