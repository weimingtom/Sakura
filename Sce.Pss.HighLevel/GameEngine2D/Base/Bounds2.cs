using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct Bounds2 : ICollisionBasics
	{
		public Vector2 Min;

		public Vector2 Max;

		public static Bounds2 Zero = new Bounds2(new Vector2(0f, 0f), new Vector2(0f, 0f));

		public static Bounds2 Quad0_1 = new Bounds2(new Vector2(0f, 0f), new Vector2(1f, 1f));

		public static Bounds2 QuadMinus1_1 = new Bounds2(new Vector2(-1f, -1f), new Vector2(1f, 1f));

		public float Aspect
		{
			get
			{
				Vector2 size = this.Size;
				return size.X / size.Y;
			}
		}

		public Vector2 Center
		{
			get
			{
				return (this.Max + this.Min) * 0.5f;
			}
		}

		public Vector2 Size
		{
			get
			{
				return this.Max - this.Min;
			}
		}

		public Vector2 Point00
		{
			get
			{
				return this.Min;
			}
		}

		public Vector2 Point11
		{
			get
			{
				return this.Max;
			}
		}

		public Vector2 Point10
		{
			get
			{
				return new Vector2(this.Max.X, this.Min.Y);
			}
		}

		public Vector2 Point01
		{
			get
			{
				return new Vector2(this.Min.X, this.Max.Y);
			}
		}

		public bool IsEmpty()
		{
			return this.Max == this.Min;
		}

		public Bounds2(Vector2 min, Vector2 max)
		{
			this.Min = min;
			this.Max = max;
		}

		public Bounds2(Vector2 point)
		{
			this.Min = point;
			this.Max = point;
		}

		public static Bounds2 SafeBounds(Vector2 min, Vector2 max)
		{
			return new Bounds2(min.Min(max), min.Max(max));
		}

		public static Bounds2 CenteredSquare(float h)
		{
			Vector2 vector = new Vector2(h, h);
			return new Bounds2(-vector, vector);
		}

		public static Bounds2 operator +(Bounds2 bounds, Vector2 value)
		{
			return new Bounds2(bounds.Min + value, bounds.Max + value);
		}

		public static Bounds2 operator -(Bounds2 bounds, Vector2 value)
		{
			return new Bounds2(bounds.Min - value, bounds.Max - value);
		}

		public bool Overlaps(Bounds2 bounds)
		{
			return this.Min.X <= bounds.Max.X && bounds.Min.X <= this.Max.X && this.Min.Y <= bounds.Max.Y && bounds.Min.Y <= this.Max.Y;
		}

		public Bounds2 Intersection(Bounds2 bounds)
		{
			Vector2 vector = this.Min.Max(bounds.Min);
			Vector2 vector2 = this.Max.Min(bounds.Max);
			Vector2 vector3 = vector2 - vector;
			Bounds2 result;
			if (vector3.X < 0f || vector3.Y < 0f)
			{
				result = Bounds2.Zero;
			}
			else
			{
				result = new Bounds2(vector, vector2);
			}
			return result;
		}

		public Bounds2 Scale(Vector2 scale, Vector2 center)
		{
			return new Bounds2((this.Min - center) * scale + center, (this.Max - center) * scale + center);
		}

		public void Add(Vector2 point)
		{
			this.Min = this.Min.Min(point);
			this.Max = this.Max.Max(point);
		}

		public void Add(Bounds2 bounds)
		{
			this.Add(bounds.Min);
			this.Add(bounds.Max);
		}

		public override string ToString()
		{
			return this.Min.ToString() + " " + this.Max.ToString();
		}

		public bool IsInside(Vector2 point)
		{
			return point == point.Max(this.Min).Min(this.Max);
		}

		public void ClosestSurfacePoint(Vector2 point, out Vector2 ret, out float sign)
		{
			Vector2 vector = point.Max(this.Min).Min(this.Max);
			if (vector != point)
			{
				ret = vector;
				sign = 1f;
			}
			else
			{
				Vector2 vector2 = vector;
				vector2.X = this.Min.X;
				float num = vector.X - this.Min.X;
				Vector2 vector3 = vector;
				vector3.X = this.Max.X;
				float num2 = this.Max.X - vector.X;
				Vector2 vector4 = vector;
				vector4.Y = this.Min.Y;
				float num3 = vector.Y - this.Min.Y;
				Vector2 vector5 = vector;
				vector5.Y = this.Max.Y;
				float num4 = this.Max.Y - vector.Y;
				ret = vector2;
				float num5 = num;
				if (num5 > num2)
				{
					ret = vector3;
					num5 = num2;
				}
				if (num5 > num3)
				{
					ret = vector4;
					num5 = num3;
				}
				if (num5 > num4)
				{
					ret = vector5;
				}
				sign = -1f;
			}
		}

		public float SignedDistance(Vector2 point)
		{
			float num = 0f;
			Vector2 vector;
			this.ClosestSurfacePoint(point, out vector, out num);
			return num * (vector - point).Length();
		}

		public bool NegativeClipSegment(ref Vector2 A, ref Vector2 B)
		{
			bool flag = true;
			flag &= new Plane2(this.Min, -Math._10).NegativeClipSegment(ref A, ref B);
			flag &= new Plane2(this.Min, -Math._01).NegativeClipSegment(ref A, ref B);
			flag &= new Plane2(this.Max, Math._10).NegativeClipSegment(ref A, ref B);
			return flag & new Plane2(this.Max, Math._01).NegativeClipSegment(ref A, ref B);
		}

		public Bounds2 OutrageousYTopBottomSwap()
		{
			Bounds2 result = this;
			float y = this.Min.Y;
			result.Min.Y = result.Max.Y;
			result.Max.Y = y;
			return result;
		}

		public Bounds2 OutrageousYVCoordFlip()
		{
			Bounds2 result = this;
			result.Min.Y = 1f - result.Min.Y;
			result.Max.Y = 1f - result.Max.Y;
			return result;
		}
	}
}
