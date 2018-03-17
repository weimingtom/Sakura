using System;

namespace Sce.Pss.Core
{
	public struct Vector3 : IEquatable<Vector3>
	{
		public float X;

		public float Y;

		public float Z;

		public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);

		public static readonly Vector3 One = new Vector3(1f, 1f, 1f);

		public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);

		public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);

		public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);

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

		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public Vector3(Vector2 xy, float z)
		{
			this.X = xy.X;
			this.Y = xy.Y;
			this.Z = z;
		}

		public Vector3(float f)
		{
			this.X = f;
			this.Y = f;
			this.Z = f;
		}

		public float Length()
		{
			return (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y + this.Z * this.Z));
		}

		public float LengthSquared()
		{
			return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
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

		public float Dot(Vector3 v)
		{
			return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
		}

		public float Dot(ref Vector3 v)
		{
			return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
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

		public Vector3 Abs()
		{
			Vector3 result;
			this.Abs(out result);
			return result;
		}

		public void Abs(out Vector3 result)
		{
			result.X = ((this.X >= 0f) ? this.X : (-this.X));
			result.Y = ((this.Y >= 0f) ? this.Y : (-this.Y));
			result.Z = ((this.Z >= 0f) ? this.Z : (-this.Z));
		}

		public Vector3 Min(Vector3 v)
		{
			Vector3 result;
			this.Min(ref v, out result);
			return result;
		}

		public void Min(ref Vector3 v, out Vector3 result)
		{
			result.X = ((this.X <= v.X) ? this.X : v.X);
			result.Y = ((this.Y <= v.Y) ? this.Y : v.Y);
			result.Z = ((this.Z <= v.Z) ? this.Z : v.Z);
		}

		public Vector3 Min(float f)
		{
			Vector3 result;
			this.Min(f, out result);
			return result;
		}

		public void Min(float f, out Vector3 result)
		{
			result.X = ((this.X <= f) ? this.X : f);
			result.Y = ((this.Y <= f) ? this.Y : f);
			result.Z = ((this.Z <= f) ? this.Z : f);
		}

		public Vector3 Max(Vector3 v)
		{
			Vector3 result;
			this.Max(ref v, out result);
			return result;
		}

		public void Max(ref Vector3 v, out Vector3 result)
		{
			result.X = ((this.X >= v.X) ? this.X : v.X);
			result.Y = ((this.Y >= v.Y) ? this.Y : v.Y);
			result.Z = ((this.Z >= v.Z) ? this.Z : v.Z);
		}

		public Vector3 Max(float f)
		{
			Vector3 result;
			this.Max(f, out result);
			return result;
		}

		public void Max(float f, out Vector3 result)
		{
			result.X = ((this.X >= f) ? this.X : f);
			result.Y = ((this.Y >= f) ? this.Y : f);
			result.Z = ((this.Z >= f) ? this.Z : f);
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

		public Vector3 MoveTo(Vector3 v, float length)
		{
			Vector3 result;
			this.MoveTo(ref v, length, out result);
			return result;
		}

		public void MoveTo(ref Vector3 v, float length, out Vector3 result)
		{
			float num = this.Distance(v);
			result = ((length >= num) ? v : this.Lerp(v, length / num));
		}

		public Vector3 TurnTo(Vector3 v, float angle)
		{
			Vector3 result;
			this.TurnTo(ref v, angle, out result);
			return result;
		}

		public void TurnTo(ref Vector3 v, float angle, out Vector3 result)
		{
			float num = this.Angle(v);
			result = ((angle >= num) ? v : this.Slerp(v, angle / num));
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

		public Vector3 RotateX(float angle)
		{
			Vector3 result;
			this.RotateX(angle, out result);
			return result;
		}

		public void RotateX(float angle, out Vector3 result)
		{
			Vector2 vector;
			Vector2.Rotation(angle, out vector);
			this.RotateX(ref vector, out result);
		}

		public Vector3 RotateX(Vector2 rotation)
		{
			Vector3 result;
			this.RotateX(ref rotation, out result);
			return result;
		}

		public void RotateX(ref Vector2 rotation, out Vector3 result)
		{
			result.Y = rotation.X * this.Y - rotation.Y * this.Z;
			result.Z = rotation.X * this.Z + rotation.Y * this.Y;
			result.X = this.X;
		}

		public Vector3 RotateY(float angle)
		{
			Vector3 result;
			this.RotateY(angle, out result);
			return result;
		}

		public void RotateY(float angle, out Vector3 result)
		{
			Vector2 vector;
			Vector2.Rotation(angle, out vector);
			this.RotateY(ref vector, out result);
		}

		public Vector3 RotateY(Vector2 rotation)
		{
			Vector3 result;
			this.RotateY(ref rotation, out result);
			return result;
		}

		public void RotateY(ref Vector2 rotation, out Vector3 result)
		{
			result.Z = rotation.X * this.Z - rotation.Y * this.X;
			result.X = rotation.X * this.X + rotation.Y * this.Z;
			result.Y = this.Y;
		}

		public Vector3 RotateZ(float angle)
		{
			Vector3 result;
			this.RotateZ(angle, out result);
			return result;
		}

		public void RotateZ(float angle, out Vector3 result)
		{
			Vector2 vector;
			Vector2.Rotation(angle, out vector);
			this.RotateZ(ref vector, out result);
		}

		public Vector3 RotateZ(Vector2 rotation)
		{
			Vector3 result;
			this.RotateZ(ref rotation, out result);
			return result;
		}

		public void RotateZ(ref Vector2 rotation, out Vector3 result)
		{
			result.X = rotation.X * this.X - rotation.Y * this.Y;
			result.Y = rotation.X * this.Y + rotation.Y * this.X;
			result.Z = this.Z;
		}

		public Vector3 Reflect(Vector3 normal)
		{
			Vector3 result;
			this.Reflect(ref normal, out result);
			return result;
		}

		public void Reflect(ref Vector3 normal, out Vector3 result)
		{
			float num = this.Dot(normal) / normal.LengthSquared();
			Vector3 vector;
			normal.Multiply(2f * num, out vector);
			this.Subtract(ref vector, out result);
		}

		public Vector3 Perpendicular()
		{
			Vector3 result;
			this.Perpendicular(out result);
			return result;
		}

		public void Perpendicular(out Vector3 result)
		{
			float num = this.X * this.X;
			float num2 = this.Y * this.Y;
			float num3 = this.Z * this.Z;
			if (num < num2)
			{
				if (num < num3)
				{
					result.X = 0f;
					result.Y = -this.Z;
					result.Z = this.Y;
				}
				else
				{
					result.Z = 0f;
					result.X = -this.Y;
					result.Y = this.X;
				}
			}
			else if (num2 < num3)
			{
				result.Y = 0f;
				result.Z = -this.X;
				result.X = this.Z;
			}
			else
			{
				result.Z = 0f;
				result.X = -this.Y;
				result.Y = this.X;
			}
		}

		public Vector3 ProjectOnLine(Vector3 point, Vector3 direction)
		{
			Vector3 result;
			this.ProjectOnLine(ref point, ref direction, out result);
			return result;
		}

		public void ProjectOnLine(ref Vector3 point, ref Vector3 direction, out Vector3 result)
		{
			Vector3 vector;
			this.Subtract(ref point, out vector);
			float f = direction.Dot(ref vector) / direction.LengthSquared();
			direction.Multiply(f, out vector);
			point.Add(ref vector, out result);
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

		public bool IsUnit(float epsilon)
		{
			return Math.Abs(this.Length() - 1f) <= epsilon;
		}

		public bool IsZero()
		{
			return this.X == 0f && this.Y == 0f && this.Z == 0f;
		}

		public bool IsOne()
		{
			return this.X == 1f && this.Y == 1f && this.Z == 1f;
		}

		public bool IsInfinity()
		{
			return float.IsInfinity(this.X) || float.IsInfinity(this.Y) || float.IsInfinity(this.Z);
		}

		public bool IsNaN()
		{
			return float.IsNaN(this.X) || float.IsNaN(this.Y) || float.IsNaN(this.Z);
		}

		public bool Equals(Vector3 v, float epsilon)
		{
			return Math.Abs(this.X - v.X) <= epsilon && Math.Abs(this.Y - v.Y) <= epsilon && Math.Abs(this.Z - v.Z) <= epsilon;
		}

		public bool Equals(Vector3 v)
		{
			return this.X == v.X && this.Y == v.Y && this.Z == v.Z;
		}

		public override bool Equals(object o)
		{
			return o is Vector3 && this.Equals((Vector3)o);
		}

		public override string ToString()
		{
			return string.Format("({0:F6},{1:F6},{2:F6})", this.X, this.Y, this.Z);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}

		public static float Length(Vector3 v)
		{
			return v.Length();
		}

		public static float Length(ref Vector3 v)
		{
			return v.Length();
		}

		public static float LengthSquared(Vector3 v)
		{
			return v.LengthSquared();
		}

		public static float LengthSquared(ref Vector3 v)
		{
			return v.LengthSquared();
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

		public static float Dot(Vector3 v1, Vector3 v2)
		{
			return v1.Dot(ref v2);
		}

		public static float Dot(ref Vector3 v1, ref Vector3 v2)
		{
			return v1.Dot(ref v2);
		}

		public static Vector3 Cross(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Cross(ref v2, out result);
			return result;
		}

		public static void Cross(ref Vector3 v1, ref Vector3 v2, out Vector3 result)
		{
			v1.Cross(ref v2, out result);
		}

		public static Vector3 Normalize(Vector3 v)
		{
			Vector3 result;
			v.Normalize(out result);
			return result;
		}

		public static void Normalize(ref Vector3 v, out Vector3 result)
		{
			v.Normalize(out result);
		}

		public static Vector3 Abs(Vector3 v)
		{
			Vector3 result;
			v.Abs(out result);
			return result;
		}

		public static void Abs(ref Vector3 v, out Vector3 result)
		{
			v.Abs(out result);
		}

		public static Vector3 Min(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Min(ref v2, out result);
			return result;
		}

		public static void Min(ref Vector3 v1, ref Vector3 v2, out Vector3 result)
		{
			v1.Min(ref v2, out result);
		}

		public static Vector3 Min(Vector3 v, float f)
		{
			Vector3 result;
			v.Min(f, out result);
			return result;
		}

		public static void Min(ref Vector3 v, float f, out Vector3 result)
		{
			v.Min(f, out result);
		}

		public static Vector3 Max(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Max(ref v2, out result);
			return result;
		}

		public static void Max(ref Vector3 v1, ref Vector3 v2, out Vector3 result)
		{
			v1.Max(ref v2, out result);
		}

		public static Vector3 Max(Vector3 v, float f)
		{
			Vector3 result;
			v.Max(f, out result);
			return result;
		}

		public static void Max(ref Vector3 v, float f, out Vector3 result)
		{
			v.Max(f, out result);
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

		public static Vector3 Repeat(Vector3 v, Vector3 min, Vector3 max)
		{
			Vector3 result;
			v.Repeat(ref min, ref max, out result);
			return result;
		}

		public static void Repeat(ref Vector3 v, ref Vector3 min, ref Vector3 max, out Vector3 result)
		{
			v.Repeat(ref min, ref max, out result);
		}

		public static Vector3 Repeat(Vector3 v, float min, float max)
		{
			Vector3 result;
			v.Repeat(min, max, out result);
			return result;
		}

		public static void Repeat(ref Vector3 v, float min, float max, out Vector3 result)
		{
			v.Repeat(min, max, out result);
		}

		public static Vector3 Lerp(Vector3 v1, Vector3 v2, float f)
		{
			Vector3 result;
			v1.Lerp(ref v2, f, out result);
			return result;
		}

		public static void Lerp(ref Vector3 v1, ref Vector3 v2, float f, out Vector3 result)
		{
			v1.Lerp(ref v2, f, out result);
		}

		public static Vector3 Slerp(Vector3 v1, Vector3 v2, float f)
		{
			Vector3 result;
			v1.Slerp(ref v2, f, out result);
			return result;
		}

		public static void Slerp(ref Vector3 v1, ref Vector3 v2, float f, out Vector3 result)
		{
			v1.Slerp(ref v2, f, out result);
		}

		public static Vector3 MoveTo(Vector3 v1, Vector3 v2, float length)
		{
			Vector3 result;
			v1.MoveTo(ref v2, length, out result);
			return result;
		}

		public static void MoveTo(ref Vector3 v1, ref Vector3 v2, float length, out Vector3 result)
		{
			v1.MoveTo(ref v2, length, out result);
		}

		public static Vector3 TurnTo(Vector3 v1, Vector3 v2, float angle)
		{
			Vector3 result;
			v1.TurnTo(ref v2, angle, out result);
			return result;
		}

		public static void TurnTo(ref Vector3 v1, ref Vector3 v2, float angle, out Vector3 result)
		{
			v1.TurnTo(ref v2, angle, out result);
		}

		public static float Angle(Vector3 v1, Vector3 v2)
		{
			return v1.Angle(ref v2);
		}

		public static float Angle(ref Vector3 v1, ref Vector3 v2)
		{
			return v1.Angle(ref v2);
		}

		public static Vector3 RotateX(Vector3 v, float angle)
		{
			Vector3 result;
			v.RotateX(angle, out result);
			return result;
		}

		public static void RotateX(ref Vector3 v, float angle, out Vector3 result)
		{
			v.RotateX(angle, out result);
		}

		public static Vector3 RotateX(Vector3 v, Vector2 rotation)
		{
			Vector3 result;
			v.RotateX(ref rotation, out result);
			return result;
		}

		public static void RotateX(ref Vector3 v, ref Vector2 rotation, out Vector3 result)
		{
			v.RotateX(ref rotation, out result);
		}

		public static Vector3 RotateY(Vector3 v, float angle)
		{
			Vector3 result;
			v.RotateY(angle, out result);
			return result;
		}

		public static void RotateY(ref Vector3 v, float angle, out Vector3 result)
		{
			v.RotateY(angle, out result);
		}

		public static Vector3 RotateY(Vector3 v, Vector2 rotation)
		{
			Vector3 result;
			v.RotateY(ref rotation, out result);
			return result;
		}

		public static void RotateY(ref Vector3 v, ref Vector2 rotation, out Vector3 result)
		{
			v.RotateY(ref rotation, out result);
		}

		public static Vector3 RotateZ(Vector3 v, float angle)
		{
			Vector3 result;
			v.RotateZ(angle, out result);
			return result;
		}

		public static void RotateZ(ref Vector3 v, float angle, out Vector3 result)
		{
			v.RotateZ(angle, out result);
		}

		public static Vector3 RotateZ(Vector3 v, Vector2 rotation)
		{
			Vector3 result;
			v.RotateZ(ref rotation, out result);
			return result;
		}

		public static void RotateZ(ref Vector3 v, ref Vector2 rotation, out Vector3 result)
		{
			v.RotateZ(ref rotation, out result);
		}

		public static Vector3 Reflect(Vector3 v, Vector3 normal)
		{
			Vector3 result;
			v.Reflect(ref normal, out result);
			return result;
		}

		public static void Reflect(ref Vector3 v, ref Vector3 normal, out Vector3 result)
		{
			v.Reflect(ref normal, out result);
		}

		public static Vector3 Perpendicular(Vector3 v)
		{
			Vector3 result;
			v.Perpendicular(out result);
			return result;
		}

		public static void Perpendicular(ref Vector3 v, out Vector3 result)
		{
			v.Perpendicular(out result);
		}

		public static Vector3 ProjectOnLine(Vector3 v, Vector3 point, Vector3 direction)
		{
			Vector3 result;
			v.ProjectOnLine(ref point, ref direction, out result);
			return result;
		}

		public static void ProjectOnLine(ref Vector3 v, ref Vector3 point, ref Vector3 direction, out Vector3 result)
		{
			v.ProjectOnLine(ref point, ref direction, out result);
		}

		public static Vector3 Add(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Add(ref v2, out result);
			return result;
		}

		public static void Add(ref Vector3 v1, ref Vector3 v2, out Vector3 result)
		{
			v1.Add(ref v2, out result);
		}

		public static Vector3 Subtract(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Subtract(ref v2, out result);
			return result;
		}

		public static void Subtract(ref Vector3 v1, ref Vector3 v2, out Vector3 result)
		{
			v1.Subtract(ref v2, out result);
		}

		public static Vector3 Multiply(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Multiply(ref v2, out result);
			return result;
		}

		public static void Multiply(ref Vector3 v1, ref Vector3 v2, out Vector3 result)
		{
			v1.Multiply(ref v2, out result);
		}

		public static Vector3 Multiply(Vector3 v, float f)
		{
			Vector3 result;
			v.Multiply(f, out result);
			return result;
		}

		public static void Multiply(ref Vector3 v, float f, out Vector3 result)
		{
			v.Multiply(f, out result);
		}

		public static Vector3 Divide(Vector3 v1, Vector3 v2)
		{
			Vector3 result;
			v1.Divide(ref v2, out result);
			return result;
		}

		public static void Divide(ref Vector3 v1, ref Vector3 v2, out Vector3 result)
		{
			v1.Divide(ref v2, out result);
		}

		public static Vector3 Divide(Vector3 v, float f)
		{
			Vector3 result;
			v.Divide(f, out result);
			return result;
		}

		public static void Divide(ref Vector3 v, float f, out Vector3 result)
		{
			v.Divide(f, out result);
		}

		public static Vector3 Negate(Vector3 v)
		{
			Vector3 result;
			v.Negate(out result);
			return result;
		}

		public static void Negate(ref Vector3 v, out Vector3 result)
		{
			v.Negate(out result);
		}

		public static bool operator ==(Vector3 v1, Vector3 v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
		}

		public static bool operator !=(Vector3 v1, Vector3 v2)
		{
			return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
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
	}
}
