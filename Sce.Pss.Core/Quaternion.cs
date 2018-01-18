using System;

namespace Sce.Pss.Core
{
	/// <summary>
	/// Description of Quaternion.
	/// </summary>
	public struct Quaternion
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public static readonly Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);
		
		public override string ToString()
		{
			return string.Format("({0:F6},{1:F6},{2:F6},{3:F6})", new object[]
			{
				this.X,
				this.Y,
				this.Z,
				this.W
			});
		}
		
		public Quaternion(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}
		
		public float Dot(Quaternion q)
		{
			return this.X * q.X + this.Y * q.Y + this.Z * q.Z + this.W * q.W;
		}
		
		public float Dot(ref Quaternion q)
		{
			return this.X * q.X + this.Y * q.Y + this.Z * q.Z + this.W * q.W;
		}
		
		public float Length()
		{
			return (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W));
		}

		public float LengthSquared()
		{
			return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
		}
		
		public Quaternion Normalize()
		{
			Quaternion result;
			this.Normalize(out result);
			return result;
		}

		public void Normalize(out Quaternion result)
		{
			float num = 1f / this.Length();
			result.X = this.X * num;
			result.Y = this.Y * num;
			result.Z = this.Z * num;
			result.W = this.W * num;
		}
		
		public Quaternion Conjugate()
		{
			Quaternion result;
			this.Conjugate(out result);
			return result;
		}
		

		public void Conjugate(out Quaternion result)
		{
			result.X = -this.X;
			result.Y = -this.Y;
			result.Z = -this.Z;
			result.W = this.W;
		}

		public Quaternion Inverse()
		{
			Quaternion result;
			this.Inverse(out result);
			return result;
		}

		public void Inverse(out Quaternion result)
		{
			float num = 1f / this.LengthSquared();
			result.X = -this.X * num;
			result.Y = -this.Y * num;
			result.Z = -this.Z * num;
			result.W = this.W * num;
		}
		
		public static Quaternion RotationX(float angle)
		{
			Quaternion result;
			Quaternion.RotationX(angle, out result);
			return result;
		}

		public static void RotationX(float angle, out Quaternion result)
		{
			angle *= 0.5f;
			result.X = (float)Math.Sin((double)angle);
			result.Y = 0f;
			result.Z = 0f;
			result.W = (float)Math.Cos((double)angle);
		}

		public static Quaternion RotationY(float angle)
		{
			Quaternion result;
			Quaternion.RotationY(angle, out result);
			return result;
		}

		public static void RotationY(float angle, out Quaternion result)
		{
			angle *= 0.5f;
			result.X = 0f;
			result.Y = (float)Math.Sin((double)angle);
			result.Z = 0f;
			result.W = (float)Math.Cos((double)angle);
		}

		public static Quaternion RotationZ(float angle)
		{
			Quaternion result;
			Quaternion.RotationZ(angle, out result);
			return result;
		}

		public static void RotationZ(float angle, out Quaternion result)
		{
			angle *= 0.5f;
			result.X = 0f;
			result.Y = 0f;
			result.Z = (float)Math.Sin((double)angle);
			result.W = (float)Math.Cos((double)angle);
		}

		public static Quaternion RotationZyx(float x, float y, float z)
		{
			Quaternion result;
			Quaternion.RotationZyx(x, y, z, out result);
			return result;
		}

		public static void RotationZyx(float x, float y, float z, out Quaternion result)
		{
			Vector3 vector;
			Vector3 vector2;
			Quaternion.RotationVector(x, y, z, out vector, out vector2);
			result.X = vector.Z * vector.Y * vector2.X - vector.X * vector2.Z * vector2.Y;
			result.Y = vector.X * vector.Z * vector2.Y + vector.Y * vector2.Z * vector2.X;
			result.Z = vector.X * vector.Y * vector2.Z - vector.Z * vector2.Y * vector2.X;
			result.W = vector.Z * vector.Y * vector.X + vector2.Z * vector2.Y * vector2.X;
		}

		public static Quaternion RotationZyx(Vector3 angles)
		{
			Quaternion result;
			Quaternion.RotationZyx(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationZyx(ref Vector3 angles, out Quaternion result)
		{
			Quaternion.RotationZyx(angles.X, angles.Y, angles.Z, out result);
		}

		public static Quaternion RotationYxz(float x, float y, float z)
		{
			Quaternion result;
			Quaternion.RotationYxz(x, y, z, out result);
			return result;
		}

		public static void RotationYxz(float x, float y, float z, out Quaternion result)
		{
			Vector3 vector;
			Vector3 vector2;
			Quaternion.RotationVector(x, y, z, out vector, out vector2);
			result.X = vector.Z * vector.Y * vector2.X + vector.X * vector2.Y * vector2.Z;
			result.Y = vector.Z * vector.X * vector2.Y - vector.Y * vector2.X * vector2.Z;
			result.Z = vector.Y * vector.X * vector2.Z - vector.Z * vector2.Y * vector2.X;
			result.W = vector.Y * vector.X * vector.Z + vector2.Y * vector2.X * vector2.Z;
		}

		public static Quaternion RotationYxz(Vector3 angles)
		{
			Quaternion result;
			Quaternion.RotationYxz(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationYxz(ref Vector3 angles, out Quaternion result)
		{
			Quaternion.RotationYxz(angles.X, angles.Y, angles.Z, out result);
		}

		public static Quaternion RotationXzy(float x, float y, float z)
		{
			Quaternion result;
			Quaternion.RotationXzy(x, y, z, out result);
			return result;
		}

		public static void RotationXzy(float x, float y, float z, out Quaternion result)
		{
			Vector3 vector;
			Vector3 vector2;
			Quaternion.RotationVector(x, y, z, out vector, out vector2);
			result.X = vector.Y * vector.Z * vector2.X - vector.X * vector2.Z * vector2.Y;
			result.Y = vector.X * vector.Z * vector2.Y - vector.Y * vector2.X * vector2.Z;
			result.Z = vector.Y * vector.X * vector2.Z + vector.Z * vector2.X * vector2.Y;
			result.W = vector.X * vector.Z * vector.Y + vector2.X * vector2.Z * vector2.Y;
		}

		public static Quaternion RotationXzy(Vector3 angles)
		{
			Quaternion result;
			Quaternion.RotationXzy(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationXzy(ref Vector3 angles, out Quaternion result)
		{
			Quaternion.RotationXzy(angles.X, angles.Y, angles.Z, out result);
		}

		public static Quaternion RotationXyz(float x, float y, float z)
		{
			Quaternion result;
			Quaternion.RotationXyz(x, y, z, out result);
			return result;
		}

		public static void RotationXyz(float x, float y, float z, out Quaternion result)
		{
			Vector3 vector;
			Vector3 vector2;
			Quaternion.RotationVector(x, y, z, out vector, out vector2);
			result.X = vector.Z * vector.Y * vector2.X + vector.X * vector2.Y * vector2.Z;
			result.Y = vector.Z * vector.X * vector2.Y - vector.Y * vector2.X * vector2.Z;
			result.Z = vector.X * vector.Y * vector2.Z + vector.Z * vector2.X * vector2.Y;
			result.W = vector.X * vector.Y * vector.Z - vector2.X * vector2.Y * vector2.Z;
		}

		public static Quaternion RotationXyz(Vector3 angles)
		{
			Quaternion result;
			Quaternion.RotationXyz(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationXyz(ref Vector3 angles, out Quaternion result)
		{
			Quaternion.RotationXyz(angles.X, angles.Y, angles.Z, out result);
		}

		public static Quaternion RotationYzx(float x, float y, float z)
		{
			Quaternion result;
			Quaternion.RotationYzx(x, y, z, out result);
			return result;
		}

		public static void RotationYzx(float x, float y, float z, out Quaternion result)
		{
			Vector3 vector;
			Vector3 vector2;
			Quaternion.RotationVector(x, y, z, out vector, out vector2);
			result.X = vector.Y * vector.Z * vector2.X + vector.X * vector2.Y * vector2.Z;
			result.Y = vector.X * vector.Z * vector2.Y + vector.Y * vector2.Z * vector2.X;
			result.Z = vector.X * vector.Y * vector2.Z - vector.Z * vector2.Y * vector2.X;
			result.W = vector.Y * vector.Z * vector.X - vector2.Y * vector2.Z * vector2.X;
		}

		public static Quaternion RotationYzx(Vector3 angles)
		{
			Quaternion result;
			Quaternion.RotationYzx(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationYzx(ref Vector3 angles, out Quaternion result)
		{
			Quaternion.RotationYzx(angles.X, angles.Y, angles.Z, out result);
		}

		public static Quaternion RotationZxy(float x, float y, float z)
		{
			Quaternion result;
			Quaternion.RotationZxy(x, y, z, out result);
			return result;
		}

		public static void RotationZxy(float x, float y, float z, out Quaternion result)
		{
			Vector3 vector;
			Vector3 vector2;
			Quaternion.RotationVector(x, y, z, out vector, out vector2);
			result.X = vector.Y * vector.Z * vector2.X - vector.X * vector2.Z * vector2.Y;
			result.Y = vector.Z * vector.X * vector2.Y + vector.Y * vector2.Z * vector2.X;
			result.Z = vector.Y * vector.X * vector2.Z + vector.Z * vector2.X * vector2.Y;
			result.W = vector.Z * vector.X * vector.Y - vector2.Z * vector2.X * vector2.Y;
		}

		public static Quaternion RotationZxy(Vector3 angles)
		{
			Quaternion result;
			Quaternion.RotationZxy(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationZxy(ref Vector3 angles, out Quaternion result)
		{
			Quaternion.RotationZxy(angles.X, angles.Y, angles.Z, out result);
		}
		
		public float Angle(Quaternion q)
		{
			return this.Angle(ref q);
		}

		public float Angle(ref Quaternion q)
		{
			float num = this.Dot(ref q);
			if (num < 0f)
			{
				num = -num;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return (float)Math.Acos((double)num) * 2f;
		}
		
		
		public Quaternion Slerp(Quaternion q, float f)
		{
			Quaternion result;
			this.Slerp(ref q, f, out result);
			return result;
		}

		public void Slerp(ref Quaternion q, float f, out Quaternion result)
		{
			if (f <= 0f)
			{
				result = this;
			}
			else if (f >= 1f)
			{
				result = q;
			}
			else
			{
				float num = this.X * q.X + this.Y * q.Y + this.Z * q.Z + this.W * q.W;
				float num2 = 1f - f;
				float num3 = f;
				if (num < 0.998047f)
				{
					if (num < 0f)
					{
						num = -num;
						num3 = -num3;
						if (num > 1f)
						{
							num = 1f;
						}
					}
					float num4 = (float)Math.Acos((double)num);
					float num5 = (float)Math.Sin((double)num4);
					if (num5 > 5E-05f)
					{
						num2 = (float)Math.Sin((double)(num2 * num4)) / num5;
						num3 = (float)Math.Sin((double)(num3 * num4)) / num5;
					}
				}
				result.X = this.X * num2 + q.X * num3;
				result.Y = this.Y * num2 + q.Y * num3;
				result.Z = this.Z * num2 + q.Z * num3;
				result.W = this.W * num2 + q.W * num3;
			}
		}
		
		private static void RotationVector(float x, float y, float z, out Vector3 c, out Vector3 s)
		{
			x *= 0.5f;
			y *= 0.5f;
			z *= 0.5f;
			c.X = (float)Math.Cos((double)x);
			c.Y = (float)Math.Cos((double)y);
			c.Z = (float)Math.Cos((double)z);
			s.X = (float)Math.Sin((double)x);
			s.Y = (float)Math.Sin((double)y);
			s.Z = (float)Math.Sin((double)z);
		}
		
		public static Quaternion FromMatrix4(Matrix4 m)
		{
			Quaternion result;
			Quaternion.FromMatrix4(ref m, out result);
			return result;
		}

		public static void FromMatrix4(ref Matrix4 m, out Quaternion result)
		{
			float num = m.M11 + m.M22 + m.M33;
			if (num > 0f)
			{
				float num2 = (float)Math.Sqrt((double)(num + 1f)) * 0.5f;
				float num3 = 0.25f / num2;
				result.X = (m.M23 - m.M32) * num3;
				result.Y = (m.M31 - m.M13) * num3;
				result.Z = (m.M12 - m.M21) * num3;
				result.W = num2;
			}
			else if (m.M11 > m.M22 && m.M11 > m.M33)
			{
				float num4 = (float)Math.Sqrt((double)(m.M11 - m.M22 - m.M33 + 1f)) * 0.5f;
				float num3 = 0.25f / num4;
				result.X = num4;
				result.Y = (m.M21 + m.M12) * num3;
				result.Z = (m.M31 + m.M13) * num3;
				result.W = (m.M23 - m.M32) * num3;
			}
			else if (m.M22 > m.M33)
			{
				float num5 = (float)Math.Sqrt((double)(m.M22 - m.M33 - m.M11 + 1f)) * 0.5f;
				float num3 = 0.25f / num5;
				result.X = (m.M21 + m.M12) * num3;
				result.Y = num5;
				result.Z = (m.M32 + m.M23) * num3;
				result.W = (m.M31 - m.M13) * num3;
			}
			else
			{
				float num6 = (float)Math.Sqrt((double)(m.M33 - m.M11 - m.M22 + 1f)) * 0.5f;
				float num3 = 0.25f / num6;
				result.X = (m.M31 + m.M13) * num3;
				result.Y = (m.M32 + m.M23) * num3;
				result.Z = num6;
				result.W = (m.M12 - m.M21) * num3;
			}
		}
		
		public Matrix4 ToMatrix4()
		{
			Matrix4 result;
			this.ToMatrix4(out result);
			return result;
		}

		public void ToMatrix4(out Matrix4 result)
		{
			float num = this.X + this.X;
			float num2 = this.Y + this.Y;
			float num3 = this.Z + this.Z;
			float num4 = this.X * num;
			float num5 = this.Y * num2;
			float num6 = this.Z * num3;
			float num7 = this.Z * num;
			float num8 = this.X * num2;
			float num9 = this.Y * num3;
			float num10 = this.W * num;
			float num11 = this.W * num2;
			float num12 = this.W * num3;
			result.M11 = 1f - num5 - num6;
			result.M12 = num8 + num12;
			result.M13 = num7 - num11;
			result.M21 = num8 - num12;
			result.M22 = 1f - num4 - num6;
			result.M23 = num9 + num10;
			result.M31 = num7 + num11;
			result.M32 = num9 - num10;
			result.M33 = 1f - num4 - num5;
			result.M14 = (result.M24 = (result.M34 = 0f));
			result.M41 = (result.M42 = (result.M43 = 0f));
			result.M44 = 1f;
		}
		
		
		public static Quaternion operator *(Quaternion q1, Quaternion q2)
		{
			Quaternion result;
			q1.Multiply(ref q2, out result);
			return result;
		}
		
		public Quaternion Multiply(Quaternion q)
		{
			Quaternion result;
			this.Multiply(ref q, out result);
			return result;
		}

		public void Multiply(ref Quaternion q, out Quaternion result)
		{
			result.X = this.W * q.X + this.X * q.W + this.Y * q.Z - this.Z * q.Y;
			result.Y = this.W * q.Y - this.X * q.Z + this.Y * q.W + this.Z * q.X;
			result.Z = this.W * q.Z + this.X * q.Y - this.Y * q.X + this.Z * q.W;
			result.W = this.W * q.W - this.X * q.X - this.Y * q.Y - this.Z * q.Z;
		}

		public Quaternion Multiply(float f)
		{
			Quaternion result;
			this.Multiply(f, out result);
			return result;
		}

		public void Multiply(float f, out Quaternion result)
		{
			result.X = this.X * f;
			result.Y = this.Y * f;
			result.Z = this.Z * f;
			result.W = this.W * f;
		}
		
		
		public static Quaternion RotationAxis(Vector3 axis, float angle)
		{
			Quaternion result;
			Quaternion.RotationAxis(ref axis, angle, out result);
			return result;
		}

		public static void RotationAxis(ref Vector3 axis, float angle, out Quaternion result)
		{
			angle *= 0.5f;
			float num = (float)Math.Sin((double)angle) / axis.Length();
			result.X = axis.X * num;
			result.Y = axis.Y * num;
			result.Z = axis.Z * num;
			result.W = (float)Math.Cos((double)angle);
		}
		
	}
}
