using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class MotionEvent
	{
		public Vector3 Acceleration
		{
			get;
			set;
		}

		public Vector3 AngularVelocity
		{
			get;
			set;
		}

		public TimeSpan Time
		{
			get;
			set;
		}

		public MotionEvent()
		{
			this.Acceleration = Vector3.Zero;
			this.AngularVelocity = Vector3.Zero;
			this.Time = TimeSpan.Zero;
		}
	}
}
