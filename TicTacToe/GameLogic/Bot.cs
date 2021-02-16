using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	public class Bot : Player
	{
		private uint _difficulty;

		private bool _isFirst;

		public Bot(char symbol, bool isFirst, uint difficulty, uint points = 0) :
			base(symbol, points)
		{
			_difficulty = difficulty;
			_isFirst = isFirst;
		}

		public uint Difficulty
		{
			get { return _difficulty; }
		}

		public bool IsFirst
		{
			get { return _isFirst; }
		}

	}
}
