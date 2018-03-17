using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct Vector2i
	{
		public int X;

		public int Y;

		public Vector2i Yx
		{
			get
			{
				return new Vector2i(this.Y, this.X);
			}
		}

		public Vector2i(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public Vector2 Vector2()
		{
			return new Vector2((float)this.X, (float)this.Y);
		}

		public Vector2i Max(Vector2i value)
		{
			return new Vector2i(Common.Max(this.X, value.X), Common.Max(this.Y, value.Y));
		}

		public Vector2i Min(Vector2i value)
		{
			return new Vector2i(Common.Min(this.X, value.X), Common.Min(this.Y, value.Y));
		}

		public Vector2i Clamp(Vector2i min, Vector2i max)
		{
			return new Vector2i(Common.Clamp(this.X, min.X, max.X), Common.Clamp(this.Y, min.Y, max.Y));
		}

		public Vector2i ClampIndex(Vector2i n)
		{
			return new Vector2i(Common.ClampIndex(this.X, n.X), Common.ClampIndex(this.Y, n.Y));
		}

		public Vector2i WrapIndex(Vector2i n)
		{
			return new Vector2i(Common.WrapIndex(this.X, n.X), Common.WrapIndex(this.Y, n.Y));
		}

		public static Vector2i operator +(Vector2i a, Vector2i value)
		{
			return new Vector2i(a.X + value.X, a.Y + value.Y);
		}

		public static Vector2i operator -(Vector2i a, Vector2i value)
		{
			return new Vector2i(a.X - value.X, a.Y - value.Y);
		}

		public static Vector2i operator *(Vector2i a, Vector2i value)
		{
			return new Vector2i(a.X * value.X, a.Y * value.Y);
		}

		public static Vector2i operator *(Vector2i a, int value)
		{
			return new Vector2i(a.X * value, a.Y * value);
		}

		public static Vector2i operator *(int value, Vector2i a)
		{
			return new Vector2i(a.X * value, a.Y * value);
		}

		public static Vector2i operator /(Vector2i a, Vector2i value)
		{
			return new Vector2i(a.X / value.X, a.Y / value.Y);
		}

		public static Vector2i operator -(Vector2i a)
		{
			return new Vector2i(-a.X, -a.Y);
		}

		public static bool operator ==(Vector2i a, Vector2i value)
		{
			return a.X == value.X && a.Y == value.Y;
		}

		public static bool operator !=(Vector2i a, Vector2i value)
		{
			return a.X != value.X || a.Y != value.Y;
		}

		public int Product()
		{
			return this.X * this.Y;
		}

		public bool Equals(Vector2i v)
		{
			return this.X == v.X && this.Y == v.Y;
		}

		public override bool Equals(object o)
		{
			return o is Vector2i && this.Equals((Vector2i)o);
		}

		public override string ToString()
		{
			return string.Format("({0},{1})", this.X, this.Y);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}
	}
}
