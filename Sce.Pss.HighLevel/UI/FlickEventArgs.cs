using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class FlickEventArgs : GestureEventArgs
	{
		public Vector2 WorldPosition
		{
			get;
			private set;
		}

		public Vector2 LocalPosition
		{
			get;
			private set;
		}

		public Vector2 Speed
		{
			get;
			private set;
		}

		public FlickEventArgs(Widget source, Vector2 worldPosition, Vector2 localPosition, Vector2 flickSpeed) : base(source)
		{
			this.WorldPosition = worldPosition;
			this.LocalPosition = localPosition;
			this.Speed = flickSpeed;
		}
	}
}
