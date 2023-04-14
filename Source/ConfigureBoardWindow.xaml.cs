using System;
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

namespace MemoryTiles
{
	/// <summary>
	/// Interaction logic for ConfigureBoardWindow.xaml
	/// </summary>
	public partial class ConfigureBoardWindow : Window
	{
		private int rowsResult;
		private int columnsResult;
		public ConfigureBoardWindow()
		{
			InitializeComponent();
		}

		public int RowsResult
		{
			get 
			{
				if (rowsResult == 0)
					return 5;
				return rowsResult; 
			}
		}
		public int ColumnsResult
		{
			get 
			{
				if(columnsResult == 0)
					return 5;
				return columnsResult; 
			}
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			int rowsResult, columnsResult;
			if (int.TryParse(rows.Text, out rowsResult))
			{
				if (rowsResult < 2 || rowsResult > 8)
				{
					MessageBox.Show("The number of rows must be between 2 and 7!");
					return;
				}
			}
			else
			{
				MessageBox.Show("Please enter a valid number of rows");
				return;
			}
			if (int.TryParse(columns.Text, out columnsResult))
			{
				if (columnsResult < 2 || columnsResult > 8)
				{
					MessageBox.Show("The number of columns must be between 2 and 7!");
					return;
				}
			}
			else
			{
				MessageBox.Show("Please enter a valid number of columns");
				return;
			}
			this.rowsResult = rowsResult;
			this.columnsResult = columnsResult;
			Close();
		}
    }
}
