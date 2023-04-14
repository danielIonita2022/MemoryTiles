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
	public partial class FinishWindow : Window
	{
		public User CurrentUser { get; set; }
		public FinishWindow(User user, bool winOrLose)
		{
			InitializeComponent();
			CurrentUser = user;
			if (winOrLose)
			{
				finalMessage.Content = "Congratulations, " + CurrentUser.Username + "! You won!";
			}
			else
			{
				finalMessage.Content = "You lost!";
			}
		}

		private void OK_Clicked(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
