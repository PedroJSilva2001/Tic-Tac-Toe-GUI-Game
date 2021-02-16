using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
namespace TicTacToe
{
	public static class Utilities
	{
		public static void awaitTimer(uint delay)
		{
			Timer t = new Timer(delay);
			t.AutoReset = false;
			t.Stop();
			t.Enabled = true;
			t.Start();
			//t.Elapsed += return ;

		}
	}
}
