using System;

namespace Sce.Pss.HighLevel.UI
{
	public class SlideInEffect : Effect
	{
		private float from;

		private float to;

		private AnimationInterpolator interpolatorCallback;

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

		public SlideInEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public SlideInEffect()
		{
			base.Widget = null;
			this.Time = 1000f;
			this.Interpolator = SlideInEffectInterpolator.EaseOutQuad;
			this.CustomInterpolator = null;
			this.MoveDirection = FourWayDirection.Left;
		}

		public SlideInEffect(Widget widget, float time, FourWayDirection direction, SlideInEffectInterpolator interpolator)
		{
			base.Widget = widget;
			this.Time = time;
			this.Interpolator = interpolator;
			this.CustomInterpolator = null;
			this.MoveDirection = direction;
		}

		public static SlideInEffect CreateAndStart(Widget widget, float time, FourWayDirection direction, SlideInEffectInterpolator interpolator)
		{
			SlideInEffect slideInEffect = new SlideInEffect(widget, time, direction, interpolator);
			slideInEffect.Start();
			return slideInEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget != null)
			{
				switch (this.Interpolator)
				{
				case SlideInEffectInterpolator.Linear:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					break;
				case SlideInEffectInterpolator.EaseOutQuad:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
					break;
				case SlideInEffectInterpolator.Overshoot:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.OvershootInterpolator);
					break;
				case SlideInEffectInterpolator.Elastic:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.ElasticInterpolator);
					break;
				case SlideInEffectInterpolator.Custom:
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
					this.from = base.Widget.Y + (float)UISystem.FramebufferHeight;
					this.to = base.Widget.Y;
					base.Widget.Y = base.Widget.Y + (float)UISystem.FramebufferHeight;
					base.Widget.Visible = true;
					break;
				case FourWayDirection.Down:
					this.from = base.Widget.Y - (float)UISystem.FramebufferHeight;
					this.to = base.Widget.Y;
					base.Widget.Y = base.Widget.Y - (float)UISystem.FramebufferHeight;
					base.Widget.Visible = true;
					return;
				case FourWayDirection.Left:
					this.from = base.Widget.X + (float)UISystem.FramebufferWidth;
					this.to = base.Widget.X;
					base.Widget.X = base.Widget.X + (float)UISystem.FramebufferWidth;
					base.Widget.Visible = true;
					return;
				case FourWayDirection.Right:
					this.from = base.Widget.X - (float)UISystem.FramebufferWidth;
					this.to = base.Widget.X;
					base.Widget.X = base.Widget.X - (float)UISystem.FramebufferWidth;
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
		}
	}
}
