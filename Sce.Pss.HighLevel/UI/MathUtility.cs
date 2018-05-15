using System;

namespace Sce.Pss.HighLevel.UI
{
	internal static class MathUtility
	{
		internal const float EPSILON = 0.0001f;

		internal const float F_PI = 3.14159274f;

		internal const float F_2PI = 6.28318548f;

		internal static float Lerp(float from, float to, float ratio)
		{
			if (ratio < 0.0001f)
			{
				return from;
			}
			if (ratio > 0.9999f)
			{
				return to;
			}
			return from * (1f - ratio) + to * ratio;
		}

		internal static T Clamp<T>(T x, T min, T max) where T : IComparable
		{
			if (x.CompareTo(min) < 0)
			{
				return min;
			}
			if (x.CompareTo(max) <= 0)
			{
				return x;
			}
			return max;
		}

		internal static float DegreeToRadian(float degree)
		{
			return degree * 0.0174532924f;
		}

		internal static float RadianToDegree(float radian)
		{
			return radian * 57.2957764f;
		}
	}
}
