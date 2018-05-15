using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class FlickGestureDetector : GestureDetector
	{
		private const float defaultMinSpeedInch = 0.4f;

		private const float defaultMaxSpeedInch = 8f;

		private int downID;

		private Vector2[] previousWorldPosition = new Vector2[2];

		private TimeSpan[] previousTime = new TimeSpan[2];

		private readonly long pollingMilliseconds = 60L;

		public event EventHandler<FlickEventArgs> FlickDetected;

		public float MinSpeed
		{
			get;
			set;
		}

		public float MaxSpeed
		{
			get;
			set;
		}

		public FlickDirection Direction
		{
			get;
			set;
		}

		public FlickGestureDetector()
		{
			this.MinSpeed = 0.4f * UISystem.Dpi;
			this.MaxSpeed = 8f * UISystem.Dpi;
			this.Direction = FlickDirection.All;
			this.Initialize();
		}

		protected internal override GestureDetectorResponse OnTouchEvent(TouchEventCollection touchEvents)
		{
			GestureDetectorResponse result = base.State;
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			if (base.State != GestureDetectorResponse.None && this.downID != primaryTouchEvent.FingerID)
			{
				return result;
			}
			switch (primaryTouchEvent.Type)
			{
			case TouchEventType.Up:
			{
				Vector2 vector = primaryTouchEvent.WorldPosition - this.previousWorldPosition[1];
				float num = (float)(primaryTouchEvent.Time - this.previousTime[1]).TotalMilliseconds;
				Vector2 zero = Vector2.Zero;
				if (this.Direction != FlickDirection.Virtical)
				{
					zero.X = vector.X / num * 1000f;
					if (zero.X < -this.MaxSpeed)
					{
						zero.X = -this.MaxSpeed;
					}
					else if (zero.X > this.MaxSpeed)
					{
						zero.X = this.MaxSpeed;
					}
					else if (Math.Abs(zero.X) < this.MinSpeed)
					{
						zero.X = 0f;
					}
				}
				if (this.Direction != FlickDirection.Horizontal)
				{
					zero.Y = vector.Y / num * 1000f;
					if (zero.Y < -this.MaxSpeed)
					{
						zero.Y = -this.MaxSpeed;
					}
					else if (this.MaxSpeed < zero.Y)
					{
						zero.Y = this.MaxSpeed;
					}
					else if (Math.Abs(zero.Y) < this.MinSpeed)
					{
						zero.Y = 0f;
					}
				}
				if (zero.X != 0f || zero.Y != 0f)
				{
					if (this.FlickDetected != null)
					{
						FlickEventArgs flickEventArgs = new FlickEventArgs(base.TargetWidget, primaryTouchEvent.WorldPosition, primaryTouchEvent.LocalPosition, zero);
						this.FlickDetected.Invoke(this, flickEventArgs);
					}
					result = GestureDetectorResponse.DetectedAndStop;
				}
				else
				{
					result = GestureDetectorResponse.FailedAndStop;
				}
				break;
			}
			case TouchEventType.Down:
				this.downID = primaryTouchEvent.FingerID;
				for (int i = 0; i < 2; i++)
				{
					this.previousWorldPosition[i] = primaryTouchEvent.WorldPosition;
					this.previousTime[i] = primaryTouchEvent.Time;
				}
				result = GestureDetectorResponse.UndetectedAndContinue;
				break;
			case TouchEventType.Move:
				if ((primaryTouchEvent.Time - this.previousTime[0]).TotalMilliseconds > (double)this.pollingMilliseconds)
				{
					this.previousWorldPosition[1] = this.previousWorldPosition[0];
					this.previousTime[1] = this.previousTime[0];
					this.previousWorldPosition[0] = primaryTouchEvent.WorldPosition;
					this.previousTime[0] = primaryTouchEvent.Time;
					result = GestureDetectorResponse.UndetectedAndContinue;
				}
				break;
			}
			return result;
		}

		protected internal override void OnResetState()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			this.downID = 0;
			for (int i = 0; i < 2; i++)
			{
				this.previousWorldPosition[i] = Vector2.Zero;
				this.previousTime[i] = TimeSpan.Zero;
			}
		}
	}
}
