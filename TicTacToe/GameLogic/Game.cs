using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	class Game
	{
		private Player[] _players;

		private uint _current;

		private char[,] _board;

		private uint _bot;

		private List<string> _availablePos;

		private bool _botIsPlaying;

		public Game(bool botIsFirst, uint difficulty)
		{
			SetupBoard();
			_players = new Player[2];

			if (botIsFirst)
			{
				_players[0] = new Bot(TTTconstants.X_Tag, true, difficulty);
				_players[1] = new Player(TTTconstants.O_Tag);
				_bot = 0;
			}
			else
			{
				_players[0] = new Player(TTTconstants.X_Tag);
				_players[1] = new Bot(TTTconstants.O_Tag, false, difficulty);
				_bot = 1;
			}

			_current = 0;
			_botIsPlaying = true;
		}

		public Game()
		{
			SetupBoard();
			_players = new Player[2];

			_players[0] = new Player(TTTconstants.X_Tag);
			_players[1] = new Player(TTTconstants.O_Tag);

			_current = 0;
			_botIsPlaying = false;
		}

		/*public Game(bool botIsFirst, uint difficulty)
		{
			SetupBoard();
			_players = new Player[2];

			if (botIsFirst)
			{
				_players[0] = new Player('X', true, difficulty);
				_players[1] = new Player('O', false, 0);
				_bot = 0;
			}
			else
			{
				_players[0] = new Player('O', false, 0);
				_players[1] = new Player('X', true, difficulty);
				_bot = 1;
			}
			_current = 0;
			_botIsPlaying = true;
		}

		public Game()
		{
			SetupBoard();

			_players = new Player[2];

			_players[0] = new Player('X', false, 0);
			_players[1] = new Player('O', false, 0);

			_current = 0;
			_botIsPlaying = false;
		}*/

		public void SetupBoard()
		{
			_board = new char[3, 3];
			_availablePos = new List<string>();

			for (int col = 0; col < 3; col++)
			{
				for (int row = 0; row < 3; row++)
				{
					_board[col, row] = TTTconstants.EmptyTag;
					_availablePos.Add($"{col}{row}");
				}
			}
		}

		private bool AreEqual(char a, char b, char c)
		{
			return a == b && b == c && a != TTTconstants.EmptyTag;
		}

		public char CheckGameState()
		{
			for (int col = 0; col < 3; col++)   //vertical
				if (AreEqual(_board[col, 0], _board[col, 1], _board[col, 2]))
					return _board[col, 0];

			for (int row = 0; row < 3; row++)   //horizontal
				if (AreEqual(_board[0, row], _board[1, row], _board[2, row]))
					return _board[0, row];

			if (AreEqual(_board[0, 0], _board[1, 1], _board[2, 2])) //diagonal
				return _board[0, 0];

			if (AreEqual(_board[2, 0], _board[1, 1], _board[0, 2]))  //diagonal
				return _board[2, 0];

			int filledPos = 0;
			for (int col = 0; col < 3; col++)
				for (int row = 0; row < 3; row++)
					if (_board[col, row] != TTTconstants.EmptyTag)
						filledPos++;

			if (filledPos == 9)
				return TTTconstants.TieTag;  

			return TTTconstants.ContinueTag; 
		}

		private int GetFeedback(char state)
		{
			if (state == _players[_bot].Symbol)
				return 1;
			else if (state == _players[1 - _bot].Symbol )
				return -1;
			else
				return 0;
		}

		public void UpdatePlayer()
		{
			//if (_current == 0) _current = 1;

			//else _current = 0;
			_current = 1 - _current;
		}

		public string GetEasyBotPlay()
		{
			Random rand = new Random();
			int index = rand.Next(_availablePos.Count);
			return _availablePos[index];
		}

		public string GetHardBotPlay()
		{
			int score = Int32.MinValue;
			string play = "00";

			foreach (string pos in _availablePos)
			{
				_board[pos[0]-48, pos[1]-48] = _players[_bot].Symbol;
				int newScore = Minimax(_board, false);
				_board[pos[0]-48, pos[1]-48] = TTTconstants.EmptyTag;

				if (newScore > score)
				{
					score = newScore;
					play = pos;
				}

			}

			return play;
		}

		public int Minimax(char[,] boardState, bool isMaximizing)
		{
			int score = Int32.MinValue;
			uint player = _bot;
			char state = CheckGameState();

			if (state != TTTconstants.ContinueTag) return GetFeedback(state);

			if (!isMaximizing)
			{
				score = Int32.MaxValue;
				player = 1-_bot;
			}

			for (int col = 0; col < 3; col++)
			{
				for (int row = 0; row < 3; row++)
				{
					if (boardState[col, row] == TTTconstants.EmptyTag)
					{
						boardState[col, row] = _players[player].Symbol;
						int newScore = Minimax(boardState, !isMaximizing);

						if (isMaximizing) score = Math.Max(score, newScore);

						else score = Math.Min(score, newScore);

						boardState[col, row] = TTTconstants.EmptyTag;
					}
				}
			}

			return score;
		}

		public Player[] Players
		{
			get { return _players; }
		}

		public char[,] Board
		{
			get { return _board; }
		}

		public uint Current
		{
			get { return _current; }

			set { _current = value; }
		}
		
		public uint Bot
		{
			get { return _bot; }
		}

		public void UpdateBoard(string pos)
		{
			_board[pos[0] - 48, pos[1] - 48] = _players[_current].Symbol;
			_availablePos.Remove(pos);
		}

		public string BotTurn()
		{
			string pos;

			if (CheckGameState() != TTTconstants.ContinueTag) return null;

			if (((Bot)_players[_bot]).Difficulty == TTTconstants.BotEasyDifficulty)  
					pos = GetEasyBotPlay();

			/*else (_players[_bot].Difficulty == TTTconstants.BotMediumDifficulty)
					break;*/
			
			else pos = GetHardBotPlay();
			
			UpdateBoard(pos);
			UpdatePlayer();

			return pos;
		}

		public bool BotIsPlaying
		{
			get { return _botIsPlaying; }
		}
	}
}
