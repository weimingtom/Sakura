using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public static class Math
	{
		public class RandGenerator
		{
			public Random Random;

			public RandGenerator(int seed = 0)
			{
				this.Random = new Random(seed);
			}

			public float NextFloat0_1()
			{
				return (float)this.Random.NextDouble();
			}

			public float NextFloatMinus1_1()
			{
				return this.NextFloat0_1() * 2f - 1f;
			}

			public float NextFloat(float mi, float ma)
			{
				return mi + (ma - mi) * this.NextFloat0_1();
			}

			public Vector2 NextVector2Minus1_1()
			{
				return new Vector2(this.NextFloat(-1f, 1f), this.NextFloat(-1f, 1f));
			}

			public Vector2 NextVector2(Vector2 mi, Vector2 ma)
			{
				return new Vector2(this.NextFloat(mi.X, ma.X), this.NextFloat(mi.Y, ma.Y));
			}

			public Vector2 NextVector2(float mi, float ma)
			{
				return new Vector2(this.NextFloat(mi, ma), this.NextFloat(mi, ma));
			}

			public Vector3 NextVector3(Vector3 mi, Vector3 ma)
			{
				return new Vector3(this.NextFloat(mi.X, ma.X), this.NextFloat(mi.Y, ma.Y), this.NextFloat(mi.Z, ma.Z));
			}

			public Vector4 NextVector4(Vector4 mi, Vector4 ma)
			{
				return new Vector4(this.NextFloat(mi.X, ma.X), this.NextFloat(mi.Y, ma.Y), this.NextFloat(mi.Z, ma.Z), this.NextFloat(mi.W, ma.W));
			}

			public Vector4 NextVector4(float mi, float ma)
			{
				return new Vector4(this.NextFloat(mi, ma), this.NextFloat(mi, ma), this.NextFloat(mi, ma), this.NextFloat(mi, ma));
			}
		}

		public static Vector2i _00i = new Vector2i(0, 0);

		public static Vector2i _10i = new Vector2i(1, 0);

		public static Vector2i _01i = new Vector2i(0, 1);

		public static Vector2i _11i = new Vector2i(1, 1);

		public static Vector3i _000i = new Vector3i(0, 0, 0);

		public static Vector3i _100i = new Vector3i(1, 0, 0);

		public static Vector3i _010i = new Vector3i(0, 1, 0);

		public static Vector3i _110i = new Vector3i(1, 1, 0);

		public static Vector3i _001i = new Vector3i(0, 0, 1);

		public static Vector3i _101i = new Vector3i(1, 0, 1);

		public static Vector3i _011i = new Vector3i(0, 1, 1);

		public static Vector3i _111i = new Vector3i(1, 1, 1);

		public static Vector2 _00 = new Vector2(0f, 0f);

		public static Vector2 _10 = new Vector2(1f, 0f);

		public static Vector2 _01 = new Vector2(0f, 1f);

		public static Vector2 _11 = new Vector2(1f, 1f);

		public static Vector3 _000 = new Vector3(0f, 0f, 0f);

		public static Vector3 _100 = new Vector3(1f, 0f, 0f);

		public static Vector3 _010 = new Vector3(0f, 1f, 0f);

		public static Vector3 _110 = new Vector3(1f, 1f, 0f);

		public static Vector3 _001 = new Vector3(0f, 0f, 1f);

		public static Vector3 _101 = new Vector3(1f, 0f, 1f);

		public static Vector3 _011 = new Vector3(0f, 1f, 1f);

		public static Vector3 _111 = new Vector3(1f, 1f, 1f);

		public static Vector4 _0000 = new Vector4(0f, 0f, 0f, 0f);

		public static Vector4 _1000 = new Vector4(1f, 0f, 0f, 0f);

		public static Vector4 _0100 = new Vector4(0f, 1f, 0f, 0f);

		public static Vector4 _1100 = new Vector4(1f, 1f, 0f, 0f);

		public static Vector4 _0010 = new Vector4(0f, 0f, 1f, 0f);

		public static Vector4 _1010 = new Vector4(1f, 0f, 1f, 0f);

		public static Vector4 _0110 = new Vector4(0f, 1f, 1f, 0f);

		public static Vector4 _1110 = new Vector4(1f, 1f, 1f, 0f);

		public static Vector4 _0001 = new Vector4(0f, 0f, 0f, 1f);

		public static Vector4 _1001 = new Vector4(1f, 0f, 0f, 1f);

		public static Vector4 _0101 = new Vector4(0f, 1f, 0f, 1f);

		public static Vector4 _1101 = new Vector4(1f, 1f, 0f, 1f);

		public static Vector4 _0011 = new Vector4(0f, 0f, 1f, 1f);

		public static Vector4 _1011 = new Vector4(1f, 0f, 1f, 1f);

		public static Vector4 _0111 = new Vector4(0f, 1f, 1f, 1f);

		public static Vector4 _1111 = new Vector4(1f, 1f, 1f, 1f);

		public static Vector4 UV_TransformIdentity = new Vector4(0f, 0f, 1f, 1f);

		public static Vector4 UV_TransformFlipV = new Vector4(0f, 1f, 1f, -1f);

		public static float Pi
		{
			get
			{
				return 3.14159274f;
			}
		}

		public static float TwicePi
		{
			get
			{
				return 6.28318548f;
			}
		}

		public static float HalfPi
		{
			get
			{
				return 1.57079637f;
			}
		}

		public static Matrix4 LookAt(Vector3 eye, Vector3 center, Vector3 _Up)
		{
			Vector3 vector = _Up.Normalize();
			Common.Assert(vector.IsUnit(0.001f));
			float num = 1E-05f;
			Vector3 vector2;
			Vector3 vector3;
			Vector3 vector4;
			if ((eye - center).Length() > num)
			{
				vector2 = (eye - center).Normalize();
				if (FMath.Abs(vector2.Dot(vector)) > 0.9999f)
				{
					vector3 = vector2.Perpendicular();
					vector4 = vector3.Cross(vector2);
				}
				else
				{
					vector4 = vector.Cross(vector2).Normalize();
					vector3 = vector2.Cross(vector4);
				}
			}
			else
			{
				vector3 = vector;
				vector4 = vector3.Perpendicular();
				vector2 = vector4.Cross(vector3);
			}
			Matrix4 matrix = default(Matrix4);
			matrix.ColumnX = vector4.Xyz0;
			matrix.ColumnY = vector3.Xyz0;
			matrix.ColumnZ = vector2.Xyz0;
			matrix.ColumnW = eye.Xyz1;
			Matrix4 result = matrix;
			Common.Assert(result.IsOrthonormal(0.001f));
			return result;
		}

		public static void TranslationRotationScale(ref Matrix3 ret, Vector2 translation, Vector2 rotation, Vector2 scale)
		{
			ret.X = new Vector3(rotation.X * scale.X, rotation.Y * scale.X, 0f);
			ret.Y = new Vector3(-rotation.Y * scale.Y, rotation.X * scale.Y, 0f);
			ret.Z = translation.Xy1;
		}

		public static float Det(Vector2 value1, Vector2 value2)
		{
			return value1.X * value2.Y - value1.Y * value2.X;
		}

		public static float Sign(float x)
		{
			float result;
			if (x < 0f)
			{
				result = -1f;
			}
			else if (x > 0f)
			{
				result = 1f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		public static Vector2 Perp(Vector2 value)
		{
			return new Vector2(-value.Y, value.X);
		}

		public static Vector4 SetAlpha(Vector4 value, float w)
		{
			value.W = w;
			return value;
		}

		public static float SafeAcos(float x)
		{
			Common.Assert(FMath.Abs(x) - 1f < 1E-05f);
			return FMath.Acos(FMath.Clamp(x, -1f, 1f));
		}

		public static float Angle(Vector2 value)
		{
			float num = Math.SafeAcos(value.Normalize().X);
			return (value.Y < 0f) ? (-num) : num;
		}

		public static Vector2 Rotate(Vector2 point, float angle, Vector2 pivot)
		{
			return pivot + (point - pivot).Rotate(angle);
		}

		public static float Deg2Rad(float value)
		{
			return value * 0.0174532924f;
		}

		public static float Rad2Deg(float value)
		{
			return value * 57.29578f;
		}

		public static Vector2 Deg2Rad(Vector2 value)
		{
			return value * 0.0174532924f;
		}

		public static Vector2 Rad2Deg(Vector2 value)
		{
			return value * 57.29578f;
		}

		public static float Lerp(float a, float b, float x)
		{
			return a + x * (b - a);
		}

		public static Vector2 Lerp(Vector2 a, Vector2 b, float x)
		{
			return a + x * (b - a);
		}

		public static Vector3 Lerp(Vector3 a, Vector3 b, float x)
		{
			return a + x * (b - a);
		}

		public static Vector4 Lerp(Vector4 a, Vector4 b, float x)
		{
			return a + x * (b - a);
		}

		public static Vector2 LerpUnitVectors(Vector2 va, Vector2 vb, float x)
		{
			return va.Rotate(va.Angle(vb) * x);
		}

		public static float LerpAngles(float a, float b, float x)
		{
			return Math.Angle(Math.LerpUnitVectors(Vector2.Rotation(a), Vector2.Rotation(b), x));
		}

		public static float Sin(uint period, float phase, uint mstime)
		{
			return FMath.Sin((mstime % period / period + phase) * Math.Pi * 2f);
		}

		public static float Sin(ulong period, float phase, ulong mstime)
		{
			return FMath.Sin((mstime % period / period + phase) * Math.Pi * 2f);
		}

		public static float Linear(float x)
		{
			return x;
		}

		public static float PowerfulScurve(float x, float p1, float p2)
		{
			return FMath.Pow(1f - FMath.Pow(1f - x, p2), p1);
		}

		public static float PowEaseIn(float x, float p)
		{
			return FMath.Pow(x, p);
		}

		public static float PowEaseOut(float x, float p)
		{
			return 1f - Math.PowEaseIn(1f - x, p);
		}

		public static float PowEaseInOut(float x, float p)
		{
			float result;
			if (x < 0.5f)
			{
				result = 0.5f * Math.PowEaseIn(x * 2f, p);
			}
			else
			{
				result = 0.5f + 0.5f * Math.PowEaseOut((x - 0.5f) * 2f, p);
			}
			return result;
		}

		public static float ExpEaseOut(float x, float a)
		{
			return (1f - FMath.Exp(-x * a)) / (1f - FMath.Exp(-a));
		}

		public static float ExpEaseIn(float x, float a)
		{
			return 1f - Math.ExpEaseOut(1f - x, a);
		}

		public static float BackEaseIn(float x, float a)
		{
			return x * x * ((a + 1f) * x - a);
		}

		public static float BackEaseOut(float x, float a)
		{
			return 1f - Math.BackEaseIn(1f - x, a);
		}

		public static float BackEaseInOut(float x, float p)
		{
			float result;
			if (x < 0.5f)
			{
				result = 0.5f * Math.BackEaseIn(x * 2f, p);
			}
			else
			{
				result = 0.5f + 0.5f * Math.BackEaseOut((x - 0.5f) * 2f, p);
			}
			return result;
		}

		public static float Impulse(float x, float b)
		{
			float num = b * x;
			return num * FMath.Exp(1f - num);
		}

		public static float ShockWave(float d, float time, float wave_half_width, float wave_speed, float wave_fade, float d_scale)
		{
			d *= d_scale;
			float num = time * wave_speed;
			float num2 = FMath.Clamp(d - num, -wave_half_width, wave_half_width) / wave_half_width;
			float num3 = (1f + FMath.Cos(Math.Pi * num2)) * 0.5f;
			return num3 * FMath.Exp(-d * wave_fade);
		}

		public static int Log2(int v)
		{
			int num = ((v > 65535) ? 1 : 0) << 4;
			v >>= num;
			int num2 = ((v > 255) ? 1 : 0) << 3;
			v >>= num2;
			num |= num2;
			num2 = ((v > 15) ? 1 : 0) << 2;
			v >>= num2;
			num |= num2;
			num2 = ((v > 3) ? 1 : 0) << 1;
			v >>= num2;
			num |= num2;
			return num | v >> 1;
		}

		public static bool IsPowerOf2(int i)
		{
			return 1 << Math.Log2(i) == i;
		}

		public static int GreatestOrEqualPowerOf2(int i)
		{
			int num = 1 << Math.Log2(i);
			return (num < i) ? (2 * num) : num;
		}

		public static Vector2 ClosestSegmentPoint(Vector2 P, Vector2 A, Vector2 B)
		{
			Vector2 vector = B - A;
			Vector2 result;
			if ((P - A).Dot(vector) <= 0f)
			{
				result = A;
			}
			else if ((P - B).Dot(vector) >= 0f)
			{
				result = B;
			}
			else
			{
				result = P.ProjectOnLine(A, vector);
			}
			return result;
		}
	}
}
