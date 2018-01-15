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
	}
}
