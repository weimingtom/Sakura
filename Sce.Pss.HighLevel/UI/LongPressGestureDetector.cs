using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class LongPressGestureDetector : GestureDetector
	{
		private const float defaultMaxDistanceInch = 0.169f;

		private int downID;

		private Vector2 downPos;

		private TimeSpan downTime;

		public event EventHandler<LongPressEventArgs> LongPressDetected;

		public float MaxDistance
		{
			get;
			set;
		}

		public float MinPressDuration
		{
			get;
			set;
		}

		public LongPressGestureDetector()
		{
			this.MaxDistance = 0.169f * UISystem.Dpi;
			this.MinPressDuration = 1000f;
			this.LongPressDetected = null;
			this.downID = 0;
			this.downPos = Vector2.Zero;
			this.downTime = TimeSpan.Zero;
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
				result = GestureDetectorResponse.FailedAndStop;
				break;
			case TouchEventType.Down:
				this.downID = primaryTouchEvent.FingerID;
				this.downPos = primaryTouchEvent.WorldPosition;
				this.downTime = primaryTouchEvent.Time;
				result = GestureDetectorResponse.UndetectedAndContinue;
				break;
			case TouchEventType.Move:
			{
				float num = primaryTouchEvent.WorldPosition.Distance(this.downPos);
				float num2 = (float)(primaryTouchEvent.Time - this.downTime).TotalMilliseconds;
				if (num <= this.MaxDistance)
				{
					if (num2 >= this.MinPressDuration)
					{
						if (this.LongPressDetected != null)
						{
							LongPressEventArgs longPressEventArgs = new LongPressEventArgs(base.TargetWidget, primaryTouchEvent.WorldPosition, primaryTouchEvent.LocalPosition, num2);
							this.LongPressDetected.Invoke(this, longPressEventArgs);
						}
						result = GestureDetectorResponse.DetectedAndContinue;
					}
				}
				else
				{
					result = GestureDetectorResponse.FailedAndStop;
				}
				break;
			}
			}
			return result;
		}

		protected internal override void OnResetState()
		{
			this.downID = 0;
			this.downPos = Vector2.Zero;
			this.downTime = TimeSpan.Zero;
		}
	}
}
