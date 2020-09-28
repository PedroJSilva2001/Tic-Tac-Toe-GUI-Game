using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameLogic
{
	
	public partial class MainWindow : Window
	{
		public char[,] Board { get; set; } = { { ' ', ' ', ' ' },
			{ ' ', ' ', ' ' },
			{ ' ', ' ', ' ' }
		};
		public Dictionary<string, char> PlayerSymbols { get; set; } = new Dictionary<string, char>
		{
			["Human"] = 'X',
			["Bot"] = 'O'
		};
		public Dictionary<char, int> Scores { get; set; } = new Dictionary<char, int>
		{
			['X'] = 0,
			['O'] = 0
		};
		public List<string> AvailablePos = new List<string> {
		"00", "01", "02","10", "11", "12","20", "21", "22"
		};
		public bool BotIsFirst { get; set; } = false;
		public bool BotIsPlaying { get; set; } = true;
		public char CurrPlayer { get; set; } = 'X';
		public bool BotIsEasy { get; set; } = false;


		public bool AreEqual(char a, char b, char c)
		{
			return a == b && b == c && a != ' ';
		}

		public char CheckGameState()
		{
			for (int col = 0; col < 3; col++)   //vertical
				if (AreEqual(Board[col, 0], Board[col, 1], Board[col, 2]))
					return Board[col, 0];

			for (int row = 0; row < 3; row++)   //horizontal
				if (AreEqual(Board[0, row], Board[1, row], Board[2, row]))
					return Board[0, row];

			if (AreEqual(Board[0, 0], Board[1, 1], Board[2, 2])) //diagonal
				return Board[0, 0];

			if (AreEqual(Board[2, 0], Board[1, 1], Board[0, 2]))  //diagonal
				return Board[2, 0];

			int filledPos = 0;
			for (int col = 0; col < 3; col++)
				for (int row = 0; row < 3; row++)
					if (Board[col, row] != ' ')
						filledPos++;

			if (filledPos == 9)
				return 'T';

			return 'C';
		}

		public int GetPoints(char state, int depth)
		{
			if (state == PlayerSymbols["Bot"])
				return 10 - depth;
			else if (state == PlayerSymbols["Human"])
				return -10 + depth;
			else
				return 0;
		}

		public void UpdatePlayer()
		{
			if (CurrPlayer == 'X')
				CurrPlayer = 'O';
			else
				CurrPlayer = 'X';
		}

		public string GetEasyBotPlay()
		{
			Random rand = new Random();
			int pos = rand.Next(AvailablePos.Count);
			return AvailablePos[pos];
		}

		public string GetHardBotPlay()
		{
			int score = Int32.MinValue;
			string pos = "00";

			for (int col = 0; col < 3; col++)
				for (int row = 0; row < 3; row++)
					if (Board[col, row] == ' ')
					{
						Board[col, row] = PlayerSymbols["Bot"];
						int newScore = Minimax(Board, 0, false);
						Board[col, row] = ' ';
						if (newScore > score)
						{
							score = newScore;
							pos = $"{col}{row}";
						}
					}

			return pos;
		}

		public int Minimax(char[,] boardState, int depth, bool IsMaximizing)
		{
			int score = Int32.MinValue;
			string player = "Bot";
			char state = CheckGameState();
			if (state != 'C')
				return GetPoints(state, depth);

			if (!IsMaximizing)
			{
				score = Int32.MaxValue;
				player = "Human";
			}

			for (int col = 0; col < 3; col++)
				for (int row = 0; row < 3; row++)
					if (boardState[col, row] == ' ')
					{
						boardState[col, row] = PlayerSymbols[player];
						int newScore = Minimax(boardState, depth + 1, !IsMaximizing);
						if (IsMaximizing)
							score = Math.Max(score, newScore);
						else
							score = Math.Min(score, newScore);
						boardState[col, row] = ' ';
					}

			return score;
		}

		public void UpdateBoard(Button button, string pos)
		{
			Board[pos[0] - 48, pos[1] - 48] = CurrPlayer;
			button.Content = CurrPlayer;
			button.IsEnabled = false;
		}

		public void BotTurn()
		{
			string pos;
			var buttonsDic = board.Children.OfType<Button>().Cast<Button>().ToDictionary<Button, string, Button>(v => v.Name.ToString().Substring(1, 2), v => v);
			if (BotIsEasy)
				pos = GetEasyBotPlay();
			else
				pos = GetHardBotPlay();
			if (CheckGameState() == 'C')
			{
				UpdateBoard(buttonsDic[pos], pos);
				UpdatePlayer();
			}
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			string pos = button.Name.ToString().Substring(1, 2);

			if (CheckGameState() == 'C')
			{
				UpdateBoard(button, pos);
				UpdatePlayer();
				if (BotIsPlaying)
				{
					BotTurn();
				}

			}
		}

		private void RestartClick(object sender, RoutedEventArgs e)
		{
			board.Children.OfType<Button>().Cast<Button>().ToList().ForEach(Button => 
			{ 
				Button.IsEnabled = true;
				Button.Content = " ";
			});

			for (int col = 0; col < 3; col++)
			{
				for (int row = 0; row < 3; row++)
				{
					Board[col, row] = ' ';
				}
			}
		}
	}
}
