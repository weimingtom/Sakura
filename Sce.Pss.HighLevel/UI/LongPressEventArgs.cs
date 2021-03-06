using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class LongPressEventArgs : GestureEventArgs
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

		public float ElapsedTime
		{
			get;
			private set;
		}

		public LongPressEventArgs(Widget source, Vector2 worldPosition, Vector2 localPosition, float elapsedTime) : base(source)
		{
			this.WorldPosition = worldPosition;
			this.LocalPosition = localPosition;
			this.ElapsedTime = elapsedTime;
		}
	}
}
