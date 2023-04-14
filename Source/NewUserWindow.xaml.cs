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
using System.IO;

namespace MemoryTiles
{
	public partial class NewUserWindow : Window
	{
		private List<string> imageList = new List<string>()
		{
			"UserImages/avatar_alien.png",
			"UserImages/avatar_dog.png",
			"UserImages/avatar_frog.png",
			"UserImages/avatar_panda.png",
			"UserImages/avatar_penguin.png",
			"UserImages/avatar_pumpkin.png",
			"UserImages/avatar_rabbit.png",
			"UserImages/avatar_robot.png"
		};
		private string username;
		private int imageIndex = 0;

		private bool validUser;
		public bool ValidUser 
		{
			get { return validUser; }
			set { validUser = value; }
		}

		public NewUserWindow()
		{
			InitializeComponent();
			UpdateAvatar();
		}

		private void UpdateUsername(object sender, TextChangedEventArgs e)
		{
			username = (sender as TextBox).Text.ToString();
		}
		private void UpdateAvatar()
		{
			string imagePath = imageList[imageIndex];
			ImageControl.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
		}
		private void PreviousImage_Click(object sender, RoutedEventArgs e)
		{
			if (imageIndex > 0)
			{
				--imageIndex;
			}
			else imageIndex = imageList.Count - 1;
			UpdateAvatar();
		}

		private void NextImage_Click(object sender, RoutedEventArgs e)
		{
			if (imageIndex < imageList.Count - 1)
			{
				++imageIndex;
			}
			else imageIndex = 0;
			UpdateAvatar();
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			if (username != null)
			{
				const string FILEPATH = @"../../UserList.txt";
				using (StreamWriter sw = new StreamWriter(FILEPATH, true))
				{
					sw.WriteLine(username + "," + imageList[imageIndex]);
				}
				ValidUser = true;
				Close();
			}
			else
			{
				MessageBox.Show("Invalid username!");
				ValidUser = false;
			}
		}
	}
}
