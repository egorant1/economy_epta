﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditDialog.xaml
    /// </summary>
    public partial class EditDialog : Window
    {
        public float Value;

        public EditDialog()
        {
            InitializeComponent();
        }

        public EditDialog(string varname, float varvalue)
        {
            InitializeComponent();

            VariableName.Text = varname;
            VariableValue.Text = varvalue.ToString();
            VariableNewValue.Text = varvalue.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VariableNewValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            float.TryParse(VariableNewValue.Text, out Value);
        }
    }
}
