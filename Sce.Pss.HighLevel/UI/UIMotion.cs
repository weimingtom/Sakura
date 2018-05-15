using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public class UIMotion : Effect
	{
		private float[] propertyValues;

		private float duration;

		private Matrix4 motionStartTransform3D;

		private readonly Dictionary<PropertyType, AnimationUtility.CubicBezierCurveSequence> timelines = new Dictionary<PropertyType, AnimationUtility.CubicBezierCurveSequence>();

		private UIMotion() : this((Widget)null, (string)null)
		{
		}

		internal UIMotion(Widget widget, UIMotionData data) : this((Widget)widget, (string)null)
		{
			this.SetMotionData(data);
		}

		public UIMotion(Widget widget, string filePath)
		{
			base.Widget = widget;
			this.propertyValues = new float[Enum.GetValues(typeof(PropertyType)).Length];
			if (filePath != null && !filePath.Equals(""))
			{
				UIMotionData uIMotionData = new UIMotionData();
				uIMotionData.Read(filePath);
				this.SetMotionData(uIMotionData);
			}
		}

		public static UIMotion CreateAndStart(Widget widget, string filePath)
		{
			UIMotion uIMotion = new UIMotion(widget, filePath);
			uIMotion.Start();
			return uIMotion;
		}

		private void SetMotionData(UIMotionData data)
		{
			float timeScale = data.header.timeScale;
			Func<float, float> func = (float timeValue) => timeValue / timeScale * 1000f;
			this.duration = func.Invoke(data.header.duration);
			using (List<UIMotionData.Property>.Enumerator enumerator = data.properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIMotionData.Property current = enumerator.Current;
					AnimationInterpolator easingCurve;
					if (current.header.useTimeMap)
					{
						int strength = data.timeMaps[(int)current.header.timeMapIndex].header.strength;
						switch (data.timeMaps[(int)current.header.timeMapIndex].header.easeType)
						{
						case EaseType.Quadratic:
							easingCurve = AnimationUtility.GetQuadInterpolator(strength);
							break;
						case EaseType.Cubic:
							easingCurve = AnimationUtility.GetCubicInterpolator(strength);
							break;
						case EaseType.Quartic:
							easingCurve = AnimationUtility.GetQuarticInterpolator(strength);
							break;
						case EaseType.Quintic:
							easingCurve = AnimationUtility.GetQuinticInterpolator(strength);
							break;
						case EaseType.DualQuadratic:
							easingCurve = AnimationUtility.GetDualQuadInterpolator(strength);
							break;
						case EaseType.DualCubic:
							easingCurve = AnimationUtility.GetDualCubicInterpolator(strength);
							break;
						case EaseType.DualQuartic:
							easingCurve = AnimationUtility.GetDualQuarticInterpolator(strength);
							break;
						case EaseType.DualQuintic:
							easingCurve = AnimationUtility.GetDualQuinticInterpolator(strength);
							break;
						case EaseType.Bounce:
							easingCurve = AnimationUtility.GetBounceInterpolator(strength);
							break;
						case EaseType.BounceIn:
							easingCurve = AnimationUtility.GetBounceInInterpolator(strength);
							break;
						case EaseType.Spring:
							easingCurve = AnimationUtility.GetSpringInterpolator(strength);
							break;
						case EaseType.SineWave:
							easingCurve = AnimationUtility.GetSineWaveInterpolator(strength);
							break;
						case EaseType.SawtoothWave:
							easingCurve = AnimationUtility.GetSawtoothWaveInterpolator(strength);
							break;
						case EaseType.SquareWave:
							easingCurve = AnimationUtility.GetSquareWaveInterpolator(strength);
							break;
						case EaseType.RandomSquareWave:
							easingCurve = AnimationUtility.GetRandomSquareWaveInterpolator(strength);
							break;
						case EaseType.DampedWave:
							easingCurve = AnimationUtility.GetDampedWaveInterpolator(strength);
							break;
						default:
							easingCurve = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
							break;
						}
					}
					else
					{
						easingCurve = new AnimationInterpolator(AnimationUtility.LinearInterpolator);
					}
					if (current.keyframes.Count > 0)
					{
						UIMotionData.Property.Keyframe keyframe = current.keyframes[0];
						AnimationUtility.CubicBezierCurveSequence cubicBezierCurveSequence = new AnimationUtility.CubicBezierCurveSequence(func.Invoke(keyframe.time), keyframe.anchorY);
						cubicBezierCurveSequence.EasingCurve = easingCurve;
						for (int i = 0; i < current.keyframes.Count - 1; i++)
						{
							int num = i;
							int num2 = i + 1;
							UIMotionData.Property.Keyframe keyframe2 = current.keyframes[num];
							UIMotionData.Property.Keyframe keyframe3 = current.keyframes[num2];
							float num3 = keyframe2.time;
							float num4 = keyframe3.time;
							float control1X = func.Invoke(keyframe2.nextX + num3);
							float control1Y = keyframe2.nextY;
							float control2X = func.Invoke(keyframe3.previousX + num4);
							float control2Y = keyframe3.previousY;
							float nextAnchorX = func.Invoke(keyframe3.time);
							float nextAnchorY = keyframe3.anchorY;
							cubicBezierCurveSequence.AppendSegment(control1X, control1Y, control2X, control2Y, nextAnchorX, nextAnchorY);
						}
						this.timelines.Add(current.header.propertyType, cubicBezierCurveSequence);
					}
				}
			}
		}

		protected override void OnStart()
		{
			this.propertyValues[6] = 1f;
			this.propertyValues[7] = 1f;
			this.propertyValues[8] = 1f;
			this.propertyValues[0] = 0f;
			this.propertyValues[1] = 0f;
			this.propertyValues[2] = 0f;
			this.propertyValues[3] = 0f;
			this.propertyValues[4] = 0f;
			this.propertyValues[5] = 0f;
			this.propertyValues[9] = 0f;
			this.propertyValues[10] = 0f;
			this.propertyValues[11] = 0f;
			this.propertyValues[12] = 100f;
			this.motionStartTransform3D = base.Widget.Transform3D;
		}

		protected override EffectUpdateResponse OnUpdate(float elapsedTime)
		{
			using (Dictionary<PropertyType, AnimationUtility.CubicBezierCurveSequence>.Enumerator enumerator = this.timelines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<PropertyType, AnimationUtility.CubicBezierCurveSequence> current = enumerator.Current;
					this.propertyValues[(int)current.Key] = current.Value.GetValue(base.TotalElapsedTime);
				}
			}
			Matrix4 matrix;
			Matrix4.Scale(this.propertyValues[6], this.propertyValues[7], this.propertyValues[8], out matrix);
			Matrix4 matrix2;
			Matrix4.Translation(this.motionStartTransform3D.M41 + this.propertyValues[0], this.motionStartTransform3D.M42 + this.propertyValues[1], this.motionStartTransform3D.M43 + this.propertyValues[2], out matrix2);
			Matrix4 matrix3;
			Matrix4.RotationXyz(this.propertyValues[3], this.propertyValues[4], this.propertyValues[5], out matrix3);
			Matrix4 matrix4;
			matrix2.Multiply(ref matrix3, out matrix4);
			Matrix4 transform3D;
			matrix4.Multiply(ref matrix, out transform3D);
			base.Widget.Transform3D = transform3D;
			base.Widget.Alpha = this.propertyValues[12] / 100f;
			if (base.TotalElapsedTime >= this.duration)
			{
				return EffectUpdateResponse.Finish;
			}
			return EffectUpdateResponse.Continue;
		}

		public new void Stop()
		{
			base.Stop();
			base.Widget.Transform3D = this.motionStartTransform3D;
		}

		protected override void OnStop()
		{
		}

		protected override void OnRepeat()
		{
			base.Widget.Transform3D = this.motionStartTransform3D;
		}
	}
}
