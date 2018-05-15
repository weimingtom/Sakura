using System;

namespace Sce.Pss.HighLevel.UI
{
	public class SlideOutEffect : Effect
	{
		private float from;

		private float to;

		private AnimationInterpolator interpolatorCallback;

		private float orgWidgetX;

		private float orgWidgetY;

		public float Time
		{
			get;
			set;
		}

		public FourWayDirection MoveDirection
		{
			get;
			set;
		}

		public SlideOutEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public SlideOutEffect()
		{
			base.Widget = null;
			this.Time = 1000f;
			this.Interpolator = SlideOutEffectInterpolator.EaseOutQuad;
			this.CustomInterpolator = null;
			this.MoveDirection = FourWayDirection.Left;
		}

		public SlideOutEffect(Widget widget, float time, FourWayDirection direction, SlideOutEffectInterpolator interpolator)
		{
			base.Widget = widget;
			this.Time = time;
			this.Interpolator = interpolator;
			this.CustomInterpolator = null;
			this.MoveDirection = direction;
		}

		public static SlideOutEffect CreateAndStart(Widget widget, float time, FourWayDirection direction, SlideOutEffectInterpolator interpolator)
		{
			SlideOutEffect slideOutEffect = new SlideOutEffect(widget, time, direction, interpolator);
			slideOutEffect.Start();
			return slideOutEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget != null)
			{
				switch (this.Interpolator)
				{
				case SlideOutEffectInterpolator.Linear:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					break;
				case SlideOutEffectInterpolator.EaseOutQuad:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
					break;
				case SlideOutEffectInterpolator.Overshoot:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.OvershootInterpolator);
					break;
				case SlideOutEffectInterpolator.Elastic:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.ElasticInterpolator);
					break;
				case SlideOutEffectInterpolator.Custom:
					if (this.CustomInterpolator != null)
					{
						this.interpolatorCallback = this.CustomInterpolator;
					}
					else
					{
						this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					}
					break;
				default:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					break;
				}
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
					this.from = base.Widget.Y;
					this.to = base.Widget.Y - (float)UISystem.FramebufferHeight;
					this.orgWidgetY = base.Widget.Y;
					base.Widget.Visible = true;
					return;
				case FourWayDirection.Down:
					this.from = base.Widget.Y;
					this.to = base.Widget.Y + (float)UISystem.FramebufferHeight;
					this.orgWidgetY = base.Widget.Y;
					base.Widget.Visible = true;
					break;
				case FourWayDirection.Left:
					this.from = base.Widget.X;
					this.to = base.Widget.X - (float)UISystem.FramebufferWidth;
					this.orgWidgetX = base.Widget.X;
					base.Widget.Visible = true;
					return;
				case FourWayDirection.Right:
					this.from = base.Widget.X;
					this.to = base.Widget.X + (float)UISystem.FramebufferWidth;
					this.orgWidgetX = base.Widget.X;
					base.Widget.Visible = true;
					return;
				default:
					return;
				}
			}
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			float num = this.interpolatorCallback(this.from, this.to, base.TotalElapsedTime / this.Time);
			if (base.TotalElapsedTime < this.Time)
			{
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
				case FourWayDirection.Down:
					base.Widget.Y = num;
					break;
				case FourWayDirection.Left:
				case FourWayDirection.Right:
					base.Widget.X = num;
					break;
				}
				return EffectUpdateResponse.Continue;
			}
			switch (this.MoveDirection)
			{
			case FourWayDirection.Up:
			case FourWayDirection.Down:
				base.Widget.Y = this.to;
				break;
			case FourWayDirection.Left:
			case FourWayDirection.Right:
				base.Widget.X = this.to;
				break;
			}
			return EffectUpdateResponse.Finish;
		}

		protected override void OnStop()
		{
			if (base.Widget != null)
			{
				switch (this.MoveDirection)
				{
				case FourWayDirection.Up:
				case FourWayDirection.Down:
					base.Widget.Y = this.orgWidgetY;
					base.Widget.Visible = false;
					break;
				case FourWayDirection.Left:
				case FourWayDirection.Right:
					base.Widget.X = this.orgWidgetX;
					base.Widget.Visible = false;
					return;
				default:
					return;
				}
			}
		}
	}
}
