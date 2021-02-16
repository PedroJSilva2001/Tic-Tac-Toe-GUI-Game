using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	public class Player
	{
		private uint _points;

		private char _symbol;

		//private bool _isBot;

		//private uint _difficulty;

		public Player(char symbol, uint points = 0)
		{
			_points = points;
			_symbol = symbol;
		}
		/*public Player(char symbol, bool isBot, uint difficulty)
		{
			_points = 0;
			_symbol = symbol;
			_isBot = isBot;
			_difficulty = difficulty;
		}*/

		public uint Points
		{
			get { return _points; }

			set { _points = value; }
		}

		public char Symbol
		{
			get { return _symbol; }
		}

		/*public bool IsBot
		{
			get { return _isBot; }

			set { _isBot = value; }
		}

		public uint Difficulty
		{
			get { return _difficulty; }

			set { _difficulty = value; }
		}*/
	}
}
