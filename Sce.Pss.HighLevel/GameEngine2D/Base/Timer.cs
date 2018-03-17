using System;
using System.Diagnostics;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class Timer
	{
		private Stopwatch m_stop_watch = new Stopwatch();

		public Timer()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.m_stop_watch.Reset();
			this.m_stop_watch.Start();
		}

		public double Milliseconds()
		{
			return this.m_stop_watch.Elapsed.TotalMilliseconds;
		}

		public double Seconds()
		{
			return this.m_stop_watch.Elapsed.TotalSeconds;
		}
	}
}
