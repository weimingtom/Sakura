using System;

namespace Sce.Pss.Core
{
	public static class FMath
	{
		public const float E = 2.71828175f;

		public const float PI = 3.14159274f;

		public const float DegToRad = 0.0174532924f;

		public const float RadToDeg = 57.29578f;

		public static float Radians(float x)
		{
			return x * 0.0174532924f;
		}

		public static float Degrees(float x)
		{
			return x * 57.29578f;
		}

		public static float Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		public static float Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		public static float Tan(float x)
		{
			return (float)Math.Tan((double)x);
		}

		public static float Asin(float x)
		{
			return (float)Math.Asin((double)x);
		}

		public static float Acos(float x)
		{
			return (float)Math.Acos((double)x);
		}

		public static float Atan(float x)
		{
			return (float)Math.Atan((double)x);
		}

		public static float Atan2(float x, float y)
		{
			return (float)Math.Atan2((double)x, (double)y);
		}

		public static float Sqrt(float x)
		{
			return (float)Math.Sqrt((double)x);
		}

		public static float Pow(float x, float y)
		{
			return (float)Math.Pow((double)x, (double)y);
		}

		public static float Exp(float x)
		{
			return (float)Math.Exp((double)x);
		}

		public static float Log(float x)
		{
			return (float)Math.Log((double)x);
		}

		public static float Log10(float x)
		{
			return (float)Math.Log10((double)x);
		}

		public static float Abs(float x)
		{
			return Math.Abs(x);
		}

		public static int Sign(float x)
		{
			return Math.Sign(x);
		}

		public static float Min(float x, float y)
		{
			return Math.Min(x, y);
		}

		public static float Max(float x, float y)
		{
			return Math.Max(x, y);
		}

		public static float Floor(float x)
		{
			return (float)Math.Floor((double)x);
		}

		public static float Ceiling(float x)
		{
			return (float)Math.Ceiling((double)x);
		}

		public static float Round(float x)
		{
			return (float)Math.Round((double)x);
		}

		public static float Truncate(float x)
		{
			return (float)Math.Truncate((double)x);
		}

		public static float Clamp(float x, float min, float max)
		{
			return (x <= min) ? min : ((x >= max) ? max : x);
		}

		public static float Repeat(float x, float min, float max)
		{
			float num = max - min;
			float num2 = (num == 0f) ? 0f : ((x - min) % num);
			return min + ((num2 >= 0f) ? num2 : (num2 + num));
		}

		public static float Mirror(float x, float min, float max)
		{
			float num = max - min;
			float num2 = (num == 0f) ? 0f : ((x - min) % (num + num));
			if (num2 < 0f)
			{
				num2 = -num2;
			}
			return min + ((num2 < num) ? num2 : (num + num - num2));
		}

		public static float Lerp(float x1, float x2, float f)
		{
			return (x2 - x1) * f + x1;
		}

		public static float MoveTo(float x1, float x2, float amount)
		{
			return (x2 > x1) ? ((x1 + amount < x2) ? (x1 + amount) : x2) : ((x1 - amount > x2) ? (x1 - amount) : x2);
		}

		public static float Step(float edge, float x)
		{
			return (x < edge) ? 0f : 1f;
		}

		public static float LinearStep(float min, float max, float x)
		{
			return (x <= min) ? 0f : ((x >= max) ? 1f : ((x - min) / (max - min)));
		}

		public static float SmoothStep(float min, float max, float x)
		{
			float num = FMath.LinearStep(min, max, x);
			return num * num * (3f - num - num);
		}

		[Obsolete("Use Repeat()")]
		public static float Wrap(float x, float min, float max)
		{
			return FMath.Repeat(x, min, max);
		}
	}
}
