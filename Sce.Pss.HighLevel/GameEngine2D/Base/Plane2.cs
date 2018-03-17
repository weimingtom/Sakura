using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct Plane2 : ICollisionBasics
	{
		public Vector2 Base;

		public Vector2 UnitNormal;

		public Plane2(Vector2 a_base, Vector2 a_unit_normal)
		{
			this.Base = a_base;
			this.UnitNormal = a_unit_normal;
		}

		public bool IsInside(Vector2 point)
		{
			return this.SignedDistance(point) <= 0f;
		}

		public void ClosestSurfacePoint(Vector2 point, out Vector2 ret, out float sign)
		{
			float num = this.SignedDistance(point);
			ret = point - num * this.UnitNormal;
			sign = ((num > 0f) ? 1f : -1f);
		}

		public float SignedDistance(Vector2 point)
		{
			return (point - this.Base).Dot(this.UnitNormal);
		}

		public Vector2 Project(Vector2 point)
		{
			return point - this.SignedDistance(point) * this.UnitNormal;
		}

		public bool NegativeClipSegment(ref Vector2 A, ref Vector2 B)
		{
			float num = this.SignedDistance(A);
			float num2 = this.SignedDistance(B);
			bool flag = num >= 0f;
			bool flag2 = num2 >= 0f;
			bool result;
			if (flag && flag2)
			{
				result = false;
			}
			else
			{
				if (flag && !flag2)
				{
					Vector2 vector = B - A;
					float num3 = -num / vector.Dot(this.UnitNormal);
					Vector2 vector2 = A + num3 * vector;
					A = vector2;
				}
				else if (!flag && flag2)
				{
					Vector2 vector = B - A;
					float num3 = -num / vector.Dot(this.UnitNormal);
					Vector2 vector2 = A + num3 * vector;
					B = vector2;
				}
				result = true;
			}
			return result;
		}
	}
}
