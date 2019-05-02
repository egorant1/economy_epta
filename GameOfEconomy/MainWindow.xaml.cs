using GameOfEconomy.MainLogic;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace GameOfEconomy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DataGridWrapper gridWrapper;
        public Game game;

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

        private void MainTable_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            // Canceling editing to make dialog
            e.Cancel = true;

            Binding binding = ((DataGridTextColumn)e.Column).Binding as Binding;
            DataGridRow row = e.Row.Item as DataGridRow;

            if (int.TryParse(binding.Path.Path.Substring(4), out int yearIndex))
            { 

                if (game.IsVariableInstr(row.Name))
                    ShowEditDialog(row, yearIndex);
                else
                    new NotEditableDialog().Show();
            }
            else
            {
                ChartWindow chartWindow = new ChartWindow(row);
                chartWindow.Show();
            }

        }

        private void ShowEditDialog(DataGridRow row, int yearIndex)
        {
            EditDialog dialog = new EditDialog(row.Name, float.Parse(row[yearIndex]));
            dialog.Show();

            dialog.Closed += (object s, System.EventArgs closedEvent) =>
            {
                row[yearIndex] = dialog.Value.ToString();
                gridWrapper.Apply();
            };
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                {
                    var serializer = new XmlSerializer(typeof(DataGridRow[]));
                    serializer.Serialize(writer, gridWrapper.rows.ToArray());
                    writer.Flush();
                }
            }
        }
        

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            LoadGame();
        }

        public void LoadGame()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    var serializer = new XmlSerializer(typeof(DataGridRow[]));
                    gridWrapper.rows = ((DataGridRow[])serializer.Deserialize(reader)).ToList();
                    gridWrapper.Apply();

                    int i = 0;
                    while (gridWrapper.rows[0][i++] != null) { }
                    i--;

                    game.MoveCarriageTo(i);

                    reader.Close();
                }
            }
        }

    }
}
