using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct Sphere2 : ICollisionBasics
	{
		public Vector2 Center;

		public float Radius;

		public Sphere2(Vector2 center, float radius)
		{
			this.Center = center;
			this.Radius = radius;
		}

		public bool IsInside(Vector2 point)
		{
			return (point - this.Center).LengthSquared() <= this.Radius * this.Radius;
		}

		public void ClosestSurfacePoint(Vector2 point, out Vector2 ret, out float sign)
		{
			Vector2 vector = point - this.Center;
			float num = vector.Length();
			float num2 = num - this.Radius;
			if (num < 1E-05f)
			{
				ret = this.Center + new Vector2(0f, this.Radius);
				sign = -1f;
			}
			else
			{
				ret = point - num2 * (vector / num);
				sign = ((num2 > 0f) ? 1f : -1f);
			}
		}

		public float SignedDistance(Vector2 point)
		{
			return (point - this.Center).Length() - this.Radius;
		}

		public bool NegativeClipSegment(ref Vector2 A, ref Vector2 B)
		{
			Vector2 vector = B - A;
			float num = this.Radius * this.Radius;
			float num2 = 1E-08f;
			bool result;
			if (vector.LengthSquared() <= num2)
			{
				if ((A - this.Center).LengthSquared() >= num)
				{
					result = false;
					return result;
				}
			}
			Vector2 vector2 = this.Center.ProjectOnLine(A, vector);
			float num3 = (vector2 - this.Center).LengthSquared();
			if (num3 >= num)
			{
				result = false;
			}
			else
			{
				float num4 = FMath.Sqrt(FMath.Max(0f, num - num3));
				Vector2 vector3 = vector.Normalize();
				Vector2 vector4 = vector2 - num4 * vector3;
				Vector2 vector5 = vector2 + num4 * vector3;
				if ((A - vector5).Dot(vector) >= 0f)
				{
					result = false;
				}
				else if ((B - vector4).Dot(vector) <= 0f)
				{
					result = false;
				}
				else
				{
					if ((A - vector4).Dot(vector) < 0f)
					{
						A = vector4;
					}
					if ((B - vector5).Dot(vector) > 0f)
					{
						B = vector5;
					}
					result = true;
				}
			}
			return result;
		}
	}
}
