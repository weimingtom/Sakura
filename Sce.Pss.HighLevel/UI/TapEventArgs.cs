using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TapEventArgs : GestureEventArgs
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

		public TapEventArgs(Widget source, Vector2 worldPosition, Vector2 localPosition) : base(source)
		{
			this.WorldPosition = worldPosition;
			this.LocalPosition = localPosition;
		}
	}
}
