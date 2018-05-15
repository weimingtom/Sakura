using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class PinchEventArgs : GestureEventArgs
	{
		public float Distance
		{
			get;
			private set;
		}

		public float Scale
		{
			get;
			private set;
		}

		public float Angle
		{
			get;
			private set;
		}

		public Vector2 WorldCenterPosition
		{
			get;
			private set;
		}

		public Vector2 LocalCenterPosition
		{
			get;
			private set;
		}

		public PinchEventArgs(Widget source, float distance, float scale, float angle, Vector2 worldCenter, Vector2 localCenter) : base(source)
		{
			this.Distance = distance;
			this.Scale = scale;
			this.Angle = angle;
			this.WorldCenterPosition = worldCenter;
			this.LocalCenterPosition = localCenter;
		}
	}
}
