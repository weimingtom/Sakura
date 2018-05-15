using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class TapGestureDetector : GestureDetector
	{
		private const float defaultMaxDistanceInch = 0.169f;

		private int downID;

		private Vector2 downPos;

		private TimeSpan downTime;

		public event EventHandler<TapEventArgs> TapDetected;

		public float MaxDistance
		{
			get;
			set;
		}

		public float MaxPressDuration
		{
			get;
			set;
		}

		public TapGestureDetector()
		{
			this.MaxDistance = 0.169f * UISystem.Dpi;
			this.MaxPressDuration = 300f;
			this.TapDetected = null;
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
			{
				Vector2 worldPosition = primaryTouchEvent.WorldPosition;
				float num = this.downPos.Distance(worldPosition);
				float num2 = (float)(primaryTouchEvent.Time - this.downTime).TotalMilliseconds;
				if (num <= this.MaxDistance && num2 <= this.MaxPressDuration)
				{
					if (this.TapDetected != null)
					{
						TapEventArgs tapEventArgs = new TapEventArgs(base.TargetWidget, primaryTouchEvent.WorldPosition, primaryTouchEvent.LocalPosition);
						this.TapDetected.Invoke(this, tapEventArgs);
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
				this.downPos = primaryTouchEvent.WorldPosition;
				this.downTime = primaryTouchEvent.Time;
				result = GestureDetectorResponse.UndetectedAndContinue;
				break;
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
