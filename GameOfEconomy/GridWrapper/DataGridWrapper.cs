using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace GameOfEconomy
{
    public class DataGridWrapper
    {
        public DataGrid dataGrid;
        
        public List<DataGridRow> rows;

        public DataGridWrapper(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            rows = new List<DataGridRow>();
        }

        public void AddColum(int year)
        {
            DataGridTextColumn column = new DataGridTextColumn
            {
                Header = $"Year {year}",
                Binding = new Binding($"Year{year}")
            };
            dataGrid.Columns.Add(column);
        }

        public void PopColum()
        {
            dataGrid.Columns.RemoveAt(dataGrid.Columns.Count - 1);
        }
        
        public void AddRow(DataGridRow dataGridRow)
        {
            rows.Add(dataGridRow);
        }

        public void Apply()
        {
            dataGrid.Items.Clear();

            foreach (DataGridRow row in rows)
            {
                dataGrid.Items.Add(row);
            }
        }

        public DataGridRow this[int key]
        {
            get
            {
                return rows[key];
            }
            set
            {
                rows[key] = value;
            }
        }

    }
}
