using System;

namespace Sce.Pss.HighLevel.UI
{
	public abstract class GestureEventArgs : EventArgs
	{
		public Widget Source
		{
			get;
			private set;
		}

		public GestureEventArgs(Widget source)
		{
			this.Source = source;
		}
	}
}
