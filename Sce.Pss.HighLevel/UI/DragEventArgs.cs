using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class DragEventArgs : GestureEventArgs
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

		public Vector2 Distance
		{
			get;
			private set;
		}

		public DragEventArgs(Widget source, Vector2 worldPosition, Vector2 localPosition, Vector2 distance) : base(source)
		{
			this.WorldPosition = worldPosition;
			this.LocalPosition = localPosition;
			this.Distance = distance;
		}
	}
}
