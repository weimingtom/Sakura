using System;

namespace Sce.Pss.HighLevel.UI
{
	public class KeyEventArgs : EventArgs
	{
		internal TimeSpan Time
		{
			get;
			set;
		}

		public KeyType KeyType
		{
			get;
			internal set;
		}

		public KeyEventType KeyEventType
		{
			get;
			internal set;
		}

		public bool Forward
		{
			get;
			set;
		}

		public KeyEventArgs(KeyEvent keyEvent)
		{
			this.Time = keyEvent.Time;
			this.KeyEventType = KeyEventType.Up;
			this.KeyType = KeyType.Left;
			this.Forward = false;
		}
	}
}
