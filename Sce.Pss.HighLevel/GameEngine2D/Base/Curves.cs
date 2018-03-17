using Sce.Pss.Core;
using System;
using System.Collections.Generic;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class Curves
	{
		internal struct CurveEvalSetup
		{
			internal float pf;

			internal int pi;

			internal float frac;

			internal CurveEvalSetup(float u, bool _4_points_setup, bool loop, float num_pointsf)
			{
				if (_4_points_setup && !loop)
				{
					float num = 1f / (num_pointsf - 1f);
					u = num + u * (1f - 2f * num);
				}
				this.pf = u * (num_pointsf - (loop ? 0f : 1f));
				this.pi = (int)FMath.Floor(this.pf);
				this.frac = this.pf - (float)this.pi;
			}
		}

		private static Matrix4 HermiteBasis = new Matrix4(new Vector4(2f, -2f, 1f, 1f), new Vector4(-3f, 3f, -2f, -1f), new Vector4(0f, 0f, 1f, 0f), new Vector4(1f, 0f, 0f, 0f));

		private static Matrix4 BezierBasis = new Matrix4(new Vector4(-1f, 3f, -3f, 1f), new Vector4(3f, -6f, 3f, 0f), new Vector4(-3f, 3f, 0f, 0f), new Vector4(1f, 0f, 0f, 0f));

		private static Matrix4 UniformCubicBspline = new Matrix4(new Vector4(-1f, 3f, -3f, 1f) / 6f, new Vector4(3f, -6f, 3f, 0f) / 6f, new Vector4(-3f, 0f, 3f, 0f) / 6f, new Vector4(1f, 4f, 1f, 0f) / 6f);

		public static float Hermite(float u, Vector4 v)
		{
			float num = u * u;
			return (Curves.HermiteBasis * new Vector4(u * num, num, u, 1f)).Dot(v);
		}

		public static float Hermite(float x, Vector2 p0, Vector2 p1, float t0, float t1)
		{
			float num = p1.X - p0.X;
			Common.Assert(num > 0f);
			return Curves.Hermite((x - p0.X) / num, new Vector4(p0.Y, p1.Y, num * t0, num * t1));
		}

		public static float Hermite(float x, Vector2 p0, Vector2 p2, Vector2 p01, Vector2 p21)
		{
			Common.Assert(p2.X > p0.X);
			Common.Assert(p01.X > p0.X);
			Common.Assert(p21.X > p2.X);
			return Curves.Hermite(x, p0, p2, (p01.Y - p0.Y) / (p01.X - p0.X), (p21.Y - p2.Y) / (p21.X - p2.X));
		}

		public static Vector2 Bezier(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = 1f - t;
			return num * num * (p0 * num + p1 * 3f * t) + t * t * (p2 * 3f * num + p3 * t);
		}

		public static Vector3 Bezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float num = 1f - t;
			return num * num * (p0 * num + p1 * 3f * t) + t * t * (p2 * 3f * num + p3 * t);
		}

		public static Vector4 Bezier(float t, Vector4 p0, Vector4 p1, Vector4 p2, Vector4 p3)
		{
			float num = 1f - t;
			return num * num * (p0 * num + p1 * 3f * t) + t * t * (p2 * 3f * num + p3 * t);
		}

		public static Vector2 CatmullRom(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = t * t;
			float num2 = num * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (-p0 + 3f * p1 - 3f * p2 + p3) * num2);
		}

		public static Vector3 CatmullRom(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float num = t * t;
			float num2 = num * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (-p0 + 3f * p1 - 3f * p2 + p3) * num2);
		}

		public static Vector4 CatmullRom(float t, Vector4 p0, Vector4 p1, Vector4 p2, Vector4 p3)
		{
			float num = t * t;
			float num2 = num * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (-p0 + 3f * p1 - 3f * p2 + p3) * num2);
		}

		public static Vector4 CatmullRomAndDerivative(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			Vector4 vector;
			vector.X = 1f;
			vector.Y = t;
			vector.Z = t * t;
			vector.W = vector.Z * t;
			Vector2 vector2 = 0.5f * (-p0 + p2);
			Vector2 vector3 = 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3);
			Vector2 vector4 = 0.5f * (-p0 + 3f * p1 - 3f * p2 + p3);
			Vector4 result = default(Vector4);
			result.Xy = p1 + vector2 * vector.Y + vector3 * vector.Z + vector4 * vector.W;
			result.Zw = vector2 + vector3 * vector.Y * 2f + vector4 * vector.Z * 3f;
			return result;
		}

		public static Vector2 BezierAuto(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float r = 0.333333343f)
		{
			float num = (p2 - p1).Length();
			Vector2 p4 = p1 + (p2 - p0).Normalize() * num * r;
			Vector2 p5 = p2 - (p3 - p1).Normalize() * num * r;
			return Curves.Bezier(t, p1, p4, p5, p2);
		}

		public static Vector2 Bspline(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = t * t;
			Matrix4 matrix = new Matrix4(p0.Xy01, p1.Xy01, p2.Xy01, p3.Xy01);
			return (matrix * (Curves.UniformCubicBspline * new Vector4(num * t, num, t, 1f))).Xy;
		}

		public static Vector4 Bspline(float t, Vector4 p0, Vector4 p1, Vector4 p2, Vector4 p3)
		{
			float num = t * t;
			Matrix4 matrix = new Matrix4(p0, p1, p2, p3);
			return matrix * (Curves.UniformCubicBspline * new Vector4(num * t, num, t, 1f));
		}

		public static Vector4 BsplineAndDerivative(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = t * t;
			Matrix4 matrix = new Matrix4(p0.Xy01, p1.Xy01, p2.Xy01, p3.Xy01);
			Vector4 result = default(Vector4);
			result.Xy = (matrix * (Curves.UniformCubicBspline * new Vector4(num * t, num, t, 1f))).Xy;
			result.Zw = (matrix * (Curves.UniformCubicBspline * new Vector4(3f * num, 2f * t, 1f, 0f))).Xy;
			return result;
		}

		public static Vector2 CatmullRom(float t, List<Vector2> points, bool loop)
		{
			int count = points.Count;
			Vector2 result;
			if (count == 0)
			{
				result = Math._00;
			}
			else
			{
				Common.IndexWrapMode indexWrapMode = new Common.IndexWrapMode(Common.ClampIndex);
				if (loop)
				{
					indexWrapMode = new Common.IndexWrapMode(Common.WrapIndex);
				}
				Curves.CurveEvalSetup curveEvalSetup = new Curves.CurveEvalSetup(t, true, loop, (float)count);
				result = Curves.CatmullRom(curveEvalSetup.frac, points[indexWrapMode(curveEvalSetup.pi - 1, count)], points[indexWrapMode(curveEvalSetup.pi, count)], points[indexWrapMode(curveEvalSetup.pi + 1, count)], points[indexWrapMode(curveEvalSetup.pi + 2, count)]);
			}
			return result;
		}

		public static Vector4 CatmullRomAndDerivative(float t, List<Vector2> points, bool loop)
		{
			int count = points.Count;
			Vector4 result;
			if (count == 0)
			{
				result = Math._0000;
			}
			else
			{
				Common.IndexWrapMode indexWrapMode = new Common.IndexWrapMode(Common.ClampIndex);
				if (loop)
				{
					indexWrapMode = new Common.IndexWrapMode(Common.WrapIndex);
				}
				Curves.CurveEvalSetup curveEvalSetup = new Curves.CurveEvalSetup(t, true, loop, (float)count);
				result = Curves.CatmullRomAndDerivative(curveEvalSetup.frac, points[indexWrapMode(curveEvalSetup.pi - 1, count)], points[indexWrapMode(curveEvalSetup.pi, count)], points[indexWrapMode(curveEvalSetup.pi + 1, count)], points[indexWrapMode(curveEvalSetup.pi + 2, count)]);
			}
			return result;
		}

		public static Vector2 Bspline(float t, List<Vector2> points, bool loop)
		{
			int count = points.Count;
			Vector2 result;
			if (count == 0)
			{
				result = Math._00;
			}
			else
			{
				Common.IndexWrapMode indexWrapMode = new Common.IndexWrapMode(Common.ClampIndex);
				if (loop)
				{
					indexWrapMode = new Common.IndexWrapMode(Common.WrapIndex);
				}
				Curves.CurveEvalSetup curveEvalSetup = new Curves.CurveEvalSetup(t, true, loop, (float)count);
				result = Curves.Bspline(curveEvalSetup.frac, points[indexWrapMode(curveEvalSetup.pi - 1, count)], points[indexWrapMode(curveEvalSetup.pi, count)], points[indexWrapMode(curveEvalSetup.pi + 1, count)], points[indexWrapMode(curveEvalSetup.pi + 2, count)]);
			}
			return result;
		}

		public static Vector4 BsplineAndDerivative(float t, List<Vector2> points, bool loop)
		{
			int count = points.Count;
			Vector4 result;
			if (count == 0)
			{
				result = Math._0000;
			}
			else
			{
				Common.IndexWrapMode indexWrapMode = new Common.IndexWrapMode(Common.ClampIndex);
				if (loop)
				{
					indexWrapMode = new Common.IndexWrapMode(Common.WrapIndex);
				}
				Curves.CurveEvalSetup curveEvalSetup = new Curves.CurveEvalSetup(t, true, loop, (float)count);
				result = Curves.BsplineAndDerivative(curveEvalSetup.frac, points[indexWrapMode(curveEvalSetup.pi - 1, count)], points[indexWrapMode(curveEvalSetup.pi, count)], points[indexWrapMode(curveEvalSetup.pi + 1, count)], points[indexWrapMode(curveEvalSetup.pi + 2, count)]);
			}
			return result;
		}
	}
}
