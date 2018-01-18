﻿using System;

namespace Sce.Pss.Core
{
	public struct Vector3
	{
		public float X;
		public float Y;
		public float Z;

		public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);
		public static readonly Vector3 One = new Vector3(1f, 1f, 1f);
		public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);
		public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);
		public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);		
		
		public override string ToString()
		{
			return string.Format("({0:F6},{1:F6},{2:F6})", this.X, this.Y, this.Z);
		}
		
		public Vector3 (float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;			
		}
		
		public Vector3 Normalize()
		{
			Vector3 result;
			this.Normalize(out result);
			return result;
		}

		public void Normalize(out Vector3 result)
		{
			float num = 1f / this.Length();
			result.X = this.X * num;
			result.Y = this.Y * num;
			result.Z = this.Z * num;
		}
		
		public Vector3 Cross(Vector3 v)
		{
			Vector3 result;
			this.Cross(ref v, out result);
			return result;
		}

		public void Cross(ref Vector3 v, out Vector3 result)
		{
			result.X = this.Y * v.Z - this.Z * v.Y;
			result.Y = this.Z * v.X - this.X * v.Z;
			result.Z = this.X * v.Y - this.Y * v.X;
		}
		
		public float Dot(Vector3 v)
		{
			return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
		}

		public float Dot(ref Vector3 v)
		{
			return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
		}
		
		public float Length()
		{
			return (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y + this.Z * this.Z));
		}
		
		public static Vector3 operator +(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Add(ref v2, out result);
			return result;
		}

		public static Vector3 operator +(Vector3 v, float f)
		{
			return new Vector3(v.X + f, v.Y + f, v.Z + f);
		}

		public static Vector3 operator +(float f, Vector3 v)
		{
			return new Vector3(f + v.X, f + v.Y, f + v.Z);
		}

		public static Vector3 operator -(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Subtract(ref v2, out result);
			return result;
		}

		public static Vector3 operator -(Vector3 v, float f)
		{
			return new Vector3(v.X - f, v.Y - f, v.Z - f);
		}

		public static Vector3 operator -(float f, Vector3 v)
		{
			return new Vector3(f - v.X, f - v.Y, f - v.Z);
		}

		public static Vector3 operator -(Vector3 v)
		{
			Vector3 result;
			v.Negate(out result);
			return result;
		}

		public static Vector3 operator *(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Multiply(ref v2, out result);
			return result;
		}

		public static Vector3 operator *(Vector3 v, float f)
		{
			Vector3 result;
			v.Multiply(f, out result);
			return result;
		}

		public static Vector3 operator *(float f, Vector3 v)
		{
			Vector3 result;
			v.Multiply(f, out result);
			return result;
		}

		public static Vector3 operator /(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Divide(ref v2, out result);
			return result;
		}

		public static Vector3 operator /(Vector3 v, float f)
		{
			Vector3 result;
			v.Divide(f, out result);
			return result;
		}

		public static Vector3 operator /(float f, Vector3 v)
		{
			return new Vector3(f / v.X, f / v.Y, f / v.Z);
		}
		
		public Vector3 Add(Vector3 v)
		{
			Vector3 result;
			this.Add(ref v, out result);
			return result;
		}

		public void Add(ref Vector3 v, out Vector3 result)
		{
			result.X = this.X + v.X;
			result.Y = this.Y + v.Y;
			result.Z = this.Z + v.Z;
		}

		public Vector3 Subtract(Vector3 v)
		{
			Vector3 result;
			this.Subtract(ref v, out result);
			return result;
		}

		public void Subtract(ref Vector3 v, out Vector3 result)
		{
			result.X = this.X - v.X;
			result.Y = this.Y - v.Y;
			result.Z = this.Z - v.Z;
		}

		public Vector3 Multiply(Vector3 v)
		{
			Vector3 result;
			this.Multiply(ref v, out result);
			return result;
		}

		public void Multiply(ref Vector3 v, out Vector3 result)
		{
			result.X = this.X * v.X;
			result.Y = this.Y * v.Y;
			result.Z = this.Z * v.Z;
		}

		public Vector3 Multiply(float f)
		{
			Vector3 result;
			this.Multiply(f, out result);
			return result;
		}

		public void Multiply(float f, out Vector3 result)
		{
			result.X = this.X * f;
			result.Y = this.Y * f;
			result.Z = this.Z * f;
		}

		public Vector3 Divide(Vector3 v)
		{
			Vector3 result;
			this.Divide(ref v, out result);
			return result;
		}

		public void Divide(ref Vector3 v, out Vector3 result)
		{
			result.X = this.X / v.X;
			result.Y = this.Y / v.Y;
			result.Z = this.Z / v.Z;
		}

		public Vector3 Divide(float f)
		{
			Vector3 result;
			this.Divide(f, out result);
			return result;
		}

		public void Divide(float f, out Vector3 result)
		{
			float num = 1f / f;
			result.X = this.X * num;
			result.Y = this.Y * num;
			result.Z = this.Z * num;
		}
		
		public float Distance(Vector3 v)
		{
			return this.Distance(ref v);
		}

		public float Distance(ref Vector3 v)
		{
			float num = this.X - v.X;
			float num2 = this.Y - v.Y;
			float num3 = this.Z - v.Z;
			return (float)Math.Sqrt((double)(num * num + num2 * num2 + num3 * num3));
		}

		public float DistanceSquared(Vector3 v)
		{
			return this.DistanceSquared(ref v);
		}

		public float DistanceSquared(ref Vector3 v)
		{
			float num = this.X - v.X;
			float num2 = this.Y - v.Y;
			float num3 = this.Z - v.Z;
			return num * num + num2 * num2 + num3 * num3;
		}
		
		public static float Distance(Vector3 v1, Vector3 v2)
		{
			return v1.Distance(ref v2);
		}

		public static float Distance(ref Vector3 v1, ref Vector3 v2)
		{
			return v1.Distance(ref v2);
		}

		public static float DistanceSquared(Vector3 v1, Vector3 v2)
		{
			return v1.DistanceSquared(ref v2);
		}

		public static float DistanceSquared(ref Vector3 v1, ref Vector3 v2)
		{
			return v1.DistanceSquared(ref v2);
		}
		
		
		
		public float Angle(Vector3 v)
		{
			return this.Angle(ref v);
		}

		public float Angle(ref Vector3 v)
		{
			float num = this.Dot(v) / (this.Length() * v.Length());
			if (num < -1f)
			{
				num = -1f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return (float)Math.Acos((double)num);
		}
		
		
		
		
		public Vector3 Clamp(Vector3 min, Vector3 max)
		{
			Vector3 result;
			this.Clamp(ref min, ref max, out result);
			return result;
		}

		public void Clamp(ref Vector3 min, ref Vector3 max, out Vector3 result)
		{
			result.X = ((this.X <= min.X) ? min.X : ((this.X >= max.X) ? max.X : this.X));
			result.Y = ((this.Y <= min.Y) ? min.Y : ((this.Y >= max.Y) ? max.Y : this.Y));
			result.Z = ((this.Z <= min.Z) ? min.Z : ((this.Z >= max.Z) ? max.Z : this.Z));
		}

		public Vector3 Clamp(float min, float max)
		{
			Vector3 result;
			this.Clamp(min, max, out result);
			return result;
		}

		public void Clamp(float min, float max, out Vector3 result)
		{
			result.X = ((this.X <= min) ? min : ((this.X >= max) ? max : this.X));
			result.Y = ((this.Y <= min) ? min : ((this.Y >= max) ? max : this.Y));
			result.Z = ((this.Z <= min) ? min : ((this.Z >= max) ? max : this.Z));
		}
		
		public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
		{
			Vector3 result;
			v.Clamp(ref min, ref max, out result);
			return result;
		}

		public static void Clamp(ref Vector3 v, ref Vector3 min, ref Vector3 max, out Vector3 result)
		{
			v.Clamp(ref min, ref max, out result);
		}

		public static Vector3 Clamp(Vector3 v, float min, float max)
		{
			Vector3 result;
			v.Clamp(min, max, out result);
			return result;
		}

		public static void Clamp(ref Vector3 v, float min, float max, out Vector3 result)
		{
			v.Clamp(min, max, out result);
		}
		
		
		
		public Vector3 Repeat(Vector3 min, Vector3 max)
		{
			Vector3 result;
			this.Repeat(ref min, ref max, out result);
			return result;
		}

		public void Repeat(ref Vector3 min, ref Vector3 max, out Vector3 result)
		{
			result.X = FMath.Repeat(this.X, min.X, max.X);
			result.Y = FMath.Repeat(this.Y, min.Y, max.Y);
			result.Z = FMath.Repeat(this.Z, min.Z, max.Z);
		}

		public Vector3 Repeat(float min, float max)
		{
			Vector3 result;
			this.Repeat(min, max, out result);
			return result;
		}

		public void Repeat(float min, float max, out Vector3 result)
		{
			result.X = FMath.Repeat(this.X, min, max);
			result.Y = FMath.Repeat(this.Y, min, max);
			result.Z = FMath.Repeat(this.Z, min, max);
		}
		
		public Vector3 Lerp(Vector3 v, float f)
		{
			Vector3 result;
			this.Lerp(ref v, f, out result);
			return result;
		}

		public void Lerp(ref Vector3 v, float f, out Vector3 result)
		{
			float num = 1f - f;
			result.X = this.X * num + v.X * f;
			result.Y = this.Y * num + v.Y * f;
			result.Z = this.Z * num + v.Z * f;
		}

		public Vector3 Slerp(Vector3 v, float f)
		{
			Vector3 result;
			this.Slerp(ref v, f, out result);
			return result;
		}

		public void Slerp(ref Vector3 v, float f, out Vector3 result)
		{
			result = Matrix4.RotationAxis(this.Cross(v), this.Angle(v) * f).TransformVector(this) * FMath.Lerp(1f, v.Length() / this.Length(), f);
		}
		
		public Vector3 Xxx
		{
			get
			{
				return new Vector3(this.X, this.X, this.X);
			}
		}

		public Vector3 Yxx
		{
			get
			{
				return new Vector3(this.Y, this.X, this.X);
			}
		}

		public Vector3 Zxx
		{
			get
			{
				return new Vector3(this.Z, this.X, this.X);
			}
		}

		public Vector3 Xyx
		{
			get
			{
				return new Vector3(this.X, this.Y, this.X);
			}
		}

		public Vector3 Yyx
		{
			get
			{
				return new Vector3(this.Y, this.Y, this.X);
			}
		}

		public Vector3 Zyx
		{
			get
			{
				return new Vector3(this.Z, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		public Vector3 Xzx
		{
			get
			{
				return new Vector3(this.X, this.Z, this.X);
			}
		}

		public Vector3 Yzx
		{
			get
			{
				return new Vector3(this.Y, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		public Vector3 Zzx
		{
			get
			{
				return new Vector3(this.Z, this.Z, this.X);
			}
		}

		public Vector3 Xxy
		{
			get
			{
				return new Vector3(this.X, this.X, this.Y);
			}
		}

		public Vector3 Yxy
		{
			get
			{
				return new Vector3(this.Y, this.X, this.Y);
			}
		}

		public Vector3 Zxy
		{
			get
			{
				return new Vector3(this.Z, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		public Vector3 Xyy
		{
			get
			{
				return new Vector3(this.X, this.Y, this.Y);
			}
		}

		public Vector3 Yyy
		{
			get
			{
				return new Vector3(this.Y, this.Y, this.Y);
			}
		}

		public Vector3 Zyy
		{
			get
			{
				return new Vector3(this.Z, this.Y, this.Y);
			}
		}

		public Vector3 Xzy
		{
			get
			{
				return new Vector3(this.X, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		public Vector3 Yzy
		{
			get
			{
				return new Vector3(this.Y, this.Z, this.Y);
			}
		}

		public Vector3 Zzy
		{
			get
			{
				return new Vector3(this.Z, this.Z, this.Y);
			}
		}

		public Vector3 Xxz
		{
			get
			{
				return new Vector3(this.X, this.X, this.Z);
			}
		}

		public Vector3 Yxz
		{
			get
			{
				return new Vector3(this.Y, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		public Vector3 Zxz
		{
			get
			{
				return new Vector3(this.Z, this.X, this.Z);
			}
		}

		public Vector3 Xyz
		{
			get
			{
				return new Vector3(this.X, this.Y, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		public Vector3 Yyz
		{
			get
			{
				return new Vector3(this.Y, this.Y, this.Z);
			}
		}

		public Vector3 Zyz
		{
			get
			{
				return new Vector3(this.Z, this.Y, this.Z);
			}
		}

		public Vector3 Xzz
		{
			get
			{
				return new Vector3(this.X, this.Z, this.Z);
			}
		}

		public Vector3 Yzz
		{
			get
			{
				return new Vector3(this.Y, this.Z, this.Z);
			}
		}

		public Vector3 Zzz
		{
			get
			{
				return new Vector3(this.Z, this.Z, this.Z);
			}
		}

		public Vector2 Xx
		{
			get
			{
				return new Vector2(this.X, this.X);
			}
		}

		public Vector2 Yx
		{
			get
			{
				return new Vector2(this.Y, this.X);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
			}
		}

		public Vector2 Zx
		{
			get
			{
				return new Vector2(this.Z, this.X);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
			}
		}

		public Vector2 Xy
		{
			get
			{
				return new Vector2(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		public Vector2 Yy
		{
			get
			{
				return new Vector2(this.Y, this.Y);
			}
		}

		public Vector2 Zy
		{
			get
			{
				return new Vector2(this.Z, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
			}
		}

		public Vector2 Xz
		{
			get
			{
				return new Vector2(this.X, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
			}
		}

		public Vector2 Yz
		{
			get
			{
				return new Vector2(this.Y, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
			}
		}

		public Vector2 Zz
		{
			get
			{
				return new Vector2(this.Z, this.Z);
			}
		}

		public Vector4 Xyz0
		{
			get
			{
				return new Vector4(this.X, this.Y, this.Z, 0f);
			}
		}

		public Vector4 Xyz1
		{
			get
			{
				return new Vector4(this.X, this.Y, this.Z, 1f);
			}
		}

		public float R
		{
			get
			{
				return this.X;
			}
			set
			{
				this.X = value;
			}
		}

		public float G
		{
			get
			{
				return this.Y;
			}
			set
			{
				this.Y = value;
			}
		}

		public float B
		{
			get
			{
				return this.Z;
			}
			set
			{
				this.Z = value;
			}
		}
		
		
		public Vector3 Negate()
		{
			Vector3 result;
			this.Negate(out result);
			return result;
		}

		public void Negate(out Vector3 result)
		{
			result.X = -this.X;
			result.Y = -this.Y;
			result.Z = -this.Z;
		}
		
		
	}
}
