using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace GameOfEconomy
{
    class DataGridWrapper
    {
        public DataGrid dataGrid;

        private List<DataGridRow> rows;

        public DataGridWrapper(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
            this.rows = new List<DataGridRow>();
        }

        public void AddColum(int year)
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = $"Year {year}";
            column.Binding = new Binding($"Year{year}");
            dataGrid.Columns.Add(column);
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
