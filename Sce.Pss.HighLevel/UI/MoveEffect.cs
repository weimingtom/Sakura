using System;

namespace Sce.Pss.HighLevel.UI
{
	public class MoveEffect : Effect
	{
		private float fromX;

		private float fromY;

		private float toX;

		private float toY;

		private AnimationInterpolator interpolatorCallback;

		public float Time
		{
			get;
			set;
		}

		public float X
		{
			get;
			set;
		}

		public float Y
		{
			get;
			set;
		}

		public MoveEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public MoveEffect()
		{
			base.Widget = null;
			this.Time = 1000f;
			this.X = 0f;
			this.Y = 0f;
			this.Interpolator = MoveEffectInterpolator.Linear;
			this.CustomInterpolator = null;
		}

		public MoveEffect(Widget widget, float time, float x, float y, MoveEffectInterpolator interpolator)
		{
			base.Widget = widget;
			this.Time = time;
			this.X = x;
			this.Y = y;
			this.Interpolator = interpolator;
			this.CustomInterpolator = null;
		}

		public static MoveEffect CreateAndStart(Widget widget, float time, float x, float y, MoveEffectInterpolator interpolator)
		{
			MoveEffect moveEffect = new MoveEffect(widget, time, x, y, interpolator);
			moveEffect.Start();
			return moveEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget != null)
			{
				switch (this.Interpolator)
				{
				case MoveEffectInterpolator.Linear:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					break;
				case MoveEffectInterpolator.EaseOutQuad:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
					break;
				case MoveEffectInterpolator.Overshoot:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.OvershootInterpolator);
					break;
				case MoveEffectInterpolator.Elastic:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.ElasticInterpolator);
					break;
				case MoveEffectInterpolator.Custom:
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
				this.fromX = base.Widget.X;
				this.fromY = base.Widget.Y;
				this.toX = this.X;
				this.toY = this.Y;
				base.Widget.Visible = true;
			}
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.TotalElapsedTime < this.Time)
			{
				base.Widget.X = this.interpolatorCallback(this.fromX, this.toX, base.TotalElapsedTime / this.Time);
				base.Widget.Y = this.interpolatorCallback(this.fromY, this.toY, base.TotalElapsedTime / this.Time);
				return EffectUpdateResponse.Continue;
			}
			base.Widget.X = this.toX;
			base.Widget.Y = this.toY;
			return EffectUpdateResponse.Finish;
		}

		protected override void OnStop()
		{
		}
	}
}
