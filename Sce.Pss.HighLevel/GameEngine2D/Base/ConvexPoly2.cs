using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct ConvexPoly2 : ICollisionBasics
	{
		public Plane2[] Planes;

		private Sphere2 m_sphere;

		public Sphere2 Sphere
		{
			get
			{
				return this.m_sphere;
			}
		}

		public ConvexPoly2(Vector2[] points)
		{
			Vector2 vector = Math._00;
			this.Planes = new Plane2[points.Length];
			int num = points.Length;
			int num2 = num - 1;
			int i = 0;
			while (i < num)
			{
				Vector2 vector2 = points[num2];
				Vector2 vector3 = points[i];
				this.Planes[num2] = new Plane2(vector2, -Math.Perp(vector3 - vector2).Normalize());
				vector += vector2;
				num2 = i++;
			}
			vector /= (float)points.Length;
			float num3 = 0f;
			for (num2 = 0; num2 != points.Length; num2++)
			{
				num3 = FMath.Max(num3, (points[num2] - vector).Length());
			}
			this.m_sphere = new Sphere2(vector, num3);
		}

		public void MakeBox(Bounds2 bounds)
		{
			this.Planes = new Plane2[4];
			this.Planes[0] = new Plane2(bounds.Point00, -Math._10);
			this.Planes[1] = new Plane2(bounds.Point10, -Math._01);
			this.Planes[2] = new Plane2(bounds.Point11, Math._10);
			this.Planes[3] = new Plane2(bounds.Point01, Math._01);
			this.m_sphere = new Sphere2(bounds.Center, bounds.Size.Length() * 0.5f);
		}

		public void MakeRegular(uint num, float r)
		{
			this.Planes = new Plane2[num];
			float num2 = Math.TwicePi * 0.5f / num;
			for (uint num3 = 0u; num3 != num; num3 += 1u)
			{
				float num4 = Math.TwicePi * num3 / num;
				Vector2 vector = Vector2.Rotation(num4 + num2);
				Vector2 a_unit_normal = Vector2.Rotation(num4);
				this.Planes[(int)((UIntPtr)num3)] = new Plane2(vector * r, a_unit_normal);
			}
			this.m_sphere = new Sphere2(Math._00, r);
		}

		public uint Size()
		{
			return (uint)this.Planes.Length;
		}

		public Vector2 GetPoint(int index)
		{
			return this.Planes[index].Base;
		}

		public Vector2 GetNormal(int index)
		{
			return this.Planes[index].UnitNormal;
		}

		public Plane2 GetPlane(int index)
		{
			return this.Planes[index];
		}

		public Bounds2 CalcBounds()
		{
			Bounds2 result;
			if (this.Size() == 0u)
			{
				result = Bounds2.Zero;
			}
			else
			{
				Bounds2 bounds = new Bounds2(this.GetPoint(0), this.GetPoint(0));
				for (int num = 1; num != (int)this.Size(); num++)
				{
					bounds.Add(this.GetPoint(num));
				}
				result = bounds;
			}
			return result;
		}

		public Vector2 CalcCenter()
		{
			Vector2 vector = Math._00;
			float num = 0f;
			int num2 = (int)this.Size();
			int index = num2 - 1;
			int i = 0;
			while (i < num2)
			{
				Vector2 point = this.GetPoint(index);
				Vector2 point2 = this.GetPoint(i);
				float num3 = Math.Det(point, point2);
				num += num3;
				vector += num3 * (point + point2);
				index = i++;
			}
			num /= 2f;
			return vector / (6f * num);
		}

		public float CalcArea()
		{
			float num = 0f;
			int num2 = (int)this.Size();
			int index = num2 - 1;
			int i = 0;
			while (i < num2)
			{
				num += Math.Det(this.GetPoint(index), this.GetPoint(i));
				index = i++;
			}
			return num / 2f;
		}

		public bool IsInside(Vector2 point)
		{
			Plane2[] planes = this.Planes;
			bool result;
			for (int i = 0; i < planes.Length; i++)
			{
				Plane2 plane = planes[i];
				if (plane.SignedDistance(point) > 0f)
				{
					result = false;
					return result;
				}
			}
			result = true;
			return result;
		}

		public void ClosestSurfacePoint(Vector2 point, out Vector2 ret, out float sign)
		{
			ret = Math._00;
			float num = -100000f;
			int num2 = -1;
			bool flag = false;
			for (int num3 = 0; num3 != this.Planes.Length; num3++)
			{
				float num4 = this.Planes[num3].SignedDistance(point);
				if (num4 > 0f)
				{
					flag = true;
				}
				else if (num < num4)
				{
					num = num4;
					num2 = num3;
				}
			}
			if (!flag)
			{
				sign = -1f;
				ret = point - num * this.Planes[num2].UnitNormal;
			}
			else
			{
				float num5 = 0f;
				int num6 = (int)this.Size();
				int num3 = num6 - 1;
				int i = 0;
				while (i < num6)
				{
					Vector2 vector = Math.ClosestSegmentPoint(point, this.GetPoint(num3), this.GetPoint(i));
					float num7 = (vector - point).LengthSquared();
					if (num3 == num6 - 1 || num7 < num5)
					{
						ret = vector;
						num5 = num7;
					}
					num3 = i++;
				}
				sign = 1f;
			}
		}

		public float SignedDistance(Vector2 point)
		{
			float num = 0f;
			Vector2 vector;
			this.ClosestSurfacePoint(point, out vector, out num);
			return num * (vector - point).Length();
		}

		public void Translate(Vector2 dx, ConvexPoly2 poly)
		{
			this.Planes = new Plane2[poly.Planes.Length];
			for (int num = 0; num != poly.Planes.Length; num++)
			{
				this.Planes[num] = poly.Planes[num];
				Plane2[] expr_49_cp_0 = this.Planes;
				int expr_49_cp_1 = num;
				expr_49_cp_0[expr_49_cp_1].Base = expr_49_cp_0[expr_49_cp_1].Base + dx;
			}
			this.m_sphere = poly.m_sphere;
			this.m_sphere.Center = this.m_sphere.Center + dx;
		}

		public bool NegativeClipSegment(ref Vector2 A, ref Vector2 B)
		{
			bool result;
			for (int num = 0; num != this.Planes.Length; num++)
			{
				if (!this.Planes[num].NegativeClipSegment(ref A, ref B))
				{
					result = false;
					return result;
				}
			}
			result = true;
			return result;
		}
	}
}
