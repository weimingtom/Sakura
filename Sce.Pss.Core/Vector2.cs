using System;

namespace Sce.Pss.Core
{
	public struct Vector2 : IEquatable<Vector2>
	{
		public float X;

		public float Y;

		public static readonly Vector2 Zero = new Vector2(0f, 0f);

		public static readonly Vector2 One = new Vector2(1f, 1f);

		public static readonly Vector2 UnitX = new Vector2(1f, 0f);

		public static readonly Vector2 UnitY = new Vector2(0f, 1f);

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

		public Vector3 Xy0
		{
			get
			{
				return new Vector3(this.X, this.Y, 0f);
			}
		}

		public Vector3 Xy1
		{
			get
			{
				return new Vector3(this.X, this.Y, 1f);
			}
		}

		public Vector4 Xy00
		{
			get
			{
				return new Vector4(this.X, this.Y, 0f, 0f);
			}
		}

		public Vector4 Xy01
		{
			get
			{
				return new Vector4(this.X, this.Y, 0f, 1f);
			}
		}

		public Vector4 Xy10
		{
			get
			{
				return new Vector4(this.X, this.Y, 1f, 0f);
			}
		}

		public Vector4 Xy11
		{
			get
			{
				return new Vector4(this.X, this.Y, 1f, 1f);
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

		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public Vector2(float f)
		{
			this.X = f;
			this.Y = f;
		}

		public float Length()
		{
			return (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
		}

		public float LengthSquared()
		{
			return this.X * this.X + this.Y * this.Y;
		}

		public float Distance(Vector2 v)
		{
			return this.Distance(ref v);
		}

		public float Distance(ref Vector2 v)
		{
			float num = this.X - v.X;
			float num2 = this.Y - v.Y;
			return (float)Math.Sqrt((double)(num * num + num2 * num2));
		}

		public float DistanceSquared(Vector2 v)
		{
			return this.DistanceSquared(ref v);
		}

		public float DistanceSquared(ref Vector2 v)
		{
			float num = this.X - v.X;
			float num2 = this.Y - v.Y;
			return num * num + num2 * num2;
		}

		public float Dot(Vector2 v)
		{
			return this.X * v.X + this.Y * v.Y;
		}

		public float Dot(ref Vector2 v)
		{
			return this.X * v.X + this.Y * v.Y;
		}

		public float Determinant(Vector2 v)
		{
			return this.X * v.Y - this.Y * v.X;
		}

		public float Determinant(ref Vector2 v)
		{
			return this.X * v.Y - this.Y * v.X;
		}

		public Vector2 Normalize()
		{
			Vector2 result;
			this.Normalize(out result);
			return result;
		}

		public void Normalize(out Vector2 result)
		{
			float num = 1f / this.Length();
			result.X = this.X * num;
			result.Y = this.Y * num;
		}

		public Vector2 Abs()
		{
			Vector2 result;
			this.Abs(out result);
			return result;
		}

		public void Abs(out Vector2 result)
		{
			result.X = ((this.X >= 0f) ? this.X : (-this.X));
			result.Y = ((this.Y >= 0f) ? this.Y : (-this.Y));
		}

		public Vector2 Min(Vector2 v)
		{
			Vector2 result;
			this.Min(ref v, out result);
			return result;
		}

		public void Min(ref Vector2 v, out Vector2 result)
		{
			result.X = ((this.X <= v.X) ? this.X : v.X);
			result.Y = ((this.Y <= v.Y) ? this.Y : v.Y);
		}

		public Vector2 Min(float f)
		{
			Vector2 result;
			this.Min(f, out result);
			return result;
		}

		public void Min(float f, out Vector2 result)
		{
			result.X = ((this.X <= f) ? this.X : f);
			result.Y = ((this.Y <= f) ? this.Y : f);
		}

		public Vector2 Max(Vector2 v)
		{
			Vector2 result;
			this.Max(ref v, out result);
			return result;
		}

		public void Max(ref Vector2 v, out Vector2 result)
		{
			result.X = ((this.X >= v.X) ? this.X : v.X);
			result.Y = ((this.Y >= v.Y) ? this.Y : v.Y);
		}

		public Vector2 Max(float f)
		{
			Vector2 result;
			this.Max(f, out result);
			return result;
		}

		public void Max(float f, out Vector2 result)
		{
			result.X = ((this.X >= f) ? this.X : f);
			result.Y = ((this.Y >= f) ? this.Y : f);
		}

		public Vector2 Clamp(Vector2 min, Vector2 max)
		{
			Vector2 result;
			this.Clamp(ref min, ref max, out result);
			return result;
		}

		public void Clamp(ref Vector2 min, ref Vector2 max, out Vector2 result)
		{
			result.X = ((this.X <= min.X) ? min.X : ((this.X >= max.X) ? max.X : this.X));
			result.Y = ((this.Y <= min.Y) ? min.Y : ((this.Y >= max.Y) ? max.Y : this.Y));
		}

		public Vector2 Clamp(float min, float max)
		{
			Vector2 result;
			this.Clamp(min, max, out result);
			return result;
		}

		public void Clamp(float min, float max, out Vector2 result)
		{
			result.X = ((this.X <= min) ? min : ((this.X >= max) ? max : this.X));
			result.Y = ((this.Y <= min) ? min : ((this.Y >= max) ? max : this.Y));
		}

		public Vector2 Repeat(Vector2 min, Vector2 max)
		{
			Vector2 result;
			this.Repeat(ref min, ref max, out result);
			return result;
		}

		public void Repeat(ref Vector2 min, ref Vector2 max, out Vector2 result)
		{
			result.X = FMath.Repeat(this.X, min.X, max.X);
			result.Y = FMath.Repeat(this.Y, min.Y, max.Y);
		}

		public Vector2 Repeat(float min, float max)
		{
			Vector2 result;
			this.Repeat(min, max, out result);
			return result;
		}

		public void Repeat(float min, float max, out Vector2 result)
		{
			result.X = FMath.Repeat(this.X, min, max);
			result.Y = FMath.Repeat(this.Y, min, max);
		}

		public Vector2 Lerp(Vector2 v, float f)
		{
			Vector2 result;
			this.Lerp(ref v, f, out result);
			return result;
		}

		public void Lerp(ref Vector2 v, float f, out Vector2 result)
		{
			float num = 1f - f;
			result.X = this.X * num + v.X * f;
			result.Y = this.Y * num + v.Y * f;
		}

		public Vector2 Slerp(Vector2 v, float f)
		{
			Vector2 result;
			this.Slerp(ref v, f, out result);
			return result;
		}

		public void Slerp(ref Vector2 v, float f, out Vector2 result)
		{
			result = this.Rotate(this.Angle(v) * f) * FMath.Lerp(1f, v.Length() / this.Length(), f);
		}

		public Vector2 MoveTo(Vector2 v, float length)
		{
			Vector2 result;
			this.MoveTo(ref v, length, out result);
			return result;
		}

		public void MoveTo(ref Vector2 v, float length, out Vector2 result)
		{
			float num = this.Distance(v);
			result = ((length >= num) ? v : this.Lerp(v, length / num));
		}

		public Vector2 TurnTo(Vector2 v, float angle)
		{
			Vector2 result;
			this.TurnTo(ref v, angle, out result);
			return result;
		}

		public void TurnTo(ref Vector2 v, float angle, out Vector2 result)
		{
			float num = this.Angle(v);
			if (num < 0f)
			{
				num = -num;
			}
			result = ((angle >= num) ? v : this.Slerp(v, angle / num));
		}

		public float Angle(Vector2 v)
		{
			return this.Angle(ref v);
		}

		public float Angle(ref Vector2 v)
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
			float num2 = (float)Math.Acos((double)num);
			return (this.X * v.Y - this.Y * v.X >= 0f) ? num2 : (-num2);
		}

		public Vector2 Rotate(float angle)
		{
			Vector2 result;
			this.Rotate(angle, out result);
			return result;
		}

		public void Rotate(float angle, out Vector2 result)
		{
			Vector2 vector;
			Vector2.Rotation(angle, out vector);
			this.Rotate(ref vector, out result);
		}

		public Vector2 Rotate(Vector2 rotation)
		{
			Vector2 result;
			this.Rotate(ref rotation, out result);
			return result;
		}

		public void Rotate(ref Vector2 rotation, out Vector2 result)
		{
			result.X = rotation.X * this.X - rotation.Y * this.Y;
			result.Y = rotation.X * this.Y + rotation.Y * this.X;
		}

		public Vector2 Reflect(Vector2 normal)
		{
			Vector2 result;
			this.Reflect(ref normal, out result);
			return result;
		}

		public void Reflect(ref Vector2 normal, out Vector2 result)
		{
			float num = this.Dot(normal) / normal.LengthSquared();
			Vector2 vector;
			normal.Multiply(2f * num, out vector);
			this.Subtract(ref vector, out result);
		}

		public Vector2 Perpendicular()
		{
			Vector2 result;
			this.Perpendicular(out result);
			return result;
		}

		public void Perpendicular(out Vector2 result)
		{
			result.X = -this.Y;
			result.Y = this.X;
		}

		public Vector2 ProjectOnLine(Vector2 point, Vector2 direction)
		{
			Vector2 result;
			this.ProjectOnLine(ref point, ref direction, out result);
			return result;
		}

		public void ProjectOnLine(ref Vector2 point, ref Vector2 direction, out Vector2 result)
		{
			Vector2 vector;
			this.Subtract(ref point, out vector);
			float f = direction.Dot(ref vector) / direction.LengthSquared();
			direction.Multiply(f, out vector);
			point.Add(ref vector, out result);
		}

		public Vector2 Add(Vector2 v)
		{
			Vector2 result;
			this.Add(ref v, out result);
			return result;
		}

		public void Add(ref Vector2 v, out Vector2 result)
		{
			result.X = this.X + v.X;
			result.Y = this.Y + v.Y;
		}

		public Vector2 Subtract(Vector2 v)
		{
			Vector2 result;
			this.Subtract(ref v, out result);
			return result;
		}

		public void Subtract(ref Vector2 v, out Vector2 result)
		{
			result.X = this.X - v.X;
			result.Y = this.Y - v.Y;
		}

		public Vector2 Multiply(Vector2 v)
		{
			Vector2 result;
			this.Multiply(ref v, out result);
			return result;
		}

		public void Multiply(ref Vector2 v, out Vector2 result)
		{
			result.X = this.X * v.X;
			result.Y = this.Y * v.Y;
		}

		public Vector2 Multiply(float f)
		{
			Vector2 result;
			this.Multiply(f, out result);
			return result;
		}

		public void Multiply(float f, out Vector2 result)
		{
			result.X = this.X * f;
			result.Y = this.Y * f;
		}

		public Vector2 Divide(Vector2 v)
		{
			Vector2 result;
			this.Divide(ref v, out result);
			return result;
		}

		public void Divide(ref Vector2 v, out Vector2 result)
		{
			result.X = this.X / v.X;
			result.Y = this.Y / v.Y;
		}

		public Vector2 Divide(float f)
		{
			Vector2 result;
			this.Divide(f, out result);
			return result;
		}

		public void Divide(float f, out Vector2 result)
		{
			float num = 1f / f;
			result.X = this.X * num;
			result.Y = this.Y * num;
		}

		public Vector2 Negate()
		{
			Vector2 result;
			this.Negate(out result);
			return result;
		}

		public void Negate(out Vector2 result)
		{
			result.X = -this.X;
			result.Y = -this.Y;
		}

		public bool IsUnit(float epsilon)
		{
			return Math.Abs(this.Length() - 1f) <= epsilon;
		}

		public bool IsZero()
		{
			return this.X == 0f && this.Y == 0f;
		}

		public bool IsOne()
		{
			return this.X == 1f && this.Y == 1f;
		}

		public bool IsInfinity()
		{
			return float.IsInfinity(this.X) || float.IsInfinity(this.Y);
		}

		public bool IsNaN()
		{
			return float.IsNaN(this.X) || float.IsNaN(this.Y);
		}

		public bool Equals(Vector2 v, float epsilon)
		{
			return Math.Abs(this.X - v.X) <= epsilon && Math.Abs(this.Y - v.Y) <= epsilon;
		}

		public bool Equals(Vector2 v)
		{
			return this.X == v.X && this.Y == v.Y;
		}

		public override bool Equals(object o)
		{
			return o is Vector2 && this.Equals((Vector2)o);
		}

		public override string ToString()
		{
			return string.Format("({0:F6},{1:F6})", this.X, this.Y);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		public static Vector2 Rotation(float angle)
		{
			Vector2 result;
			Vector2.Rotation(angle, out result);
			return result;
		}

		public static void Rotation(float angle, out Vector2 result)
		{
			result.X = (float)Math.Cos((double)angle);
			result.Y = (float)Math.Sin((double)angle);
		}

		public static float Length(Vector2 v)
		{
			return v.Length();
		}

		public static float Length(ref Vector2 v)
		{
			return v.Length();
		}

		public static float LengthSquared(Vector2 v)
		{
			return v.LengthSquared();
		}

		public static float LengthSquared(ref Vector2 v)
		{
			return v.LengthSquared();
		}

		public static float Distance(Vector2 v1, Vector2 v2)
		{
			return v1.Distance(ref v2);
		}

		public static float Distance(ref Vector2 v1, ref Vector2 v2)
		{
			return v1.Distance(ref v2);
		}

		public static float DistanceSquared(Vector2 v1, Vector2 v2)
		{
			return v1.DistanceSquared(ref v2);
		}

		public static float DistanceSquared(ref Vector2 v1, ref Vector2 v2)
		{
			return v1.DistanceSquared(ref v2);
		}

		public static float Dot(Vector2 v1, Vector2 v2)
		{
			return v1.Dot(ref v2);
		}

		public static float Dot(ref Vector2 v1, ref Vector2 v2)
		{
			return v1.Dot(ref v2);
		}

		public static float Determinant(Vector2 v1, Vector2 v2)
		{
			return v1.Determinant(ref v2);
		}

		public static float Determinant(ref Vector2 v1, ref Vector2 v2)
		{
			return v1.Determinant(ref v2);
		}

		public static Vector2 Normalize(Vector2 v)
		{
			Vector2 result;
			v.Normalize(out result);
			return result;
		}

		public static void Normalize(ref Vector2 v, out Vector2 result)
		{
			v.Normalize(out result);
		}

		public static Vector2 Abs(Vector2 v)
		{
			Vector2 result;
			v.Abs(out result);
			return result;
		}

		public static void Abs(ref Vector2 v, out Vector2 result)
		{
			v.Abs(out result);
		}

		public static Vector2 Min(Vector2 v1, Vector2 v2)
		{
			Vector2 result;
			v1.Min(ref v2, out result);
			return result;
		}

		public static void Min(ref Vector2 v1, ref Vector2 v2, out Vector2 result)
		{
			v1.Min(ref v2, out result);
		}

		public static Vector2 Min(Vector2 v, float f)
		{
			Vector2 result;
			v.Min(f, out result);
			return result;
		}

		public static void Min(ref Vector2 v, float f, out Vector2 result)
		{
			v.Min(f, out result);
		}

		public static Vector2 Max(Vector2 v1, Vector2 v2)
		{
			Vector2 result;
			v1.Max(ref v2, out result);
			return result;
		}

		public static void Max(ref Vector2 v1, ref Vector2 v2, out Vector2 result)
		{
			v1.Max(ref v2, out result);
		}

		public static Vector2 Max(Vector2 v, float f)
		{
			Vector2 result;
			v.Max(f, out result);
			return result;
		}

		public static void Max(ref Vector2 v, float f, out Vector2 result)
		{
			v.Max(f, out result);
		}

		public static Vector2 Clamp(Vector2 v, Vector2 min, Vector2 max)
		{
			Vector2 result;
			v.Clamp(ref min, ref max, out result);
			return result;
		}

		public static void Clamp(ref Vector2 v, ref Vector2 min, ref Vector2 max, out Vector2 result)
		{
			v.Clamp(ref min, ref max, out result);
		}

		public static Vector2 Clamp(Vector2 v, float min, float max)
		{
			Vector2 result;
			v.Clamp(min, max, out result);
			return result;
		}

		public static void Clamp(ref Vector2 v, float min, float max, out Vector2 result)
		{
			v.Clamp(min, max, out result);
		}

		public static Vector2 Repeat(Vector2 v, Vector2 min, Vector2 max)
		{
			Vector2 result;
			v.Repeat(ref min, ref max, out result);
			return result;
		}

		public static void Repeat(ref Vector2 v, ref Vector2 min, ref Vector2 max, out Vector2 result)
		{
			v.Repeat(ref min, ref max, out result);
		}

		public static Vector2 Repeat(Vector2 v, float min, float max)
		{
			Vector2 result;
			v.Repeat(min, max, out result);
			return result;
		}

		public static void Repeat(ref Vector2 v, float min, float max, out Vector2 result)
		{
			v.Repeat(min, max, out result);
		}

		public static Vector2 Lerp(Vector2 v1, Vector2 v2, float f)
		{
			Vector2 result;
			v1.Lerp(ref v2, f, out result);
			return result;
		}

		public static void Lerp(ref Vector2 v1, ref Vector2 v2, float f, out Vector2 result)
		{
			v1.Lerp(ref v2, f, out result);
		}

		public static Vector2 Slerp(Vector2 v1, Vector2 v2, float f)
		{
			Vector2 result;
			v1.Slerp(ref v2, f, out result);
			return result;
		}

		public static void Slerp(ref Vector2 v1, ref Vector2 v2, float f, out Vector2 result)
		{
			v1.Slerp(ref v2, f, out result);
		}

		public static Vector2 MoveTo(Vector2 v1, Vector2 v2, float length)
		{
			Vector2 result;
			v1.MoveTo(ref v2, length, out result);
			return result;
		}

		public static void MoveTo(ref Vector2 v1, ref Vector2 v2, float length, out Vector2 result)
		{
			v1.MoveTo(ref v2, length, out result);
		}

		public static Vector2 TurnTo(Vector2 v1, Vector2 v2, float angle)
		{
			Vector2 result;
			v1.TurnTo(ref v2, angle, out result);
			return result;
		}

		public static void TurnTo(ref Vector2 v1, ref Vector2 v2, float angle, out Vector2 result)
		{
			v1.TurnTo(ref v2, angle, out result);
		}

		public static float Angle(Vector2 v1, Vector2 v2)
		{
			return v1.Angle(ref v2);
		}

		public static float Angle(ref Vector2 v1, ref Vector2 v2)
		{
			return v1.Angle(ref v2);
		}

		public static Vector2 Rotate(Vector2 v, float angle)
		{
			Vector2 result;
			v.Rotate(angle, out result);
			return result;
		}

		public static void Rotate(ref Vector2 v, float angle, out Vector2 result)
		{
			v.Rotate(angle, out result);
		}

		public static Vector2 Rotate(Vector2 v, Vector2 rotation)
		{
			Vector2 result;
			v.Rotate(ref rotation, out result);
			return result;
		}

		public static void Rotate(ref Vector2 v, ref Vector2 rotation, out Vector2 result)
		{
			v.Rotate(ref rotation, out result);
		}

		public static Vector2 Reflect(Vector2 v, Vector2 normal)
		{
			Vector2 result;
			v.Reflect(ref normal, out result);
			return result;
		}

		public static void Reflect(ref Vector2 v, ref Vector2 normal, out Vector2 result)
		{
			v.Reflect(ref normal, out result);
		}

		public static Vector2 Perpendicular(Vector2 v)
		{
			Vector2 result;
			v.Perpendicular(out result);
			return result;
		}

		public static void Perpendicular(ref Vector2 v, out Vector2 result)
		{
			v.Perpendicular(out result);
		}

		public static Vector2 ProjectOnLine(Vector2 v, Vector2 point, Vector2 direction)
		{
			Vector2 result;
			v.ProjectOnLine(ref point, ref direction, out result);
			return result;
		}

		public static void ProjectOnLine(ref Vector2 v, ref Vector2 point, ref Vector2 direction, out Vector2 result)
		{
			v.ProjectOnLine(ref point, ref direction, out result);
		}

		public static Vector2 Add(Vector2 v1, Vector2 v2)
		{
			Vector2 result;
			v1.Add(ref v2, out result);
			return result;
		}

		public static void Add(ref Vector2 v1, ref Vector2 v2, out Vector2 result)
		{
			v1.Add(ref v2, out result);
		}

		public static Vector2 Subtract(Vector2 v1, Vector2 v2)
		{
			Vector2 result;
			v1.Subtract(ref v2, out result);
			return result;
		}

		public static void Subtract(ref Vector2 v1, ref Vector2 v2, out Vector2 result)
		{
			v1.Subtract(ref v2, out result);
		}

		public static Vector2 Multiply(Vector2 v1, Vector2 v2)
		{
			Vector2 result;
			v1.Multiply(ref v2, out result);
			return result;
		}

		public static void Multiply(ref Vector2 v1, ref Vector2 v2, out Vector2 result)
		{
			v1.Multiply(ref v2, out result);
		}

		public static Vector2 Multiply(Vector2 v, float f)
		{
			Vector2 result;
			v.Multiply(f, out result);
			return result;
		}

		public static void Multiply(ref Vector2 v, float f, out Vector2 result)
		{
			v.Multiply(f, out result);
		}

		public static Vector2 Divide(Vector2 v1, Vector2 v2)
		{
			Vector2 result;
			v1.Divide(ref v2, out result);
			return result;
		}

		public static void Divide(ref Vector2 v1, ref Vector2 v2, out Vector2 result)
		{
			v1.Divide(ref v2, out result);
		}

		public static Vector2 Divide(Vector2 v, float f)
		{
			Vector2 result;
			v.Divide(f, out result);
			return result;
		}

		public static void Divide(ref Vector2 v, float f, out Vector2 result)
		{
			v.Divide(f, out result);
		}

		public static Vector2 Negate(Vector2 v)
		{
			Vector2 result;
			v.Negate(out result);
			return result;
		}

		public static void Negate(ref Vector2 v, out Vector2 result)
		{
			v.Negate(out result);
		}

		public static bool operator ==(Vector2 v1, Vector2 v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y;
		}

		public static bool operator !=(Vector2 v1, Vector2 v2)
		{
			return v1.X != v2.X || v1.Y != v2.Y;
		}

		public static Vector2 operator +(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static Vector2 operator +(Vector2 v, float f)
		{
			return new Vector2(v.X + f, v.Y + f);
		}

		public static Vector2 operator +(float f, Vector2 v)
		{
			return new Vector2(f + v.X, f + v.Y);
		}

		public static Vector2 operator -(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
		}

		public static Vector2 operator -(Vector2 v, float f)
		{
			return new Vector2(v.X - f, v.Y - f);
		}

		public static Vector2 operator -(float f, Vector2 v)
		{
			return new Vector2(f - v.X, f - v.Y);
		}

		public static Vector2 operator -(Vector2 v)
		{
			return new Vector2(-v.X, -v.Y);
		}

		public static Vector2 operator *(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
		}

		public static Vector2 operator *(Vector2 v, float f)
		{
			return new Vector2(v.X * f, v.Y * f);
		}

		public static Vector2 operator *(float f, Vector2 v)
		{
			return new Vector2(f * v.X, f * v.Y);
		}

		public static Vector2 operator /(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
		}

		public static Vector2 operator /(Vector2 v, float f)
		{
			float num = 1f / f;
			return new Vector2(v.X * num, v.Y * num);
		}

		public static Vector2 operator /(float f, Vector2 v)
		{
			return new Vector2(f / v.X, f / v.Y);
		}
	}
}
