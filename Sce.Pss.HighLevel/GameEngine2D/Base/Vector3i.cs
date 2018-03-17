using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct Vector3i
	{
		public int X;

		public int Y;

		public int Z;

		public Vector2i Xx
		{
			get
			{
				return new Vector2i(this.X, this.X);
			}
		}

		public Vector2i Xy
		{
			get
			{
				return new Vector2i(this.X, this.Y);
			}
		}

		public Vector2i Xz
		{
			get
			{
				return new Vector2i(this.X, this.Z);
			}
		}

		public Vector2i Yx
		{
			get
			{
				return new Vector2i(this.Y, this.X);
			}
		}

		public Vector2i Yy
		{
			get
			{
				return new Vector2i(this.Y, this.Y);
			}
		}

		public Vector2i Yz
		{
			get
			{
				return new Vector2i(this.Y, this.Z);
			}
		}

		public Vector2i Zx
		{
			get
			{
				return new Vector2i(this.Z, this.X);
			}
		}

		public Vector2i Zy
		{
			get
			{
				return new Vector2i(this.Z, this.Y);
			}
		}

		public Vector2i Zz
		{
			get
			{
				return new Vector2i(this.Z, this.Z);
			}
		}

		public Vector3i Xxx
		{
			get
			{
				return new Vector3i(this.X, this.X, this.X);
			}
		}

		public Vector3i Xxy
		{
			get
			{
				return new Vector3i(this.X, this.X, this.Y);
			}
		}

		public Vector3i Xxz
		{
			get
			{
				return new Vector3i(this.X, this.X, this.Z);
			}
		}

		public Vector3i Xyx
		{
			get
			{
				return new Vector3i(this.X, this.Y, this.X);
			}
		}

		public Vector3i Xyy
		{
			get
			{
				return new Vector3i(this.X, this.Y, this.Y);
			}
		}

		public Vector3i Xyz
		{
			get
			{
				return new Vector3i(this.X, this.Y, this.Z);
			}
		}

		public Vector3i Xzx
		{
			get
			{
				return new Vector3i(this.X, this.Z, this.X);
			}
		}

		public Vector3i Xzy
		{
			get
			{
				return new Vector3i(this.X, this.Z, this.Y);
			}
		}

		public Vector3i Xzz
		{
			get
			{
				return new Vector3i(this.X, this.Z, this.Z);
			}
		}

		public Vector3i Yxx
		{
			get
			{
				return new Vector3i(this.Y, this.X, this.X);
			}
		}

		public Vector3i Yxy
		{
			get
			{
				return new Vector3i(this.Y, this.X, this.Y);
			}
		}

		public Vector3i Yxz
		{
			get
			{
				return new Vector3i(this.Y, this.X, this.Z);
			}
		}

		public Vector3i Yyx
		{
			get
			{
				return new Vector3i(this.Y, this.Y, this.X);
			}
		}

		public Vector3i Yyy
		{
			get
			{
				return new Vector3i(this.Y, this.Y, this.Y);
			}
		}

		public Vector3i Yyz
		{
			get
			{
				return new Vector3i(this.Y, this.Y, this.Z);
			}
		}

		public Vector3i Yzx
		{
			get
			{
				return new Vector3i(this.Y, this.Z, this.X);
			}
		}

		public Vector3i Yzy
		{
			get
			{
				return new Vector3i(this.Y, this.Z, this.Y);
			}
		}

		public Vector3i Yzz
		{
			get
			{
				return new Vector3i(this.Y, this.Z, this.Z);
			}
		}

		public Vector3i Zxx
		{
			get
			{
				return new Vector3i(this.Z, this.X, this.X);
			}
		}

		public Vector3i Zxy
		{
			get
			{
				return new Vector3i(this.Z, this.X, this.Y);
			}
		}

		public Vector3i Zxz
		{
			get
			{
				return new Vector3i(this.Z, this.X, this.Z);
			}
		}

		public Vector3i Zyx
		{
			get
			{
				return new Vector3i(this.Z, this.Y, this.X);
			}
		}

		public Vector3i Zyy
		{
			get
			{
				return new Vector3i(this.Z, this.Y, this.Y);
			}
		}

		public Vector3i Zyz
		{
			get
			{
				return new Vector3i(this.Z, this.Y, this.Z);
			}
		}

		public Vector3i Zzx
		{
			get
			{
				return new Vector3i(this.Z, this.Z, this.X);
			}
		}

		public Vector3i Zzy
		{
			get
			{
				return new Vector3i(this.Z, this.Z, this.Y);
			}
		}

		public Vector3i Zzz
		{
			get
			{
				return new Vector3i(this.Z, this.Z, this.Z);
			}
		}

		public Vector3i(int x, int y, int z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public Vector3i(Vector2i xy, int z)
		{
			this.X = xy.X;
			this.Y = xy.Y;
			this.Z = z;
		}

		public Vector3 Vector3()
		{
			return new Vector3((float)this.X, (float)this.Y, (float)this.Z);
		}

		public Vector3i Max(Vector3i value)
		{
			return new Vector3i(Common.Max(this.X, value.X), Common.Max(this.Y, value.Y), Common.Max(this.Z, value.Z));
		}

		public Vector3i Min(Vector3i value)
		{
			return new Vector3i(Common.Min(this.X, value.X), Common.Min(this.Y, value.Y), Common.Min(this.Z, value.Z));
		}

		public Vector3i Clamp(Vector3i min, Vector3i max)
		{
			return new Vector3i(Common.Clamp(this.X, min.X, max.X), Common.Clamp(this.Y, min.Y, max.Y), Common.Clamp(this.Z, min.Z, max.Z));
		}

		public Vector3i ClampIndex(Vector3i n)
		{
			return new Vector3i(Common.ClampIndex(this.X, n.X), Common.ClampIndex(this.Y, n.Y), Common.ClampIndex(this.Z, n.Z));
		}

		public Vector3i WrapIndex(Vector3i n)
		{
			return new Vector3i(Common.WrapIndex(this.X, n.X), Common.WrapIndex(this.Y, n.Y), Common.WrapIndex(this.Z, n.Z));
		}

		public static Vector3i operator +(Vector3i a, Vector3i value)
		{
			return new Vector3i(a.X + value.X, a.Y + value.Y, a.Z + value.Z);
		}

		public static Vector3i operator -(Vector3i a, Vector3i value)
		{
			return new Vector3i(a.X - value.X, a.Y - value.Y, a.Z - value.Z);
		}

		public static Vector3i operator *(Vector3i a, Vector3i value)
		{
			return new Vector3i(a.X * value.X, a.Y * value.Y, a.Z * value.Z);
		}

		public static Vector3i operator /(Vector3i a, Vector3i value)
		{
			return new Vector3i(a.X / value.X, a.Y / value.Y, a.Z / value.Z);
		}

		public static bool operator ==(Vector3i a, Vector3i value)
		{
			return a.X == value.X && a.Y == value.Y && a.Z == value.Z;
		}

		public static bool operator !=(Vector3i a, Vector3i value)
		{
			return a.X != value.X || a.Y != value.Y || a.Z != value.Z;
		}

		public int Product()
		{
			return this.X * this.Y * this.Z;
		}

		public bool Equals(Vector3i v)
		{
			return this.X == v.X && this.Y == v.Y && this.Z == v.Z;
		}

		public override bool Equals(object o)
		{
			return o is Vector3i && this.Equals((Vector3i)o);
		}

		public override string ToString()
		{
			return string.Format("({0},{1},{2})", this.X, this.Y, this.Z);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}
	}
}
