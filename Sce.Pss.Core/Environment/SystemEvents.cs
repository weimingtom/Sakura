using System;
using System.Diagnostics;
using System.Threading;
using Sakura.OpenTK;
using Sce.Pss.Core.Graphics;

namespace Sce.Pss.Core.Environment
{
	public static class SystemEvents
	{
		private static Stopwatch timer = new Stopwatch();
		public static void CheckEvents ()
		{
			MyGameWindow.ProcessEvents();
			double delta = timer.Elapsed.TotalMilliseconds;
			double frame = 1000.0 / 24.0;
			if (delta < frame)
			{
				int free = (int)(frame - delta);
				Thread.Sleep(free);
				//Debug.WriteLine("Sleep: " + free);
			}
			timer.Restart();
		}
	}
}
