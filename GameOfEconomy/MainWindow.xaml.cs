using GameOfEconomy.MainLogic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GameOfEconomy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentStep;

        private DataGridWrapper gridWrapper;
        private Game game;

        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            gridWrapper = new DataGridWrapper(MainTable);
            game = new Game(gridWrapper);
        }


        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            game.SimulateNextStep();
        }
       

    }
}
