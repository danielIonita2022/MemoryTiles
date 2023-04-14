using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace MemoryTiles
{
	[Serializable]
	public class Board
	{
		[XmlArray]
		public List<Tile> TileList { get; set; }

		[XmlAttribute("rows")]
		public int Rows { get; set; }
		[XmlAttribute("columns")]
		public int Columns { get; set; }

		[XmlAttribute]
		public int CurrentLevel { get; set; }

		public List<List<Tile>> TileItems { get; set; }
		
		public Board()
		{
			TileList = new List<Tile>();
			TileItems = new List<List<Tile>>();
		}
		public Board(bool firstTimeInit)
		{
			Rows = PlayWindow.BoardRows;
			Columns = PlayWindow.BoardColumns;
			TileList = new List<Tile>(Rows * Columns);
			if(CurrentLevel == 0)
				CurrentLevel = 1;
			string tileImagePath = @"D:\SEM2\MVP\MemoryTiles\MemoryTiles\TileImages\level" + CurrentLevel.ToString() + "\\";
			string[] imageFilenames = Directory.GetFiles(tileImagePath, "*.jpg");
			GenerateRandomTiles(imageFilenames);

			TileItems = new List<List<Tile>>();
			for (int i = 0; i < Rows; i++)
			{
				TileItems.Add(new List<Tile>());
				for (int j = 0; j < Columns; j++)
				{
					if(Rows * Columns % 2 == 1 && i == Rows - 1 && j == Columns - 1)
						TileItems[i].Add(new Tile());
					else
						TileItems[i].Add(new Tile(TileList[i * Rows + j]));
				}
			}
		}
		public Board(int level)
		{
			CurrentLevel = level;
			Rows = PlayWindow.BoardRows;
			Columns = PlayWindow.BoardColumns;
			TileList = new List<Tile>(Rows*Columns);

			string tileImagePath = @"D:\SEM2\MVP\MemoryTiles\MemoryTiles\TileImages\level" + CurrentLevel.ToString();
			string[] imageFilenames = Directory.GetFiles(tileImagePath, "*.jpg");
			GenerateRandomTiles(imageFilenames);

			TileItems = new List<List<Tile>>();
			for (int i = 0; i < Rows; i++)
			{
				TileItems.Add(new List<Tile>());
				for (int j = 0; j < Columns; j++)
				{
					TileItems[i].Add(new Tile(TileList[i * Columns + j]));
				}
			}
		}

		private void ShuffleTileList()
		{
			Random random = new Random();
			int n = TileList.Count;
			while (n > 1)
			{
				n--;
				int k = random.Next(n + 1);
				Tile tile = TileList[k];
				TileList[k] = TileList[n];
				TileList[n] = tile;
			}
		}

		private void GenerateRandomTiles(string[] imageFilenames)
		{
			HashSet<int> randomIndexSet = new HashSet<int>();
			Random rand = new Random();
			string backImageFile = @"D:\SEM2\MVP\MemoryTiles\MemoryTiles\TileImages\backface256.jpg";
			for (int i = 0; i < Rows * Columns / 2; i++)
			{
				int randomIndex;
				do
				{
					randomIndex = rand.Next(imageFilenames.Length);

				} while (randomIndexSet.Contains(randomIndex));

				randomIndexSet.Add(randomIndex);
				Tile newTile = new Tile(imageFilenames[randomIndex], backImageFile);
				TileList.Add(newTile);
				TileList.Add(newTile);
			}
			// daca numarul de tile-uri este impar, adaugam un tile suplimentar,
			// fara pereche
			if(Rows * Columns % 2 == 1)
			{
				int randomIndex;
				do
				{
					randomIndex = rand.Next(imageFilenames.Length);

				} while (randomIndexSet.Contains(randomIndex));

				randomIndexSet.Add(randomIndex);
				Tile newTile = new Tile(imageFilenames[randomIndex], backImageFile);
				TileList.Add(newTile);
			}
			ShuffleTileList();
		}

		public void FlipUnmatchedTiles()
		{
			foreach (var tileList in TileItems)
			{
				foreach (var tile in tileList)
				{
					if (tile.IsFlipped && !tile.IsMatched)
						tile.IsFlipped = false;
				}
			}
		}
	}
}
