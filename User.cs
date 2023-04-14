using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryTiles
{
	public class User
	{

		public User(string username, string userImage)
		{
			Username = username;
			UserImage = userImage;
			GamesWon = 0;
		}

		public string Username { get; set; }
		public string UserImage { get; set; }
		public int GamesWon { get; set; }
		public int GamesPlayed { get; set; }
	}
}
