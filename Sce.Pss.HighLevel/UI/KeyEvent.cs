using System;

namespace Sce.Pss.HighLevel.UI
{
	public class KeyEvent
	{
		public KeyEventType KeyEventType
		{
			get;
			set;
		}

		public KeyType KeyType
		{
			get;
			set;
		}

		public TimeSpan Time
		{
			get;
			set;
		}

		public bool Forward
		{
			get;
			set;
		}

		public KeyEvent()
		{
			this.KeyEventType = KeyEventType.Up;
			this.KeyType = KeyType.Left;
			this.Time = TimeSpan.Zero;
			this.Forward = false;
		}
	}
}
