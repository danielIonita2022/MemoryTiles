using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MemoryTiles
{
	[Serializable]
	public class Tile
	{
		public Tile()
		{
			FrontImage = string.Empty;
			BackImage = string.Empty;
			IsFlipped = false;
			IsMatched = false;
		}
		public Tile(string frontImage, string backImage)
		{
			FrontImage = frontImage;
			BackImage = backImage;
			IsFlipped = false;
			IsMatched = false;
		}

		public Tile(Tile tile)
		{
			FrontImage = tile.FrontImage;
			BackImage = tile.BackImage;
			IsFlipped = tile.IsFlipped;
			IsMatched = tile.IsMatched;
		}

		public string FrontImage { get; set; }

		public string BackImage { get; set; }

		public bool IsFlipped { get; set; }
		public bool IsMatched { get; set; }
		public string CurrentImagePath
		{
			get
			{
				if (IsFlipped)
				{
					return FrontImage;
				}
				else
				{
					return BackImage;
				}
			}
			set { }
		}
	}
}
