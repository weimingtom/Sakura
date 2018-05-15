using Sce.Pss.Core;
using System;
using System.Collections;

namespace Sce.Pss.HighLevel.UI
{
	public class PinchGestureDetector : GestureDetector
	{
		private const float defaultMinPinchDistanceInch = 0.169f;

		private const int REQUIRED_TOUCH_COUNT = 2;

		private float firstDistance;

		private Vector2 firstTouchVector;

		public event EventHandler<PinchEventArgs> PinchDetected;

		public event EventHandler<PinchEventArgs> PinchStartDetected;

		public event EventHandler<PinchEventArgs> PinchEndDetected;

		public float MinPinchDistance
		{
			get;
			set;
		}

		public PinchGestureDetector()
		{
			this.MinPinchDistance = 0.169f * UISystem.Dpi;
			this.PinchDetected = null;
			this.PinchStartDetected = null;
			this.PinchEndDetected = null;
			this.firstDistance = -1f;
			this.firstTouchVector = Vector2.Zero;
		}

		protected internal override GestureDetectorResponse OnTouchEvent(TouchEventCollection touchEvents)
		{
			GestureDetectorResponse result = base.State;
			TouchEventCollection touchEventCollection = new TouchEventCollection();
			foreach (TouchEvent touchEvent in touchEvents)
			{
				if (touchEvent.Type != TouchEventType.Enter && touchEvent.Type != TouchEventType.Leave && touchEvent.Type != TouchEventType.None)
				{
					touchEventCollection.Add(touchEvent);
				}
			}
			if (touchEventCollection.Count >= 2)
			{
				bool flag = false;
				Vector2[] array = new Vector2[2];
				Vector2[] array2 = new Vector2[2];
				for (int i = 0; i < 2; i++)
				{
					TouchEvent touchEvent2 = touchEventCollection[i];
					array[i] = touchEvent2.LocalPosition;
					array2[i] = touchEvent2.WorldPosition;
					if (touchEvent2.Type == TouchEventType.Up)
					{
						flag = true;
					}
				}
				float num = array[0].Distance(array[1]);
				if (this.firstDistance < 0f)
				{
					this.firstDistance = num;
					this.firstTouchVector = array[1] - array[0];
				}
				if (base.State == GestureDetectorResponse.UndetectedAndContinue || base.State == GestureDetectorResponse.None)
				{
					if (FMath.Abs(num - this.firstDistance) > this.MinPinchDistance)
					{
						Vector2 localCenter = (array[0] + array[1]) / 2f;
						Vector2 worldCenter = (array2[0] + array2[1]) / 2f;
						if (this.PinchStartDetected != null)
						{
							this.PinchStartDetected.Invoke(this, new PinchEventArgs(base.TargetWidget, num, 1f, 0f, worldCenter, localCenter));
						}
						result = GestureDetectorResponse.DetectedAndContinue;
					}
					else
					{
						result = GestureDetectorResponse.UndetectedAndContinue;
					}
				}
				else if (base.State == GestureDetectorResponse.DetectedAndContinue)
				{
					Vector2 vector = array[1] - array[0];
					float scale = num / this.firstDistance;
					float angle = this.firstTouchVector.Angle(vector);
					Vector2 localCenter2 = (array[0] + array[1]) / 2f;
					Vector2 worldCenter2 = (array2[0] + array2[1]) / 2f;
					if (flag)
					{
						if (this.PinchEndDetected != null)
						{
							this.PinchEndDetected.Invoke(this, new PinchEventArgs(base.TargetWidget, num, scale, angle, worldCenter2, localCenter2));
						}
						result = GestureDetectorResponse.DetectedAndStop;
						this.firstDistance = -1f;
						this.firstTouchVector = Vector2.Zero;
					}
					else
					{
						if (this.PinchDetected != null)
						{
							this.PinchDetected.Invoke(this, new PinchEventArgs(base.TargetWidget, num, scale, angle, worldCenter2, localCenter2));
						}
						result = GestureDetectorResponse.DetectedAndContinue;
					}
				}
			}
			else if (base.State == GestureDetectorResponse.DetectedAndContinue)
			{
				result = GestureDetectorResponse.FailedAndStop;
			}
			else
			{
				result = GestureDetectorResponse.UndetectedAndContinue;
			}
			return result;
		}

		protected internal override void OnResetState()
		{
			this.firstDistance = -1f;
			this.firstTouchVector = Vector2.Zero;
		}
	}
}
