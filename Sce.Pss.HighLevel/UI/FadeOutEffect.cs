using System;

namespace Sce.Pss.HighLevel.UI
{
	public class FadeOutEffect : Effect
	{
		private AnimationInterpolator interpolatorCallback;

		public float Time
		{
			get;
			set;
		}

		public FadeOutEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public FadeOutEffect()
		{
			base.Widget = null;
			this.Time = 1000f;
			this.Interpolator = FadeOutEffectInterpolator.EaseOutQuad;
			this.CustomInterpolator = null;
		}

		public FadeOutEffect(Widget widget, float time, FadeOutEffectInterpolator interpolator)
		{
			base.Widget = widget;
			this.Time = time;
			this.Interpolator = FadeOutEffectInterpolator.EaseOutQuad;
			this.CustomInterpolator = null;
		}

		public static FadeOutEffect CreateAndStart(Widget widget, float time, FadeOutEffectInterpolator interpolator)
		{
			FadeOutEffect fadeOutEffect = new FadeOutEffect(widget, time, interpolator);
			fadeOutEffect.Start();
			return fadeOutEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget != null)
			{
				switch (this.Interpolator)
				{
				case FadeOutEffectInterpolator.Linear:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					break;
				case FadeOutEffectInterpolator.EaseOutQuad:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
					break;
				case FadeOutEffectInterpolator.Custom:
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
				base.Widget.Visible = true;
			}
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.TotalElapsedTime < this.Time)
			{
				float num = this.interpolatorCallback(1f, 0f, base.TotalElapsedTime / this.Time);
				base.Widget.Alpha = ((num > 0f) ? num : 0f);
				return EffectUpdateResponse.Continue;
			}
			base.Widget.Alpha = 0f;
			return EffectUpdateResponse.Finish;
		}

		protected override void OnStop()
		{
		}
	}
}
