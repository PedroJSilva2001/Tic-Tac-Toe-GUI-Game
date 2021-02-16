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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Game _game;

		private Dictionary<string, Button> _buttons;

		private bool _cellPressOnQueue;

		private bool _restartPressOnQueue;

		private Game Game {
			get { return _game; }

			set { _game = value; }
		}

		public MainWindow()
		{
			InitializeComponent();
			_game = new Game(false, 1);
			_buttons = board.Children.OfType<Button>().ToDictionary<Button, string, Button>(
				v => v.Name.ToString().Substring(1, 2), v => v);
			_cellPressOnQueue = false;
			_restartPressOnQueue = false;
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			if (_cellPressOnQueue || _restartPressOnQueue ) return;

			_cellPressOnQueue = true;

			Button button = (Button)sender;
			string pos = button.Name.ToString().Substring(1, 2);

			if (_game.CheckGameState() == 'C')
			{
				_game.UpdateBoard(pos);
				_game.UpdatePlayer();
				PrintSymbol(pos);

				if (_game.BotIsPlaying)
				{
					//await Task.Delay(1500);

					string posBot = _game.BotTurn();
					if (posBot != null) PrintSymbol(posBot);
				}
			}
			_cellPressOnQueue = false;
		}

		private async void RestartClick(object sender, RoutedEventArgs e)
		{
			//UnsubscribingButtons();
			_restartPressOnQueue = true;
			ReenableButtons();
			_game.SetupBoard();
			_game.Current = 0;
			//SubscribingButtons();
			_restartPressOnQueue = false;
		}

		private void ReenableButtons()
		{
			board.Children.OfType<Button>().ToList().ForEach(Button =>
			{
				Button.IsEnabled = true;
				Button.Content = " ";
			});
		}

		private void PrintSymbol(string pos)
		{
			Button button = _buttons[pos];
			button.IsEnabled = false;
			button.Content = _game.Players[_game.Current].Symbol;
		}


		private void UnsubscribingButtons()
		{
			board.Children.OfType<Button>().ToList().ForEach(Button =>
			{
				Button.Click -= ButtonClick;
				
			});
		}

		private void SubscribingButtons()
		{
			board.Children.OfType<Button>().ToList().ForEach(Button =>
			{
				Button.Click += ButtonClick;

			});
		}

	}
}
