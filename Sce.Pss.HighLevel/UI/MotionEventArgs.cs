using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class MotionEventArgs : EventArgs
	{
		internal TimeSpan Time
		{
			get;
			private set;
		}

		public Vector3 Acceleration
		{
			get;
			private set;
		}

		public Vector3 AngularVelocity
		{
			get;
			private set;
		}

		public MotionEventArgs()
		{
			this.Acceleration = Vector3.Zero;
			this.AngularVelocity = Vector3.Zero;
		}

		public MotionEventArgs(MotionEvent motionEvent)
		{
			this.Time = motionEvent.Time;
			this.Acceleration = motionEvent.Acceleration;
			this.AngularVelocity = motionEvent.AngularVelocity;
		}
	}
}
