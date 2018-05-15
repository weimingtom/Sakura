using System;

namespace Sce.Pss.HighLevel.UI
{
	public class FadeInEffect : Effect
	{
		private AnimationInterpolator interpolatorCallback;

		public float Time
		{
			get;
			set;
		}

		public FadeInEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public FadeInEffect()
		{
			base.Widget = null;
			this.Time = 1000f;
			this.Interpolator = FadeInEffectInterpolator.EaseOutQuad;
			this.CustomInterpolator = null;
		}

		public FadeInEffect(Widget widget, float time, FadeInEffectInterpolator interpolator)
		{
			base.Widget = widget;
			this.Time = time;
			this.Interpolator = interpolator;
			this.CustomInterpolator = null;
		}

		public static FadeInEffect CreateAndStart(Widget widget, float time, FadeInEffectInterpolator interpolator)
		{
			FadeInEffect fadeInEffect = new FadeInEffect(widget, time, interpolator);
			fadeInEffect.Start();
			return fadeInEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget != null)
			{
				switch (this.Interpolator)
				{
				case FadeInEffectInterpolator.Linear:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					break;
				case FadeInEffectInterpolator.EaseOutQuad:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
					break;
				case FadeInEffectInterpolator.Custom:
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
				base.Widget.Alpha = 0f;
				base.Widget.Visible = true;
			}
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.TotalElapsedTime < this.Time)
			{
				float num = this.interpolatorCallback(0f, 1f, base.TotalElapsedTime / this.Time);
				base.Widget.Alpha = ((num < 1f) ? num : 1f);
				return EffectUpdateResponse.Continue;
			}
			base.Widget.Alpha = 1f;
			return EffectUpdateResponse.Finish;
		}

		protected override void OnStop()
		{
		}
	}
}
