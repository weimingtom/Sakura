using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.UI
{
	public class ZoomEffect : Effect
	{
		private float time;

		private float targetScaleX;

		private float targetScaleY;

		private float targetScaleZ;

		private float startScaleX;

		private float startScaleY;

		private float startScaleZ;

		private Matrix4 baseTransformMat;

		private AnimationInterpolator interpolatorCallback;

		public float TargetScaleX
		{
			get
			{
				return this.targetScaleX;
			}
			set
			{
				this.targetScaleX = value;
			}
		}

		public float TargetScaleY
		{
			get
			{
				return this.targetScaleY;
			}
			set
			{
				this.targetScaleY = value;
			}
		}

		public float TargetScaleZ
		{
			get
			{
				return this.targetScaleZ;
			}
			set
			{
				this.targetScaleZ = value;
			}
		}

		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		public ZoomEffectInterpolator Interpolator
		{
			get;
			set;
		}

		public AnimationInterpolator CustomInterpolator
		{
			get;
			set;
		}

		public ZoomEffect()
		{
			base.Widget = null;
			this.Time = 1000f;
			this.targetScaleX = 1f;
			this.targetScaleY = 1f;
			this.targetScaleZ = 1f;
			this.Interpolator = ZoomEffectInterpolator.EaseOutQuad;
		}

		public ZoomEffect(Widget widget, float time, float scale, ZoomEffectInterpolator interpolator)
		{
			base.Widget = widget;
			this.Time = time;
			this.targetScaleX = scale;
			this.targetScaleY = scale;
			this.targetScaleZ = scale;
			this.Interpolator = interpolator;
		}

		public static ZoomEffect CreateAndStart(Widget widget, float time, float scale, ZoomEffectInterpolator interpolator)
		{
			ZoomEffect zoomEffect = new ZoomEffect(widget, time, scale, interpolator);
			zoomEffect.Start();
			return zoomEffect;
		}

		protected override void OnStart()
		{
			if (base.Widget != null)
			{
				switch (this.Interpolator)
				{
				case ZoomEffectInterpolator.Linear:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					break;
				case ZoomEffectInterpolator.EaseOutQuad:
					this.interpolatorCallback = new AnimationInterpolator(AnimationUtility.EaseOutQuadInterpolator);
					break;
				case ZoomEffectInterpolator.Overshoot:
					this.interpolatorCallback = new AnimationInterpolator(ZoomEffect.ZoomOvershootInterpolator);
					break;
				case ZoomEffectInterpolator.Elastic:
					this.interpolatorCallback = new AnimationInterpolator(ZoomEffect.ZoomElasticInterpolator);
					break;
				case ZoomEffectInterpolator.Custom:
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
				this.baseTransformMat = base.Widget.Transform3D;
				this.startScaleX = this.baseTransformMat.ColumnX.Length();
				this.startScaleY = this.baseTransformMat.ColumnY.Length();
				this.startScaleZ = this.baseTransformMat.ColumnZ.Length();
				this.baseTransformMat.ColumnX = (this.baseTransformMat.ColumnX / this.startScaleX);
				this.baseTransformMat.ColumnY = (this.baseTransformMat.ColumnY / this.startScaleY);
				this.baseTransformMat.ColumnZ = (this.baseTransformMat.ColumnZ / this.startScaleZ);
			}
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			if (base.Widget == null)
			{
				return EffectUpdateResponse.Finish;
			}
			EffectUpdateResponse result = EffectUpdateResponse.Continue;
			float num;
			float num2;
			float num3;
			if (base.TotalElapsedTime < this.time)
			{
				float ratio = base.TotalElapsedTime / this.time;
				num = this.interpolatorCallback(this.startScaleX, this.targetScaleX, ratio);
				num2 = this.interpolatorCallback(this.startScaleY, this.targetScaleY, ratio);
				num3 = this.interpolatorCallback(this.startScaleZ, this.targetScaleZ, ratio);
			}
			else
			{
				num = this.targetScaleX;
				num2 = this.targetScaleY;
				num3 = this.targetScaleZ;
				result = EffectUpdateResponse.Finish;
			}
			Matrix4 matrix = Matrix4.Scale(new Vector3(num, num2, num3));
			Matrix4 transform3D;
			this.baseTransformMat.Multiply(ref matrix, out transform3D);
			base.Widget.Transform3D = transform3D;
			return result;
		}

		protected override void OnStop()
		{
		}

		private static float ZoomElasticInterpolator(float from, float to, float ratio)
		{
			ratio = AnimationUtility.ElasticInterpolator(0f, (float)Math.Log((double)(to / from)), ratio);
			return (float)Math.Exp((double)ratio) * from;
		}

		private static float ZoomOvershootInterpolator(float from, float to, float ratio)
		{
			ratio = AnimationUtility.OvershootInterpolator(0f, (float)Math.Log((double)(to / from)), ratio);
			return (float)Math.Exp((double)ratio) * from;
		}
	}
}
