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
	public partial class StatsWindow : Window
	{
		public User CurrentUser { get; set; }
		public StatsWindow(User currentUser)
		{
			InitializeComponent();
			CurrentUser = currentUser;
			userLabel.Content = CurrentUser.Username;
			playedLabel.Content = CurrentUser.GamesPlayed;
			wonLabel.Content = CurrentUser.GamesWon;
		}
	}
}
