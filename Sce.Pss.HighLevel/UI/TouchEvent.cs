using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TouchEvent
	{
		public Widget Source
		{
			get;
			set;
		}

		public Vector2 WorldPosition
		{
			get;
			set;
		}

		public Vector2 LocalPosition
		{
			get;
			set;
		}

		public TimeSpan Time
		{
			get;
			set;
		}

		public TouchEventType Type
		{
			get;
			set;
		}

		public int FingerID
		{
			get;
			set;
		}

		public TouchEvent()
		{
			this.FingerID = 0;
			this.Time = TimeSpan.Zero;
			this.Type = TouchEventType.None;
			this.WorldPosition = Vector2.Zero;
			this.LocalPosition = Vector2.Zero;
			this.Source = null;
		}
	}
}
