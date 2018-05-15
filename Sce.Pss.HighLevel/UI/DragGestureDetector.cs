using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class DragGestureDetector : GestureDetector
	{
		private const float defaultMaxDistanceInch = 0.169f;

		private int downID;

		private Vector2 previousWorldPosition;

		public event EventHandler<DragEventArgs> DragDetected;

		public event EventHandler<DragEventArgs> DragStartDetected;

		public event EventHandler<DragEventArgs> DragEndDetected;

		public float MaxDistance
		{
			get;
			set;
		}

		public DragGestureDetector()
		{
			this.MaxDistance = 0.169f * UISystem.Dpi;
			this.DragDetected = null;
			this.downID = 0;
			this.previousWorldPosition = Vector2.Zero;
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
				if (this.DragEndDetected != null)
				{
					DragEventArgs dragEventArgs = new DragEventArgs(base.TargetWidget, primaryTouchEvent.WorldPosition, primaryTouchEvent.LocalPosition, primaryTouchEvent.WorldPosition - this.previousWorldPosition);
					this.DragEndDetected.Invoke(this, dragEventArgs);
				}
				this.previousWorldPosition = primaryTouchEvent.WorldPosition;
				result = GestureDetectorResponse.FailedAndStop;
				break;
			case TouchEventType.Down:
				this.downID = primaryTouchEvent.FingerID;
				this.previousWorldPosition = primaryTouchEvent.WorldPosition;
				result = GestureDetectorResponse.UndetectedAndContinue;
				break;
			case TouchEventType.Move:
				if (base.State == GestureDetectorResponse.UndetectedAndContinue)
				{
					Vector2 worldPosition = primaryTouchEvent.WorldPosition;
					float num = this.previousWorldPosition.Distance(worldPosition);
					if (num >= this.MaxDistance)
					{
						DragEventArgs dragEventArgs2 = new DragEventArgs(base.TargetWidget, primaryTouchEvent.WorldPosition, primaryTouchEvent.LocalPosition, primaryTouchEvent.WorldPosition - this.previousWorldPosition);
						this.previousWorldPosition = primaryTouchEvent.WorldPosition;
						if (this.DragStartDetected != null)
						{
							this.DragStartDetected.Invoke(this, dragEventArgs2);
						}
						if (this.DragDetected != null)
						{
							this.DragDetected.Invoke(this, dragEventArgs2);
						}
						result = GestureDetectorResponse.DetectedAndContinue;
					}
				}
				else
				{
					if (this.DragDetected != null)
					{
						DragEventArgs dragEventArgs3 = new DragEventArgs(base.TargetWidget, primaryTouchEvent.WorldPosition, primaryTouchEvent.LocalPosition, primaryTouchEvent.WorldPosition - this.previousWorldPosition);
						this.DragDetected.Invoke(this, dragEventArgs3);
					}
					this.previousWorldPosition = primaryTouchEvent.WorldPosition;
					result = GestureDetectorResponse.DetectedAndContinue;
				}
				break;
			}
			return result;
		}

		protected internal override void OnResetState()
		{
			this.downID = 0;
			this.previousWorldPosition = Vector2.Zero;
		}
	}
}
