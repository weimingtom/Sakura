using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.UI
{
	public static class AnimationUtility
	{
		internal class CubicBezierCurveSequence
		{
			internal class Segment
			{
				private int refineRepeatCount = 10;

				private float t;

				private float x;

				private float y;

				private float a3x;

				private float a2x;

				private float a1x;

				private float a0x;

				private float a3y;

				private float a2y;

				private float a1y;

				private float a0y;

				private readonly float startX;

				private readonly float endX;

				public int RefineRepeatCount
				{
					get
					{
						return this.refineRepeatCount;
					}
					set
					{
						if (value > 0)
						{
							this.refineRepeatCount = value;
						}
					}
				}

				public float T
				{
					get
					{
						return this.t;
					}
					set
					{
						if (this.t != value)
						{
							this.t = value;
							this.Update();
						}
					}
				}

				public float X
				{
					get
					{
						return this.x;
					}
					set
					{
						if (this.x != value)
						{
							this.T = AnimationUtility.CubicBezierCurveSequence.Segment.SolveEquationByNewtonsMethod((float _t) => this.a3x * _t * _t * _t + this.a2x * _t * _t + this.a1x * _t + this.a0x - value, (float _t) => 3f * this.a3x * _t * _t + 2f * this.a2x * _t + this.a1x, (value - this.startX) / (this.endX - this.startX), this.RefineRepeatCount);
						}
					}
				}

				public float Y
				{
					get
					{
						return this.y;
					}
				}

				public float StartX
				{
					get
					{
						return this.startX;
					}
				}

				public float EndX
				{
					get
					{
						return this.endX;
					}
				}

				public Segment(float anchor1X, float anchor1Y, float control1X, float control1Y, float control2X, float control2Y, float anchor2X, float anchor2Y)
				{
					this.t = 0f;
					this.x = anchor1X;
					this.y = anchor1Y;
					this.startX = anchor1X;
					this.endX = anchor2X;
					this.a3x = -anchor1X + 3f * control1X - 3f * control2X + anchor2X;
					this.a2x = 3f * anchor1X - 6f * control1X + 3f * control2X;
					this.a1x = -3f * anchor1X + 3f * control1X;
					this.a0x = anchor1X;
					this.a3y = -anchor1Y + 3f * control1Y - 3f * control2Y + anchor2Y;
					this.a2y = 3f * anchor1Y - 6f * control1Y + 3f * control2Y;
					this.a1y = -3f * anchor1Y + 3f * control1Y;
					this.a0y = anchor1Y;
				}

				public override bool Equals(object obj)
				{
					AnimationUtility.CubicBezierCurveSequence.Segment segment = obj as AnimationUtility.CubicBezierCurveSequence.Segment;
					return this.startX == segment.startX && this.endX == segment.endX && this.a3x == segment.a3x && this.a2x == segment.a2x && this.a1x == segment.a1x && this.a0x == segment.a0x && this.a3y == segment.a3y && this.a2y == segment.a2y && this.a1y == segment.a1y && this.a0y == segment.a0y;
				}

				public override int GetHashCode()
				{
					return (int)(this.startX + this.endX + this.a3x + this.a2x + this.a1x + this.a0x + this.a3y + this.a2y + this.a1y + this.a0y);
				}

				private void Update()
				{
					float num = this.t * this.t;
					float num2 = num * this.t;
					this.x = this.a3x * num2 + this.a2x * num + this.a1x * this.t + this.a0x;
					this.y = this.a3y * num2 + this.a2y * num + this.a1y * this.t + this.a0y;
				}

				private static float SolveEquationByNewtonsMethod(Func<float, float> equation, Func<float, float> derivative, float initial_root, int refineCount)
				{
					float num = initial_root;
					for (int i = 0; i < refineCount; i++)
					{
						float num2 = equation.Invoke(num);
						if (Math.Abs(num2) < 0.1f)
						{
							break;
						}
						float num3 = derivative.Invoke(num);
						while (Math.Abs(num3) < 0.1f)
						{
							num += 0.001f;
							num3 = derivative.Invoke(num);
						}
						num = FMath.Clamp(num - num2 / num3, 0f, 1f);
					}
					return num;
				}
			}

			private readonly List<AnimationUtility.CubicBezierCurveSequence.Segment> segments = new List<AnimationUtility.CubicBezierCurveSequence.Segment>();

			private float firstX;

			private float firstY;

			private float lastX;

			private float lastY;

			public AnimationInterpolator EasingCurve
			{
				get;
				set;
			}

			public CubicBezierCurveSequence(float startX, float startY)
			{
				this.lastX = startX;
				this.firstX = startX;
				this.lastY = startY;
				this.firstY = startY;
			}

			public void AppendSegment(float control1X, float control1Y, float control2X, float control2Y, float nextAnchorX, float nextAnchorY)
			{
				if (!this.IsValidArguments(control1X, control2X, nextAnchorX))
				{
					throw new ArgumentException("anchor X of Bezier curve segment must be monotonically increase.");
				}
				this.segments.Add(new AnimationUtility.CubicBezierCurveSequence.Segment(this.lastX, this.lastY, control1X, control1Y, control2X, control2Y, nextAnchorX, nextAnchorY));
				this.lastX = nextAnchorX;
				this.lastY = nextAnchorY;
			}

			internal float GetValue(float x)
			{
				if (this.EasingCurve != null && this.lastX != this.firstX)
				{
					x = this.EasingCurve(this.firstX, this.lastX, (x - this.firstX) / (this.lastX - this.firstX));
				}
				AnimationUtility.CubicBezierCurveSequence.Segment segment = this.FindSegment(x);
				if (segment != null)
				{
					segment.X = x;
					return segment.Y;
				}
				if (x >= this.lastX)
				{
					return this.lastY;
				}
				if (x < this.firstX)
				{
					return this.firstY;
				}
				throw new Exception("internal error");
			}

			private bool IsValidArguments(float control1X, float control2X, float nextAnchorX)
			{
				return nextAnchorX >= this.lastX && nextAnchorX >= control2X && nextAnchorX >= control1X && this.lastX <= nextAnchorX && this.lastX <= control1X && this.lastX <= control2X;
			}

			private AnimationUtility.CubicBezierCurveSequence.Segment FindSegment(float x)
			{
				using (List<AnimationUtility.CubicBezierCurveSequence.Segment>.Enumerator enumerator = this.segments.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AnimationUtility.CubicBezierCurveSequence.Segment current = enumerator.Current;
						if (current.StartX <= x && x < current.EndX)
						{
							return current;
						}
					}
				}
				return null;
			}
		}

		private static Func<float, float> _linear = (float x) => x;

		private static Func<float, float> _quadCurve = (float x) => x * x;

		private static Func<float, float> _cubicCurve = (float x) => x * x * x;

		private static Func<float, float> _quarticCurve = (float x) => x * x * x * x;

		private static Func<float, float> _quinticCurve = (float x) => x * x * x * x * x;

		public static float LinearInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (to - from) * ratio + from;
		}

		public static float EaseInQuadInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (to - from) * ratio * ratio + from;
		}

		public static float EaseOutQuadInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return -(to - from) * ratio * (ratio - 2f) + from;
		}

		public static float EaseInOutQuadInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseInQuadInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseOutQuadInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseOutInQuadInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseOutQuadInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseInQuadInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseInCubicInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (to - from) * ratio * ratio * ratio + from;
		}

		public static float EaseOutCubicInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			ratio -= 1f;
			return (to - from) * ratio * ratio * ratio + to;
		}

		public static float EaseInOutCubicInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseInCubicInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseOutCubicInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseOutInCubicInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseOutCubicInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseInCubicInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseInQuartInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			ratio *= ratio;
			return (to - from) * ratio * ratio + from;
		}

		public static float EaseOutQuartInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			ratio -= 1f;
			ratio *= ratio;
			return (from - to) * ratio * ratio + to;
		}

		public static float EaseInOutQuartInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseInQuartInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseOutQuartInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseOutInQuartInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseOutQuartInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseInQuartInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseInQuintInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			float num = ratio * ratio;
			return (to - from) * num * num * ratio + from;
		}

		public static float EaseOutQuintInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			ratio -= 1f;
			float num = ratio * ratio;
			return (to - from) * num * num * ratio + to;
		}

		public static float EaseInOutQuintInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseInQuintInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseOutQuintInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseOutInQuintInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseOutQuintInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseInQuintInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseInSineInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (float)((double)(-(double)(to - from)) * Math.Cos((double)ratio * 3.1415926535897931 / 2.0) + (double)to);
		}

		public static float EaseOutSineInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (float)((double)(to - from) * Math.Sin((double)ratio * 1.5707963267948966) + (double)from);
		}

		public static float EaseInOutSineInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (float)((double)(-(double)(to - from) / 2f) * (Math.Cos(3.1415926535897931 * (double)ratio) - 1.0) + (double)from);
		}

		public static float EaseOutInSineInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseOutSineInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseInSineInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseInExpoInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio > 0f)
			{
				return (to - from) * (float)(Math.Pow(2.0, (double)(10f * (ratio - 1f))) - 0.0010000000474974513) + from;
			}
			return from;
		}

		public static float EaseOutExpoInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 1f)
			{
				return (to - from) * 1.001f * (1f - (float)Math.Pow(2.0, (double)(-10f * ratio))) + from;
			}
			return to;
		}

		public static float EaseInOutExpoInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseInExpoInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseOutExpoInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseOutInExpoInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseOutExpoInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseInExpoInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseInCircInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (float)((double)(-(double)(to - from)) * (Math.Sqrt((double)(1f - ratio * ratio)) - 1.0) + (double)from);
		}

		public static float EaseOutCircInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (float)((double)(to - from) * Math.Sqrt((double)(1f - (ratio -= 1f) * ratio)) + (double)from);
		}

		public static float EaseInOutCircInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseInCircInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseOutCircInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float EaseOutInCircInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.5f)
			{
				return AnimationUtility.EaseOutCircInterpolator(from, (to + from) / 2f, ratio * 2f);
			}
			return AnimationUtility.EaseInCircInterpolator((to + from) / 2f, to, ratio * 2f - 1f);
		}

		public static float ElasticInterpolator(float from, float to, float ratio)
		{
			float num = 0.3f;
			float num2 = num / 4f;
			float num3 = to - from;
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio == 0f)
			{
				return from;
			}
			if (ratio == 1f)
			{
				return to;
			}
			return (float)((double)num3 * Math.Pow(2.0, (double)(-10f * ratio)) * Math.Sin((double)(ratio - num2) * 6.2831853071795862 / (double)num) + (double)to);
		}

		public static float UndershootInterpolator(float from, float to, float ratio)
		{
			float num = 1.70158f;
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (to - from) * ratio * ratio * ((num + 1f) * ratio - num) + from;
		}

		public static float OvershootInterpolator(float from, float to, float ratio)
		{
			float num = 1.70158f;
			ratio = FMath.Clamp(ratio, 0f, 1f);
			return (to - from) * ((ratio -= 1f) * ratio * ((num + 1f) * ratio + num) + 1f) + from;
		}

		public static float BounceInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if ((double)ratio < 0.36363636363636365)
			{
				return (to - from) * (7.5625f * ratio * ratio) + from;
			}
			if (ratio < 0.727272749f)
			{
				return (to - from) * (7.5625f * (ratio -= 0.545454562f) * ratio + 0.75f) + from;
			}
			if (ratio < 0.909090936f)
			{
				return (to - from) * (7.5625f * (ratio -= 0.8181818f) * ratio + 0.9375f) + from;
			}
			return (to - from) * (7.5625f * (ratio -= 0.954545438f) * ratio + 0.984375f) + from;
		}

		public static float FlipBounceInterpolator(float from, float to, float ratio)
		{
			ratio = FMath.Clamp(ratio, 0f, 1f);
			if (ratio < 0.333333343f)
			{
				ratio *= 3f;
				return (to - from) * ratio * ratio + from;
			}
			ratio = (ratio - 0.333333343f) * 1.5f;
			return to + (from - to) / 15f * ((1f - ratio) * (1f - (float)Math.Cos((double)ratio * 18.849555921538759)));
		}

		private static float GetFunctionInterpolateRatio(int strength)
		{
			float num = (float)(-(float)strength) / 100f;
			return Math.Abs(FMath.Max(-1f, FMath.Min(1f, num)));
		}

		private static Func<float, float> InterpolateFunctions(Func<float, float> func1, Func<float, float> func2, float ratio)
		{
			return (float x) => ratio * func1.Invoke(x) + (1f - ratio) * func2.Invoke(x);
		}

		private static Func<float, float> QuadCurve(float a, float b, float c)
		{
			return (float x) => a * (x * x) + b * x + c;
		}

		private static Func<float, float> XShift(Func<float, float> f, float x_shift)
		{
			return (float x) => f.Invoke(x - x_shift);
		}

		private static AnimationInterpolator GetAnimationInterpolator(Func<float, float> ratioConverter)
		{
			return (float from, float to, float ratio) => from + (to - from) * ratioConverter.Invoke(ratio);
		}

		private static Func<float, float> GetRatioInputInverse(Func<float, float> origFunc)
		{
			return (float r) => origFunc.Invoke(1f - r);
		}

		private static Func<float, float> GetRatioOutputInverse(Func<float, float> origFunc)
		{
			return (float r) => 1f - origFunc.Invoke(r);
		}

		private static float GetBounceFirstDuration(int strength)
		{
			float num = 0f;
			for (int i = 1; i <= strength; i++)
			{
				num += 1f / (float)i;
			}
			return 1f / num;
		}

		private static int GetBounceCount(float T0, float ratio)
		{
			float num = T0;
			int num2 = 0;
			while (num < ratio)
			{
				num2++;
				num += T0 / (float)(num2 + 1);
			}
			return num2;
		}

		private static float GetBounceOffset(int totalBound, int boundCount, float T0)
		{
			float num = 0f;
			for (int i = 0; i < boundCount; i++)
			{
				num += T0 / (float)(i + 1);
			}
			return num;
		}

		internal static AnimationInterpolator GetQuadInterpolator(int strength)
		{
			Func<float, float> func = AnimationUtility._quadCurve;
			if (strength > 0)
			{
				func = ((float r) => 1f - AnimationUtility._quadCurve.Invoke(r - 1f));
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetCubicInterpolator(int strength)
		{
			Func<float, float> func = AnimationUtility._cubicCurve;
			if (strength > 0)
			{
				func = ((float r) => 1f + AnimationUtility._cubicCurve.Invoke(r - 1f));
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetQuarticInterpolator(int strength)
		{
			Func<float, float> func = AnimationUtility._quarticCurve;
			if (strength > 0)
			{
				func = ((float r) => 1f - AnimationUtility._quarticCurve.Invoke(r - 1f));
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetQuinticInterpolator(int strength)
		{
			Func<float, float> func = AnimationUtility._quinticCurve;
			if (strength > 0)
			{
				func = ((float r) => 1f + AnimationUtility._quinticCurve.Invoke(r - 1f));
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetDualQuadInterpolator(int strength)
		{
			Func<float, float> func;
			if (strength > 0)
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return AnimationUtility._quadCurve.Invoke(2f * (r - 0.5f)) / 2f + 0.5f;
					}
					return 0.5f - AnimationUtility._quadCurve.Invoke(2f * (r - 0.5f)) / 2f;
				};
			}
			else
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return 1f - AnimationUtility._quadCurve.Invoke(2f * (r - 1f)) / 2f;
					}
					return AnimationUtility._quadCurve.Invoke(2f * r) / 2f;
				};
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetDualCubicInterpolator(int strength)
		{
			Func<float, float> func;
			if (strength > 0)
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return AnimationUtility._cubicCurve.Invoke(2f * (r - 0.5f)) / 2f + 0.5f;
					}
					return 0.5f + AnimationUtility._cubicCurve.Invoke(2f * (r - 0.5f)) / 2f;
				};
			}
			else
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return 1f + AnimationUtility._cubicCurve.Invoke(2f * (r - 1f)) / 2f;
					}
					return AnimationUtility._cubicCurve.Invoke(2f * r) / 2f;
				};
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetDualQuarticInterpolator(int strength)
		{
			Func<float, float> func;
			if (strength > 0)
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return AnimationUtility._quarticCurve.Invoke(2f * (r - 0.5f)) / 2f + 0.5f;
					}
					return 0.5f - AnimationUtility._quarticCurve.Invoke(2f * (r - 0.5f)) / 2f;
				};
			}
			else
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return 1f - AnimationUtility._quarticCurve.Invoke(2f * (r - 1f)) / 2f;
					}
					return AnimationUtility._quarticCurve.Invoke(2f * r) / 2f;
				};
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetDualQuinticInterpolator(int strength)
		{
			Func<float, float> func;
			if (strength > 0)
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return AnimationUtility._quinticCurve.Invoke(2f * (r - 0.5f)) / 2f + 0.5f;
					}
					return 0.5f + AnimationUtility._quinticCurve.Invoke(2f * (r - 0.5f)) / 2f;
				};
			}
			else
			{
				func = delegate(float r)
				{
					if (r >= 0.5f)
					{
						return 1f + AnimationUtility._quinticCurve.Invoke(2f * (r - 1f)) / 2f;
					}
					return AnimationUtility._quinticCurve.Invoke(2f * r) / 2f;
				};
			}
			float functionInterpolateRatio = AnimationUtility.GetFunctionInterpolateRatio(strength);
			return AnimationUtility.GetAnimationInterpolator(AnimationUtility.InterpolateFunctions(func, AnimationUtility._linear, functionInterpolateRatio));
		}

		internal static AnimationInterpolator GetBounceInterpolator(int strength)
		{
			float bounceFirstDuration = AnimationUtility.GetBounceFirstDuration(Math.Abs(strength));
			Func<float, float> bounceRatioConverter = AnimationUtility.GetBounceRatioConverter(Math.Abs(strength), bounceFirstDuration);
			if (strength < 0)
			{
				return AnimationUtility.GetAnimationInterpolator(AnimationUtility.GetRatioInputInverse(bounceRatioConverter));
			}
			return AnimationUtility.GetAnimationInterpolator(bounceRatioConverter);
		}

		private static Func<float, float> GetBounceRatioConverter(int strength, float T0)
		{
			return delegate(float ratio)
			{
				int bounceCount = AnimationUtility.GetBounceCount(T0, ratio);
				Func<float, float> func = AnimationUtility.SelectBounceCurve(strength, bounceCount, T0);
				return func.Invoke(ratio);
			};
		}

		private static Func<float, float> SelectBounceCurve(int totalBound, int boundCount, float T0)
		{
			float num = AnimationUtility.GetBounceFirstDuration(totalBound) / (float)(boundCount + 1);
			float bounceOffset = AnimationUtility.GetBounceOffset(totalBound, boundCount, T0);
			float num2 = FMath.Exp((float)(-(float)boundCount));
			float a = -4f * num2 / num / num;
			float b = 4f * num2 / num;
			float c = 0f;
			return AnimationUtility.XShift(AnimationUtility.QuadCurve(a, b, c), bounceOffset);
		}

		internal static AnimationInterpolator GetBounceInInterpolator(int strength)
		{
			float bounceFirstDuration = AnimationUtility.GetBounceFirstDuration(Math.Abs(strength));
			Func<float, float> bounceInRatioConverter = AnimationUtility.GetBounceInRatioConverter(Math.Abs(strength), bounceFirstDuration);
			if (strength < 0)
			{
				return AnimationUtility.GetAnimationInterpolator(AnimationUtility.GetRatioOutputInverse(AnimationUtility.GetRatioInputInverse(bounceInRatioConverter)));
			}
			return AnimationUtility.GetAnimationInterpolator(bounceInRatioConverter);
		}

		private static Func<float, float> GetBounceInRatioConverter(int strength, float T0)
		{
			return delegate(float ratio)
			{
				int bounceCount = AnimationUtility.GetBounceCount(T0, ratio);
				Func<float, float> func = AnimationUtility.SelectBounceInCurve(strength, bounceCount, T0);
				return func.Invoke(ratio);
			};
		}

		private static Func<float, float> SelectBounceInCurve(int totalBound, int boundCount, float T0)
		{
			float num = T0 / (float)(boundCount + 1);
			float bounceOffset = AnimationUtility.GetBounceOffset(totalBound, boundCount, T0);
			float a;
			float b;
			float c;
			if (boundCount == 0)
			{
				a = 1f / num / num;
				b = 0f;
				c = 0f;
			}
			else
			{
				float num2 = FMath.Exp((float)(-(float)boundCount));
				a = 4f * num2 / num / num;
				b = -4f * num2 / num;
				c = 1f;
			}
			return AnimationUtility.XShift(AnimationUtility.QuadCurve(a, b, c), bounceOffset);
		}

		internal static AnimationInterpolator GetSpringInterpolator(int strength)
		{
			if (strength > 0)
			{
				strength = Math.Min(100, strength);
				float startupTime = 0.4f / (float)strength;
				float cycleTime = (1f - startupTime) / (float)strength;
				float angularFrequency = 6.28318548f / cycleTime;
				Func<float, float> ratioConverter = delegate(float t)
				{
					if (t >= startupTime)
					{
						return 0.7f + 0.3f * FMath.Exp(-(t - startupTime) / cycleTime) * FMath.Cos(angularFrequency * (t - startupTime));
					}
					return FMath.Sin(t * 3.14159274f / 2f / startupTime);
				};
				return AnimationUtility.GetAnimationInterpolator(ratioConverter);
			}
			return new AnimationInterpolator(AnimationUtility.LinearInterpolator);
		}

		internal static AnimationInterpolator GetSineWaveInterpolator(int strength)
		{
			strength = (int)FMath.Clamp((float)strength, 0f, 100f);
			return AnimationUtility.GetAnimationInterpolator(delegate(float ratio)
			{
				if (strength == 0)
				{
					return ratio;
				}
				return (1f + FMath.Sin(ratio * 3.14159274f * (float)strength - 1.57079637f)) / 2f;
			});
		}

		internal static AnimationInterpolator GetSawtoothWaveInterpolator(int strength)
		{
			strength = (int)FMath.Clamp((float)strength, 1f, 100f);
			return AnimationUtility.GetAnimationInterpolator(delegate(float ratio)
			{
				float num = FMath.Repeat(ratio * (float)strength, 0f, 2f);
				if (1f < num)
				{
					return 2f - num;
				}
				return num;
			});
		}

		internal static AnimationInterpolator GetSquareWaveInterpolator(int strength)
		{
			strength = (int)FMath.Clamp((float)strength, 2f, 100f);
			return AnimationUtility.GetAnimationInterpolator(delegate(float ratio)
			{
				float num = FMath.Repeat(ratio * (float)strength, 0f, 2f);
				if (num <= 1f)
				{
					return 0f;
				}
				return 1f;
			});
		}

		internal static AnimationInterpolator GetRandomSquareWaveInterpolator(int strength)
		{
			AnimationInterpolator square = AnimationUtility.GetSquareWaveInterpolator(strength);
			Random random = new Random();
			float prevVal = 0f;
			float randomFacor = 0f;
			return delegate(float from, float to, float ratio)
			{
				float num = square(from, to, ratio);
				if (prevVal != num && prevVal <= 0f)
				{
					randomFacor = (float)random.NextDouble();
				}
				return prevVal = randomFacor * square(from, to, ratio);
			};
		}

		internal static AnimationInterpolator GetDampedWaveInterpolator(int strength)
		{
			strength = (int)FMath.Clamp((float)strength, 0f, 100f);
			return AnimationUtility.GetAnimationInterpolator(delegate(float ratio)
			{
				if (strength == 0)
				{
					return ratio;
				}
				return -FMath.Cos(ratio * 3.14159274f * (float)strength * 2f) * FMath.Exp(-ratio * (float)strength) / 2f;
			});
		}
	}
}
