using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class DoubleTapGestureDetector : GestureDetector
	{
		private const float defaultMaxDistanceInch = 0.157f;

		private const int requiredDownCount = 2;

		private int downCount;

		private int firstDownID;

		private Vector2 firstDownPos;

		private TimeSpan previousTime;

		public event EventHandler<DoubleTapEventArgs> DoubleTapDetected;

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

		public float MaxNextPressDuration
		{
			get;
			set;
		}

		public DoubleTapGestureDetector()
		{
			this.MaxDistance = 0.157f * UISystem.Dpi;
			this.MaxPressDuration = 300f;
			this.MaxNextPressDuration = 300f;
			this.DoubleTapDetected = null;
			this.downCount = 0;
			this.firstDownID = 0;
			this.firstDownPos = Vector2.Zero;
			this.previousTime = TimeSpan.Zero;
		}

		protected internal override GestureDetectorResponse OnTouchEvent(TouchEventCollection touchEvents)
		{
			GestureDetectorResponse result = base.State;
			TouchEvent primaryTouchEvent = touchEvents.PrimaryTouchEvent;
			if (base.State != GestureDetectorResponse.None && this.firstDownID != primaryTouchEvent.FingerID)
			{
				return result;
			}
			Vector2 worldPosition = primaryTouchEvent.WorldPosition;
			float num = this.firstDownPos.Distance(worldPosition);
			float num2 = (float)(primaryTouchEvent.Time - this.previousTime).TotalMilliseconds;
			switch (primaryTouchEvent.Type)
			{
			case TouchEventType.Up:
				if (this.downCount == 2)
				{
					if (num <= this.MaxDistance && num2 <= this.MaxPressDuration)
					{
						if (this.DoubleTapDetected != null)
						{
							DoubleTapEventArgs doubleTapEventArgs = new DoubleTapEventArgs(base.TargetWidget, primaryTouchEvent.WorldPosition, primaryTouchEvent.LocalPosition);
							this.DoubleTapDetected.Invoke(this, doubleTapEventArgs);
						}
						result = GestureDetectorResponse.DetectedAndStop;
					}
					else
					{
						result = GestureDetectorResponse.FailedAndStop;
					}
				}
				else if (num <= this.MaxDistance && num2 <= this.MaxPressDuration)
				{
					this.previousTime = primaryTouchEvent.Time;
					result = GestureDetectorResponse.UndetectedAndContinue;
				}
				else
				{
					result = GestureDetectorResponse.FailedAndStop;
				}
				break;
			case TouchEventType.Down:
				if (this.downCount > 0)
				{
					if (num <= this.MaxDistance && num2 <= this.MaxNextPressDuration)
					{
						this.downCount++;
						this.previousTime = primaryTouchEvent.Time;
					}
					else
					{
						this.OnResetState();
					}
				}
				if (this.downCount == 0)
				{
					this.downCount++;
					this.firstDownID = primaryTouchEvent.FingerID;
					this.firstDownPos = primaryTouchEvent.WorldPosition;
					this.previousTime = primaryTouchEvent.Time;
				}
				result = GestureDetectorResponse.UndetectedAndContinue;
				break;
			}
			return result;
		}

		protected internal override void OnResetState()
		{
			this.downCount = 0;
			this.firstDownID = 0;
			this.firstDownPos = Vector2.Zero;
			this.previousTime = TimeSpan.Zero;
		}
	}
}
