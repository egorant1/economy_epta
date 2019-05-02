using System.Windows;

namespace GameOfEconomy
{
    /// <summary>
    /// Interaction logic for NotEditableDialog.xaml
    /// </summary>
    public partial class NotEditableDialog : Window
    {
        public NotEditableDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
