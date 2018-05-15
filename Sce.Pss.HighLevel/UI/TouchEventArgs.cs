using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TouchEventArgs : EventArgs
	{
		public TouchEventCollection TouchEvents
		{
			get;
			private set;
		}

		public bool Forward
		{
			get
			{
				return this.TouchEvents.Forward;
			}
			set
			{
				this.TouchEvents.Forward = value;
			}
		}

		public TouchEventArgs(TouchEventCollection touchEvents)
		{
			this.TouchEvents = touchEvents;
		}
	}
}
