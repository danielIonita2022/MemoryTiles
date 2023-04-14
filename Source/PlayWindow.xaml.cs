using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using Path = System.IO.Path;

namespace MemoryTiles
{
	public partial class PlayWindow : Window
	{
		private User currentUser;
		private const string USERPATH = @"../../UserData";
		private DispatcherTimer timer;
		public bool IsTimerRunning { get; set; } = false;

		public string CurrentUserName
		{
			get
			{
				return currentUser.Username;
			}
		}
		public string CurrentUserImage
		{
			get
			{
				return currentUser.UserImage;
			}
		}
		public int Moves { get; set; } = 0;
		public Tile[] TilePair { get; set; }
		public Button[] ButtonPair { get; set; }
		public int CurrentLevel { get; set; }
		public bool IsWon { get; set; } = false;
		public int CountdownSeconds { get; set; }
		public static int BoardRows { get; set; }
		public static int BoardColumns { get; set; }

		public PlayWindow(User currentUser)
		{
			BoardRows = 5;
			BoardColumns = 5;
			InitializeComponent();
			boardDataContext = new Board(true);
			DataContext = boardDataContext;
			grid.Visibility = Visibility.Collapsed;
			this.currentUser = currentUser;
			userNameField.Content = CurrentUserName;
			userAvatarField.Source = new BitmapImage(new Uri(CurrentUserImage, UriKind.Relative));
			levelField.Content = "";
			string userPath = USERPATH + "/" + currentUser.Username;
			if (!Directory.Exists(userPath))
			{
				openGameButton.IsEnabled = false;
			}
			
			UpdateInfoFromStatsFile();

			saveGameButton.IsEnabled = false;
		}

		private void UpdateInfoFromStatsFile()
		{
			string userStatsPath = USERPATH + "/" + currentUser.Username;
			string filePath = Path.Combine(userStatsPath, "stats.txt");
			string savePath = Path.Combine(userStatsPath, "savefile.xml");
			if (!Directory.Exists(userStatsPath))
			{
				Directory.CreateDirectory(userStatsPath);
				File.Create(filePath).Close();
				File.Create(savePath).Close();
			}
			string[] userData = File.ReadAllLines(filePath);
			if (userData.Length != 0)
			{
				string[] data = userData[0].Split(',');
				currentUser.GamesPlayed = int.Parse(data[1]);
				currentUser.GamesWon = int.Parse(data[2]);
			}
		}

		private void WriteToStatsFile()
		{
			string userStatsPath = USERPATH + "/" + currentUser.Username;
			string filePath = Path.Combine(userStatsPath, "stats.txt");
			File.WriteAllText(filePath, string.Empty);
			using (StreamWriter sw = new StreamWriter(filePath, true))
			{
				sw.WriteLine(currentUser.Username + "," + currentUser.GamesPlayed + "," + currentUser.GamesWon);
			}
		}
		private void StartTimer()
		{
			CountdownSeconds = BoardRows * BoardColumns * (4 - CurrentLevel) * 5;
			timer = new DispatcherTimer();
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Stop();
			timer.Start();
			IsTimerRunning = true;
		}
		private void NewGame_Clicked(object sender, RoutedEventArgs e)
		{
			grid.Visibility = Visibility.Visible;
			saveGameButton.IsEnabled = true;

			TilePair = new Tile[2];
			ButtonPair = new Button[2];
			CurrentLevel = 0;

			currentUser.GamesPlayed++;
			WriteToStatsFile();

			StartTimer();

			NextLevel(false);
		}
		private void SaveGame_Clicked(object sender, RoutedEventArgs e)
		{
			string userPath = USERPATH + "/" + currentUser.Username;
			if (!Directory.Exists(userPath))
			{
				Directory.CreateDirectory(userPath);
				openGameButton.IsEnabled = true;	
			}
			boardDataContext.FlipUnmatchedTiles();
			string filePath = Path.Combine(userPath, "savefile.xml");
			XmlSerializer serializer = new XmlSerializer(typeof(Board));
			using (TextWriter writer = new StreamWriter(filePath))
			{
				serializer.Serialize(writer, boardDataContext);
			}
			MessageBox.Show("Game successfully saved!");
			openGameButton.IsEnabled = true;
		}
		private void OpenGame_Clicked(object sender, RoutedEventArgs e)
		{
			saveGameButton.IsEnabled = true;
			string userPath = USERPATH + "/" + currentUser.Username;
			string filePath = Path.Combine(userPath, "savefile.xml");
			XmlSerializer serializer = new XmlSerializer(typeof(Board));
			using (TextReader reader = new StreamReader(filePath))
			{
				boardDataContext = (Board)serializer.Deserialize(reader);
			}
			MessageBox.Show("Game successfully loaded!");
			StartTimer();
			timer.Tick -= new EventHandler(timer_Tick);
			NextLevel(true);
		}

		private void Statistics_Clicked(object sender, RoutedEventArgs e)
		{
			StatsWindow statsWindow = new StatsWindow(currentUser);
			statsWindow.ShowDialog();
		}
		private void OnExitClick(object sender, EventArgs e)
		{
			Close();
		}

		private void Standard_Checked(object sender, RoutedEventArgs e)
		{
			MenuItem standard = sender as MenuItem;
			standard.IsChecked = true;
			if (custom != null)
			{
				custom.IsChecked = false;
			}
			BoardRows = 5;
			BoardColumns = 5;
		}
		private void Custom_Checked(object sender, RoutedEventArgs e)
		{
			MenuItem custom = sender as MenuItem;
			custom.IsChecked = true;
			standard.IsChecked = false;
			ConfigureBoardWindow configureBoard = new ConfigureBoardWindow();
			configureBoard.ShowDialog();
			BoardRows = configureBoard.RowsResult;
			BoardColumns = configureBoard.ColumnsResult;
		}

		private void OnAboutClick(object sender, EventArgs e)
		{
			var aboutWindow = new Window
			{
				Title = "About",
				Width = 400,
				Height = 300,
				Content = new Label { Content = "Nume student: Ionita Daniel-Andrei\n" +
				"Grupa: 10LF312\n" +
				"Specializarea: Informatica Aplicata" }
			};
			aboutWindow.Show();
		}

		public void NextLevel(bool loadedLevel)
		{
			CurrentLevel++;
			if (CurrentLevel == 4)
			{
				timer.Stop();
				IsTimerRunning = false;
				IsWon = true;
				currentUser.GamesWon++;
				WriteToStatsFile();
				FinishWindow winnerWindow = new FinishWindow(currentUser, true);
				winnerWindow.ShowDialog();
			}
			else if (!loadedLevel)
			{
				levelField.Content = "Level: " + CurrentLevel.ToString();
				boardDataContext = new Board(CurrentLevel);
				DataContext = boardDataContext;
				Moves = 0;
			}
			else if (loadedLevel)
			{
				BoardRows = boardDataContext.Rows;
				BoardColumns = boardDataContext.Columns;

				CurrentLevel = boardDataContext.CurrentLevel;
				levelField.Content = "Level: " + CurrentLevel.ToString();

				userNameField.Content = CurrentUserName;
				userAvatarField.Source = new BitmapImage(new Uri(CurrentUserImage, UriKind.Relative));

				DataContext = boardDataContext;
				Moves = 0;

				TilePair = new Tile[2];
				ButtonPair = new Button[2];

				grid.Visibility = Visibility.Visible;
			}
		}

		private bool IsSolved()
		{
			foreach (var list in boardDataContext.TileItems)
			{
				foreach (var tile in list)
				{
					if (!tile.IsFlipped)
					{
						return false;
					}
				}
			}
			return true;
		}


		private bool IsSameImage(Tile firstTile, Tile secondTile)
		{
			if (firstTile.FrontImage == secondTile.FrontImage)
			{
				return true;
			}
			return false;
		}

		private void FlipTile_Clicked(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			Tile clickedTile = button.DataContext as Tile;

			if (clickedTile.IsFlipped)
			{
				return;
			}
			else
			{
				if (Moves < 2)
				{
					clickedTile.IsFlipped = true;
					TilePair[Moves] = clickedTile;
					ButtonPair[Moves] = button;
					Moves++;
					//change image from backFace to frontFace
					Tile newTile = new Tile(clickedTile);
					newTile.CurrentImagePath = newTile.FrontImage;
					button.DataContext = newTile;
					button.InvalidateVisual();
				}
				else
				{
					if (!IsSameImage(TilePair[0], TilePair[1]))
					{
						TilePair[0].IsFlipped = false;
						TilePair[0].CurrentImagePath = TilePair[0].BackImage;
						Button firstButton = ButtonPair[0];
						firstButton.DataContext = TilePair[0];
						firstButton.InvalidateVisual();

						TilePair[1].IsFlipped = false;
						TilePair[1].CurrentImagePath = TilePair[1].BackImage;
						Button secondButton = ButtonPair[1];
						secondButton.DataContext = TilePair[1];
						secondButton.InvalidateVisual();
					}
					else
					{
						TilePair[0].IsMatched = true;
						TilePair[1].IsMatched = true;
					}

					Moves = 0;
					clickedTile.IsFlipped = true;
					TilePair[Moves] = clickedTile;
					ButtonPair[Moves] = button;
					Moves++;
					Tile newTile = new Tile(clickedTile);
					newTile.CurrentImagePath = newTile.FrontImage;
					button.DataContext = newTile;
					button.InvalidateVisual();
				}
				if (IsSolved())
				{
					Thread.Sleep(1200);
					NextLevel(false);
				}
			}
		}
		void timer_Tick(object sender, EventArgs e)
		{
			CountdownSeconds--;
			if (IsTimerRunning)
			{
				if (CountdownSeconds < 0)
				{
					// Countdown is over, stop the timer
					timer.Stop();
					timeField.Content = "Game Lost!";
					grid.Visibility = Visibility.Collapsed;
					IsTimerRunning = false;
					FinishWindow lostWindow = new FinishWindow(currentUser, false);
					lostWindow.ShowDialog();
				}
				else
				{
					// Update the display with the remaining time
					int minutes = CountdownSeconds / 60;
					int seconds = CountdownSeconds % 60;
					timeField.Content = string.Format("{0:00}:{1:00}", minutes, seconds);
				}
			}
		}
	}
}
