using System;

namespace Sce.Pss.Core
{
	public static class FMath
	{
		public const float PI = 3.14159274f;
		
		public static float Radians (float x)
		{
			return x * 0.0174532924f;
		}
		
		public static float Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		public static float Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}
		
		public static float Repeat(float x, float min, float max)
		{
			float num = max - min;
			float num2 = (num == 0f) ? 0f : ((x - min) % num);
			return min + ((num2 >= 0f) ? num2 : (num2 + num));
		}

		public static float Lerp(float x1, float x2, float f)
		{
			return (x2 - x1) * f + x1;
		}
	}
}
