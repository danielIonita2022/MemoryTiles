using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemoryTiles
{
	public partial class MainWindow : Window
	{
		private List<User> userList;
		private User currentUser;

		public MainWindow()
		{
			InitializeComponent();
			playButton.IsEnabled = false;
			deleteUserButton.IsEnabled = false;
			userList = new List<User>();
			ParseUserListFile(@"../../UserList.txt");
			userListView.ItemsSource = userList;
		}
		private void ParseUserListFile(string filename)
		{
			string[] lines = File.ReadAllLines(filename);
			foreach (string line in lines)
			{
				string[] parts = line.Split(',');
				string username = parts[0];
				string userImage = parts[1];
				User user = new User(username, userImage);
				userList.Add(user);
			}
		}

		private void GetLastUserFromFile(string filename)
		{
			string[] lines = File.ReadAllLines(filename);
			string lastLine = lines[lines.Length - 1];
			string[] parts = lastLine.Split(',');
			string username = parts[0];
			string userImage = parts[1];
			User user = new User(username, userImage);
			userList.Add(user);
		}

		private void NewUser_Button_Click(object sender, RoutedEventArgs e)
		{
			Hide();
			NewUserWindow newUserWindow = new NewUserWindow();
			newUserWindow.ShowDialog();
			if (newUserWindow.ValidUser)
			{
				GetLastUserFromFile(@"../../UserList.txt");
				userListView.Items.Refresh();
			}
			Show();
		}

		private void DeleteUserFromFile(string filepath)
		{
			File.WriteAllText(filepath, string.Empty);
			foreach (User user in userList)
			{
				using (StreamWriter sw = new StreamWriter(filepath, true))
				{
					sw.WriteLine(user.Username + "," + user.UserImage);
				}
			}
		}

		private void Delete_Button_Clicked(object sender, RoutedEventArgs e)
		{
			if (currentUser != null)
			{
				string userPath = @"../../UserData/" + currentUser.Username;
				if (Directory.Exists(userPath))
				{
					Directory.Delete(userPath, true);
				}
				userList.Remove(currentUser);
				userListView.Items.Refresh();
				DeleteUserFromFile(@"../../UserList.txt");
				
			}
		}

		private void Exit_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		private void UserView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			currentUser = userListView.SelectedItem as User;
			if (currentUser != null)
			{ 
				string imagePath = currentUser.UserImage;
				ImageControl.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
				playButton.IsEnabled = true;
				deleteUserButton.IsEnabled = true;
			}	
		}

		private void playButton_Click(object sender, RoutedEventArgs e)
		{
			Hide();
			PlayWindow playWindow = new PlayWindow(currentUser);
			playWindow.ShowDialog();
			Show();
		}
	}
}
