using System;

namespace Sce.Pss.Core
{
	public struct Matrix4 : IEquatable<Matrix4>
	{
		public float M11;

		public float M12;

		public float M13;

		public float M14;

		public float M21;

		public float M22;

		public float M23;

		public float M24;

		public float M31;

		public float M32;

		public float M33;

		public float M34;

		public float M41;

		public float M42;

		public float M43;

		public float M44;

		public static readonly Matrix4 Zero = new Matrix4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);

		public static readonly Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

		public Vector4 ColumnX
		{
			get
			{
				return new Vector4(this.M11, this.M12, this.M13, this.M14);
			}
			set
			{
				this.M11 = value.X;
				this.M12 = value.Y;
				this.M13 = value.Z;
				this.M14 = value.W;
			}
		}

		public Vector4 ColumnY
		{
			get
			{
				return new Vector4(this.M21, this.M22, this.M23, this.M24);
			}
			set
			{
				this.M21 = value.X;
				this.M22 = value.Y;
				this.M23 = value.Z;
				this.M24 = value.W;
			}
		}

		public Vector4 ColumnZ
		{
			get
			{
				return new Vector4(this.M31, this.M32, this.M33, this.M34);
			}
			set
			{
				this.M31 = value.X;
				this.M32 = value.Y;
				this.M33 = value.Z;
				this.M34 = value.W;
			}
		}

		public Vector4 ColumnW
		{
			get
			{
				return new Vector4(this.M41, this.M42, this.M43, this.M44);
			}
			set
			{
				this.M41 = value.X;
				this.M42 = value.Y;
				this.M43 = value.Z;
				this.M44 = value.W;
			}
		}

		public Vector4 RowX
		{
			get
			{
				return new Vector4(this.M11, this.M21, this.M31, this.M41);
			}
			set
			{
				this.M11 = value.X;
				this.M21 = value.Y;
				this.M31 = value.Z;
				this.M41 = value.W;
			}
		}

		public Vector4 RowY
		{
			get
			{
				return new Vector4(this.M12, this.M22, this.M32, this.M42);
			}
			set
			{
				this.M12 = value.X;
				this.M22 = value.Y;
				this.M32 = value.Z;
				this.M42 = value.W;
			}
		}

		public Vector4 RowZ
		{
			get
			{
				return new Vector4(this.M13, this.M23, this.M33, this.M43);
			}
			set
			{
				this.M13 = value.X;
				this.M23 = value.Y;
				this.M33 = value.Z;
				this.M43 = value.W;
			}
		}

		public Vector4 RowW
		{
			get
			{
				return new Vector4(this.M14, this.M24, this.M34, this.M44);
			}
			set
			{
				this.M14 = value.X;
				this.M24 = value.Y;
				this.M34 = value.Z;
				this.M44 = value.W;
			}
		}

		public Vector3 AxisX
		{
			get
			{
				return new Vector3(this.M11, this.M12, this.M13);
			}
			set
			{
				this.M11 = value.X;
				this.M12 = value.Y;
				this.M13 = value.Z;
			}
		}

		public Vector3 AxisY
		{
			get
			{
				return new Vector3(this.M21, this.M22, this.M23);
			}
			set
			{
				this.M21 = value.X;
				this.M22 = value.Y;
				this.M23 = value.Z;
			}
		}

		public Vector3 AxisZ
		{
			get
			{
				return new Vector3(this.M31, this.M32, this.M33);
			}
			set
			{
				this.M31 = value.X;
				this.M32 = value.Y;
				this.M33 = value.Z;
			}
		}

		public Vector3 AxisW
		{
			get
			{
				return new Vector3(this.M41, this.M42, this.M43);
			}
			set
			{
				this.M41 = value.X;
				this.M42 = value.Y;
				this.M43 = value.Z;
			}
		}

		public Matrix4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M13 = m13;
			this.M14 = m14;
			this.M21 = m21;
			this.M22 = m22;
			this.M23 = m23;
			this.M24 = m24;
			this.M31 = m31;
			this.M32 = m32;
			this.M33 = m33;
			this.M34 = m34;
			this.M41 = m41;
			this.M42 = m42;
			this.M43 = m43;
			this.M44 = m44;
		}

		public Matrix4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
		{
			this.M11 = x.X;
			this.M12 = x.Y;
			this.M13 = x.Z;
			this.M14 = x.W;
			this.M21 = y.X;
			this.M22 = y.Y;
			this.M23 = y.Z;
			this.M24 = y.W;
			this.M31 = z.X;
			this.M32 = z.Y;
			this.M33 = z.Z;
			this.M34 = z.W;
			this.M41 = w.X;
			this.M42 = w.Y;
			this.M43 = w.Z;
			this.M44 = w.W;
		}

		public Matrix4(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
		{
			this.M11 = x.X;
			this.M12 = x.Y;
			this.M13 = x.Z;
			this.M14 = 0f;
			this.M21 = y.X;
			this.M22 = y.Y;
			this.M23 = y.Z;
			this.M24 = 0f;
			this.M31 = z.X;
			this.M32 = z.Y;
			this.M33 = z.Z;
			this.M34 = 0f;
			this.M41 = w.X;
			this.M42 = w.Y;
			this.M43 = w.Z;
			this.M44 = 1f;
		}

		public float Determinant()
		{
			float result;
			if (this.M14 == 0f && this.M24 == 0f && this.M34 == 0f && this.M44 == 1f)
			{
				float num = this.M22 * this.M33 - this.M23 * this.M32;
				float num2 = this.M13 * this.M32 - this.M12 * this.M33;
				float num3 = this.M12 * this.M23 - this.M13 * this.M22;
				result = this.M11 * num + this.M21 * num2 + this.M31 * num3;
			}
			else
			{
				float num = this.M11 * this.M22 - this.M12 * this.M21;
				float num2 = this.M11 * this.M23 - this.M13 * this.M21;
				float num3 = this.M11 * this.M24 - this.M14 * this.M21;
				float num4 = this.M12 * this.M23 - this.M13 * this.M22;
				float num5 = this.M12 * this.M24 - this.M14 * this.M22;
				float num6 = this.M13 * this.M24 - this.M14 * this.M23;
				float num7 = this.M31 * this.M42 - this.M32 * this.M41;
				float num8 = this.M31 * this.M43 - this.M33 * this.M41;
				float num9 = this.M31 * this.M44 - this.M34 * this.M41;
				float num10 = this.M32 * this.M43 - this.M33 * this.M42;
				float num11 = this.M32 * this.M44 - this.M34 * this.M42;
				float num12 = this.M33 * this.M44 - this.M34 * this.M43;
				result = num * num12 - num2 * num11 + num3 * num10 + num4 * num9 - num5 * num8 + num6 * num7;
			}
			return result;
		}

		public Matrix4 Transpose()
		{
			Matrix4 result;
			this.Transpose(out result);
			return result;
		}

		public void Transpose(out Matrix4 result)
		{
			result.M11 = this.M11;
			result.M12 = this.M21;
			result.M13 = this.M31;
			result.M14 = this.M41;
			result.M21 = this.M12;
			result.M22 = this.M22;
			result.M23 = this.M32;
			result.M24 = this.M42;
			result.M31 = this.M13;
			result.M32 = this.M23;
			result.M33 = this.M33;
			result.M34 = this.M43;
			result.M41 = this.M14;
			result.M42 = this.M24;
			result.M43 = this.M34;
			result.M44 = this.M44;
		}

		public Matrix4 Inverse()
		{
			Matrix4 result;
			this.Inverse(out result);
			return result;
		}

		public void Inverse(out Matrix4 result)
		{
			if (this.M14 == 0f && this.M24 == 0f && this.M34 == 0f && this.M44 == 1f)
			{
				this.InverseAffine(out result);
			}
			else
			{
				float num = this.M11 * this.M22 - this.M12 * this.M21;
				float num2 = this.M11 * this.M23 - this.M13 * this.M21;
				float num3 = this.M11 * this.M24 - this.M14 * this.M21;
				float num4 = this.M12 * this.M23 - this.M13 * this.M22;
				float num5 = this.M12 * this.M24 - this.M14 * this.M22;
				float num6 = this.M13 * this.M24 - this.M14 * this.M23;
				float num7 = this.M31 * this.M42 - this.M32 * this.M41;
				float num8 = this.M31 * this.M43 - this.M33 * this.M41;
				float num9 = this.M31 * this.M44 - this.M34 * this.M41;
				float num10 = this.M32 * this.M43 - this.M33 * this.M42;
				float num11 = this.M32 * this.M44 - this.M34 * this.M42;
				float num12 = this.M33 * this.M44 - this.M34 * this.M43;
				float num13 = 1f / (num * num12 - num2 * num11 + num3 * num10 + num4 * num9 - num5 * num8 + num6 * num7);
				result.M11 = (this.M22 * num12 - this.M23 * num11 + this.M24 * num10) * num13;
				result.M12 = (-this.M12 * num12 + this.M13 * num11 - this.M14 * num10) * num13;
				result.M13 = (this.M42 * num6 - this.M43 * num5 + this.M44 * num4) * num13;
				result.M14 = (-this.M32 * num6 + this.M33 * num5 - this.M34 * num4) * num13;
				result.M21 = (-this.M21 * num12 + this.M23 * num9 - this.M24 * num8) * num13;
				result.M22 = (this.M11 * num12 - this.M13 * num9 + this.M14 * num8) * num13;
				result.M23 = (-this.M41 * num6 + this.M43 * num3 - this.M44 * num2) * num13;
				result.M24 = (this.M31 * num6 - this.M33 * num3 + this.M34 * num2) * num13;
				result.M31 = (this.M21 * num11 - this.M22 * num9 + this.M24 * num7) * num13;
				result.M32 = (-this.M11 * num11 + this.M12 * num9 - this.M14 * num7) * num13;
				result.M33 = (this.M41 * num5 - this.M42 * num3 + this.M44 * num) * num13;
				result.M34 = (-this.M31 * num5 + this.M32 * num3 - this.M34 * num) * num13;
				result.M41 = (-this.M21 * num10 + this.M22 * num8 - this.M23 * num7) * num13;
				result.M42 = (this.M11 * num10 - this.M12 * num8 + this.M13 * num7) * num13;
				result.M43 = (-this.M41 * num4 + this.M42 * num2 - this.M43 * num) * num13;
				result.M44 = (this.M31 * num4 - this.M32 * num2 + this.M33 * num) * num13;
			}
		}

		public Matrix4 InverseAffine()
		{
			Matrix4 result;
			this.InverseAffine(out result);
			return result;
		}

		public void InverseAffine(out Matrix4 result)
		{
			float num = this.M22 * this.M33 - this.M23 * this.M32;
			float num2 = this.M13 * this.M32 - this.M12 * this.M33;
			float num3 = this.M12 * this.M23 - this.M13 * this.M22;
			float num4 = 1f / (this.M11 * num + this.M21 * num2 + this.M31 * num3);
			result.M11 = num * num4;
			result.M12 = num2 * num4;
			result.M13 = num3 * num4;
			result.M14 = 0f;
			result.M21 = (this.M23 * this.M31 - this.M21 * this.M33) * num4;
			result.M22 = (this.M11 * this.M33 - this.M13 * this.M31) * num4;
			result.M23 = (this.M13 * this.M21 - this.M11 * this.M23) * num4;
			result.M24 = 0f;
			result.M31 = (this.M21 * this.M32 - this.M22 * this.M31) * num4;
			result.M32 = (this.M12 * this.M31 - this.M11 * this.M32) * num4;
			result.M33 = (this.M11 * this.M22 - this.M12 * this.M21) * num4;
			result.M34 = 0f;
			result.M41 = -(result.M11 * this.M41 + result.M21 * this.M42 + result.M31 * this.M43);
			result.M42 = -(result.M12 * this.M41 + result.M22 * this.M42 + result.M32 * this.M43);
			result.M43 = -(result.M13 * this.M41 + result.M23 * this.M42 + result.M33 * this.M43);
			result.M44 = 1f;
		}

		public Matrix4 InverseOrthonormal()
		{
			Matrix4 result;
			this.InverseOrthonormal(out result);
			return result;
		}

		public void InverseOrthonormal(out Matrix4 result)
		{
			result.M11 = this.M11;
			result.M12 = this.M21;
			result.M13 = this.M31;
			result.M14 = 0f;
			result.M21 = this.M12;
			result.M22 = this.M22;
			result.M23 = this.M32;
			result.M24 = 0f;
			result.M31 = this.M13;
			result.M32 = this.M23;
			result.M33 = this.M33;
			result.M34 = 0f;
			result.M41 = -(this.M11 * this.M41 + this.M12 * this.M42 + this.M13 * this.M43);
			result.M42 = -(this.M21 * this.M41 + this.M22 * this.M42 + this.M23 * this.M43);
			result.M43 = -(this.M31 * this.M41 + this.M32 * this.M42 + this.M33 * this.M43);
			result.M44 = 1f;
		}

		public Matrix4 Orthonormalize()
		{
			Matrix4 result;
			this.Orthonormalize(out result);
			return result;
		}

		public void Orthonormalize(out Matrix4 result)
		{
			Vector3 vector;
			this.AxisZ.Normalize(out vector);
			Vector3 vector2;
			this.AxisY.Cross(ref vector, out vector2);
			vector2.Normalize(out vector2);
			Vector3 vector3;
			vector.Cross(ref vector2, out vector3);
			result.M11 = vector2.X;
			result.M12 = vector2.Y;
			result.M13 = vector2.Z;
			result.M14 = 0f;
			result.M21 = vector3.X;
			result.M22 = vector3.Y;
			result.M23 = vector3.Z;
			result.M24 = 0f;
			result.M31 = vector.X;
			result.M32 = vector.Y;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = this.M41;
			result.M42 = this.M42;
			result.M43 = this.M43;
			result.M44 = 1f;
		}

		public Vector4 Transform(Vector4 v)
		{
			Vector4 result;
			this.Transform(ref v, out result);
			return result;
		}

		public void Transform(ref Vector4 v, out Vector4 result)
		{
			result.X = this.M11 * v.X + this.M21 * v.Y + this.M31 * v.Z + this.M41 * v.W;
			result.Y = this.M12 * v.X + this.M22 * v.Y + this.M32 * v.Z + this.M42 * v.W;
			result.Z = this.M13 * v.X + this.M23 * v.Y + this.M33 * v.Z + this.M43 * v.W;
			result.W = this.M14 * v.X + this.M24 * v.Y + this.M34 * v.Z + this.M44 * v.W;
		}

		public Vector3 TransformPoint(Vector3 v)
		{
			Vector3 result;
			this.TransformPoint(ref v, out result);
			return result;
		}

		public void TransformPoint(ref Vector3 v, out Vector3 result)
		{
			result.X = this.M11 * v.X + this.M21 * v.Y + this.M31 * v.Z + this.M41;
			result.Y = this.M12 * v.X + this.M22 * v.Y + this.M32 * v.Z + this.M42;
			result.Z = this.M13 * v.X + this.M23 * v.Y + this.M33 * v.Z + this.M43;
		}

		public Vector2 TransformPoint(Vector2 v)
		{
			Vector2 result;
			this.TransformPoint(ref v, out result);
			return result;
		}

		public void TransformPoint(ref Vector2 v, out Vector2 result)
		{
			result.X = this.M11 * v.X + this.M21 * v.Y + this.M41;
			result.Y = this.M12 * v.X + this.M22 * v.Y + this.M42;
		}

		public Vector3 TransformVector(Vector3 v)
		{
			Vector3 result;
			this.TransformVector(ref v, out result);
			return result;
		}

		public void TransformVector(ref Vector3 v, out Vector3 result)
		{
			result.X = this.M11 * v.X + this.M21 * v.Y + this.M31 * v.Z;
			result.Y = this.M12 * v.X + this.M22 * v.Y + this.M32 * v.Z;
			result.Z = this.M13 * v.X + this.M23 * v.Y + this.M33 * v.Z;
		}

		public Vector2 TransformVector(Vector2 v)
		{
			Vector2 result;
			this.TransformVector(ref v, out result);
			return result;
		}

		public void TransformVector(ref Vector2 v, out Vector2 result)
		{
			result.X = this.M11 * v.X + this.M21 * v.Y;
			result.Y = this.M12 * v.X + this.M22 * v.Y;
		}

		public Vector4 TransformProjection(Vector4 v)
		{
			Vector4 result;
			this.TransformProjection(ref v, out result);
			return result;
		}

		public void TransformProjection(ref Vector4 v, out Vector4 result)
		{
			result.W = this.M14 * v.X + this.M24 * v.Y + this.M34 * v.Z + this.M44 * v.W;
			result.X = (this.M11 * v.X + this.M21 * v.Y + this.M31 * v.Z + this.M41 * v.W) / result.W;
			result.Y = (this.M12 * v.X + this.M22 * v.Y + this.M32 * v.Z + this.M42 * v.W) / result.W;
			result.Z = (this.M13 * v.X + this.M23 * v.Y + this.M33 * v.Z + this.M43 * v.W) / result.W;
		}

		public Vector3 TransformProjection(Vector3 v)
		{
			Vector3 result;
			this.TransformProjection(ref v, out result);
			return result;
		}

		public void TransformProjection(ref Vector3 v, out Vector3 result)
		{
			float num = this.M14 * v.X + this.M24 * v.Y + this.M34 * v.Z + this.M44;
			result.X = (this.M11 * v.X + this.M21 * v.Y + this.M31 * v.Z + this.M41) / num;
			result.Y = (this.M12 * v.X + this.M22 * v.Y + this.M32 * v.Z + this.M42) / num;
			result.Z = (this.M13 * v.X + this.M23 * v.Y + this.M33 * v.Z + this.M43) / num;
		}

		public Vector2 TransformProjection(Vector2 v)
		{
			Vector2 result;
			this.TransformProjection(ref v, out result);
			return result;
		}

		public void TransformProjection(ref Vector2 v, out Vector2 result)
		{
			float num = this.M14 * v.X + this.M24 * v.Y + this.M44;
			result.X = (this.M11 * v.X + this.M21 * v.Y + this.M41) / num;
			result.Y = (this.M12 * v.X + this.M22 * v.Y + this.M42) / num;
		}

		public Matrix4 Add(Matrix4 m)
		{
			Matrix4 result;
			this.Add(ref m, out result);
			return result;
		}

		public void Add(ref Matrix4 m, out Matrix4 result)
		{
			result.M11 = this.M11 + m.M11;
			result.M12 = this.M12 + m.M12;
			result.M13 = this.M13 + m.M13;
			result.M14 = this.M14 + m.M14;
			result.M21 = this.M21 + m.M21;
			result.M22 = this.M22 + m.M22;
			result.M23 = this.M23 + m.M23;
			result.M24 = this.M24 + m.M24;
			result.M31 = this.M31 + m.M31;
			result.M32 = this.M32 + m.M32;
			result.M33 = this.M33 + m.M33;
			result.M34 = this.M34 + m.M34;
			result.M41 = this.M41 + m.M41;
			result.M42 = this.M42 + m.M42;
			result.M43 = this.M43 + m.M43;
			result.M44 = this.M44 + m.M44;
		}

		public Matrix4 Subtract(Matrix4 m)
		{
			Matrix4 result;
			this.Subtract(ref m, out result);
			return result;
		}

		public void Subtract(ref Matrix4 m, out Matrix4 result)
		{
			result.M11 = this.M11 - m.M11;
			result.M12 = this.M12 - m.M12;
			result.M13 = this.M13 - m.M13;
			result.M14 = this.M14 - m.M14;
			result.M21 = this.M21 - m.M21;
			result.M22 = this.M22 - m.M22;
			result.M23 = this.M23 - m.M23;
			result.M24 = this.M24 - m.M24;
			result.M31 = this.M31 - m.M31;
			result.M32 = this.M32 - m.M32;
			result.M33 = this.M33 - m.M33;
			result.M34 = this.M34 - m.M34;
			result.M41 = this.M41 - m.M41;
			result.M42 = this.M42 - m.M42;
			result.M43 = this.M43 - m.M43;
			result.M44 = this.M44 - m.M44;
		}

		public Matrix4 Multiply(Matrix4 m)
		{
			Matrix4 result;
			this.Multiply(ref m, out result);
			return result;
		}

		public void Multiply(ref Matrix4 m, out Matrix4 result)
		{
			result.M11 = this.M11 * m.M11 + this.M21 * m.M12 + this.M31 * m.M13 + this.M41 * m.M14;
			result.M12 = this.M12 * m.M11 + this.M22 * m.M12 + this.M32 * m.M13 + this.M42 * m.M14;
			result.M13 = this.M13 * m.M11 + this.M23 * m.M12 + this.M33 * m.M13 + this.M43 * m.M14;
			result.M14 = this.M14 * m.M11 + this.M24 * m.M12 + this.M34 * m.M13 + this.M44 * m.M14;
			result.M21 = this.M11 * m.M21 + this.M21 * m.M22 + this.M31 * m.M23 + this.M41 * m.M24;
			result.M22 = this.M12 * m.M21 + this.M22 * m.M22 + this.M32 * m.M23 + this.M42 * m.M24;
			result.M23 = this.M13 * m.M21 + this.M23 * m.M22 + this.M33 * m.M23 + this.M43 * m.M24;
			result.M24 = this.M14 * m.M21 + this.M24 * m.M22 + this.M34 * m.M23 + this.M44 * m.M24;
			result.M31 = this.M11 * m.M31 + this.M21 * m.M32 + this.M31 * m.M33 + this.M41 * m.M34;
			result.M32 = this.M12 * m.M31 + this.M22 * m.M32 + this.M32 * m.M33 + this.M42 * m.M34;
			result.M33 = this.M13 * m.M31 + this.M23 * m.M32 + this.M33 * m.M33 + this.M43 * m.M34;
			result.M34 = this.M14 * m.M31 + this.M24 * m.M32 + this.M34 * m.M33 + this.M44 * m.M34;
			result.M41 = this.M11 * m.M41 + this.M21 * m.M42 + this.M31 * m.M43 + this.M41 * m.M44;
			result.M42 = this.M12 * m.M41 + this.M22 * m.M42 + this.M32 * m.M43 + this.M42 * m.M44;
			result.M43 = this.M13 * m.M41 + this.M23 * m.M42 + this.M33 * m.M43 + this.M43 * m.M44;
			result.M44 = this.M14 * m.M41 + this.M24 * m.M42 + this.M34 * m.M43 + this.M44 * m.M44;
		}

		public Matrix4 MultiplyAffine(Matrix4 m)
		{
			Matrix4 result;
			this.MultiplyAffine(ref m, out result);
			return result;
		}

		public void MultiplyAffine(ref Matrix4 m, out Matrix4 result)
		{
			result.M11 = this.M11 * m.M11 + this.M21 * m.M12 + this.M31 * m.M13;
			result.M12 = this.M12 * m.M11 + this.M22 * m.M12 + this.M32 * m.M13;
			result.M13 = this.M13 * m.M11 + this.M23 * m.M12 + this.M33 * m.M13;
			result.M14 = 0f;
			result.M21 = this.M11 * m.M21 + this.M21 * m.M22 + this.M31 * m.M23;
			result.M22 = this.M12 * m.M21 + this.M22 * m.M22 + this.M32 * m.M23;
			result.M23 = this.M13 * m.M21 + this.M23 * m.M22 + this.M33 * m.M23;
			result.M24 = 0f;
			result.M31 = this.M11 * m.M31 + this.M21 * m.M32 + this.M31 * m.M33;
			result.M32 = this.M12 * m.M31 + this.M22 * m.M32 + this.M32 * m.M33;
			result.M33 = this.M13 * m.M31 + this.M23 * m.M32 + this.M33 * m.M33;
			result.M34 = 0f;
			result.M41 = this.M11 * m.M41 + this.M21 * m.M42 + this.M31 * m.M43 + this.M41;
			result.M42 = this.M12 * m.M41 + this.M22 * m.M42 + this.M32 * m.M43 + this.M42;
			result.M43 = this.M13 * m.M41 + this.M23 * m.M42 + this.M33 * m.M43 + this.M43;
			result.M44 = 1f;
		}

		public Matrix4 Multiply(float f)
		{
			Matrix4 result;
			this.Multiply(f, out result);
			return result;
		}

		public void Multiply(float f, out Matrix4 result)
		{
			result.M11 = this.M11 * f;
			result.M12 = this.M12 * f;
			result.M13 = this.M13 * f;
			result.M14 = this.M14 * f;
			result.M21 = this.M21 * f;
			result.M22 = this.M22 * f;
			result.M23 = this.M23 * f;
			result.M24 = this.M24 * f;
			result.M31 = this.M31 * f;
			result.M32 = this.M32 * f;
			result.M33 = this.M33 * f;
			result.M34 = this.M34 * f;
			result.M41 = this.M41 * f;
			result.M42 = this.M42 * f;
			result.M43 = this.M43 * f;
			result.M44 = this.M44 * f;
		}

		public Matrix4 Divide(float f)
		{
			Matrix4 result;
			this.Divide(f, out result);
			return result;
		}

		public void Divide(float f, out Matrix4 result)
		{
			this.Multiply(1f / f, out result);
		}

		public Matrix4 Negate()
		{
			Matrix4 result;
			this.Negate(out result);
			return result;
		}

		public void Negate(out Matrix4 result)
		{
			this.Multiply(-1f, out result);
		}

		public bool IsOrthonormal(float epsilon)
		{
			return !this.ColumnX.IsNaN() && !this.ColumnY.IsNaN() && !this.ColumnZ.IsNaN() && !this.ColumnW.IsNaN() && this.AxisX.Cross(this.AxisY).Dot(this.AxisZ) > 0f && this.ColumnX.IsUnit(epsilon) && this.ColumnY.IsUnit(epsilon) && this.ColumnZ.IsUnit(epsilon) && Math.Abs(this.ColumnX.Dot(this.ColumnY)) < epsilon && Math.Abs(this.ColumnY.Dot(this.ColumnZ)) < epsilon && Math.Abs(this.ColumnZ.Dot(this.ColumnX)) < epsilon && this.M44 == 1f && (this.M44 == 1f && this.M14 == 0f && this.M24 == 0f) && this.M34 == 0f;
		}

		public bool IsIdentity()
		{
			return this.Equals(Matrix4.Identity);
		}

		public bool IsInfinity()
		{
			return this.ColumnX.IsInfinity() || this.ColumnY.IsInfinity() || this.ColumnZ.IsInfinity() || this.ColumnW.IsInfinity();
		}

		public bool IsNaN()
		{
			return this.ColumnX.IsNaN() || this.ColumnY.IsNaN() || this.ColumnZ.IsNaN() || this.ColumnW.IsNaN();
		}

		public bool Equals(Matrix4 m, float epsilon)
		{
			return this.ColumnX.Equals(m.ColumnX, epsilon) && this.ColumnY.Equals(m.ColumnY, epsilon) && this.ColumnZ.Equals(m.ColumnZ, epsilon) && this.ColumnW.Equals(m.ColumnW, epsilon);
		}

		public bool Equals(Matrix4 m)
		{
			return this.ColumnX == m.ColumnX && this.ColumnY == m.ColumnY && this.ColumnZ == m.ColumnZ && this.ColumnW == m.ColumnW;
		}

		public override bool Equals(object o)
		{
			return o is Matrix4 && this.Equals((Matrix4)o);
		}

		public override string ToString()
		{
			return string.Format("({0},{1},{2},{3})", new object[]
			{
				this.ColumnX,
				this.ColumnY,
				this.ColumnZ,
				this.ColumnW
			});
		}

		public override int GetHashCode()
		{
			return this.ColumnX.GetHashCode() ^ this.ColumnY.GetHashCode() ^ this.ColumnZ.GetHashCode() ^ this.ColumnW.GetHashCode();
		}

		public static Matrix4 Transformation(Vector3 translation, Quaternion rotation, Vector3 scale)
		{
			Matrix4 result;
			Matrix4.Transformation(ref translation, ref rotation, ref scale, out result);
			return result;
		}

		public static void Transformation(ref Vector3 translation, ref Quaternion rotation, ref Vector3 scale, out Matrix4 result)
		{
			rotation.ToMatrix4(out result);
			result.M11 *= scale.X;
			result.M12 *= scale.X;
			result.M13 *= scale.X;
			result.M21 *= scale.Y;
			result.M22 *= scale.Y;
			result.M23 *= scale.Y;
			result.M31 *= scale.Z;
			result.M32 *= scale.Z;
			result.M33 *= scale.Z;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
		}

		public static Matrix4 Transformation(Vector3 translation, Vector3 scale)
		{
			Matrix4 result;
			Matrix4.Transformation(ref translation, ref scale, out result);
			return result;
		}

		public static void Transformation(ref Vector3 translation, ref Vector3 scale, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.M11 = scale.X;
			result.M22 = scale.Y;
			result.M33 = scale.Z;
			result.M41 = translation.X;
			result.M42 = translation.Y;
			result.M43 = translation.Z;
		}

		public static Matrix4 Translation(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.Translation(x, y, z, out result);
			return result;
		}

		public static void Translation(float x, float y, float z, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.M41 = x;
			result.M42 = y;
			result.M43 = z;
		}

		public static Matrix4 Translation(Vector3 translation)
		{
			Matrix4 result;
			Matrix4.Translation(translation.X, translation.Y, translation.Z, out result);
			return result;
		}

		public static void Translation(ref Vector3 translation, out Matrix4 result)
		{
			Matrix4.Translation(translation.X, translation.Y, translation.Z, out result);
		}

		public static Matrix4 RotationAxis(Vector3 axis, float angle)
		{
			Matrix4 result;
			Matrix4.RotationAxis(ref axis, angle, out result);
			return result;
		}

		public static void RotationAxis(ref Vector3 axis, float angle, out Matrix4 result)
		{
			Quaternion quaternion;
			Quaternion.RotationAxis(ref axis, angle, out quaternion);
			quaternion.ToMatrix4(out result);
		}

		public static Matrix4 RotationX(float angle)
		{
			Matrix4 result;
			Matrix4.RotationX(angle, out result);
			return result;
		}

		public static void RotationX(float angle, out Matrix4 result)
		{
			float num = (float)Math.Sin((double)angle);
			float num2 = (float)Math.Cos((double)angle);
			result = Matrix4.Identity;
			result.M22 = num2;
			result.M23 = num;
			result.M32 = -num;
			result.M33 = num2;
		}

		public static Matrix4 RotationY(float angle)
		{
			Matrix4 result;
			Matrix4.RotationY(angle, out result);
			return result;
		}

		public static void RotationY(float angle, out Matrix4 result)
		{
			float num = (float)Math.Sin((double)angle);
			float num2 = (float)Math.Cos((double)angle);
			result = Matrix4.Identity;
			result.M33 = num2;
			result.M31 = num;
			result.M13 = -num;
			result.M11 = num2;
		}

		public static Matrix4 RotationZ(float angle)
		{
			Matrix4 result;
			Matrix4.RotationZ(angle, out result);
			return result;
		}

		public static void RotationZ(float angle, out Matrix4 result)
		{
			float num = (float)Math.Sin((double)angle);
			float num2 = (float)Math.Cos((double)angle);
			result = Matrix4.Identity;
			result.M11 = num2;
			result.M12 = num;
			result.M21 = -num;
			result.M22 = num2;
		}

		public static Matrix4 RotationZyx(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.RotationZyx(x, y, z, out result);
			return result;
		}

		public static void RotationZyx(float x, float y, float z, out Matrix4 result)
		{
			Quaternion quaternion;
			Quaternion.RotationZyx(x, y, z, out quaternion);
			quaternion.ToMatrix4(out result);
		}

		public static Matrix4 RotationZyx(Vector3 angles)
		{
			Matrix4 result;
			Matrix4.RotationZyx(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationZyx(ref Vector3 angles, out Matrix4 result)
		{
			Matrix4.RotationZyx(angles.X, angles.Y, angles.Z, out result);
		}

		public static Matrix4 RotationYxz(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.RotationYxz(x, y, z, out result);
			return result;
		}

		public static void RotationYxz(float x, float y, float z, out Matrix4 result)
		{
			Quaternion quaternion;
			Quaternion.RotationYxz(x, y, z, out quaternion);
			quaternion.ToMatrix4(out result);
		}

		public static Matrix4 RotationYxz(Vector3 angles)
		{
			Matrix4 result;
			Matrix4.RotationYxz(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationYxz(ref Vector3 angles, out Matrix4 result)
		{
			Matrix4.RotationYxz(angles.X, angles.Y, angles.Z, out result);
		}

		public static Matrix4 RotationXzy(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.RotationXzy(x, y, z, out result);
			return result;
		}

		public static void RotationXzy(float x, float y, float z, out Matrix4 result)
		{
			Quaternion quaternion;
			Quaternion.RotationXzy(x, y, z, out quaternion);
			quaternion.ToMatrix4(out result);
		}

		public static Matrix4 RotationXzy(Vector3 angles)
		{
			Matrix4 result;
			Matrix4.RotationXzy(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationXzy(ref Vector3 angles, out Matrix4 result)
		{
			Matrix4.RotationXzy(angles.X, angles.Y, angles.Z, out result);
		}

		public static Matrix4 RotationXyz(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.RotationXyz(x, y, z, out result);
			return result;
		}

		public static void RotationXyz(float x, float y, float z, out Matrix4 result)
		{
			Quaternion quaternion;
			Quaternion.RotationXyz(x, y, z, out quaternion);
			quaternion.ToMatrix4(out result);
		}

		public static Matrix4 RotationXyz(Vector3 angles)
		{
			Matrix4 result;
			Matrix4.RotationXyz(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationXyz(ref Vector3 angles, out Matrix4 result)
		{
			Matrix4.RotationXyz(angles.X, angles.Y, angles.Z, out result);
		}

		public static Matrix4 RotationYzx(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.RotationYzx(x, y, z, out result);
			return result;
		}

		public static void RotationYzx(float x, float y, float z, out Matrix4 result)
		{
			Quaternion quaternion;
			Quaternion.RotationYzx(x, y, z, out quaternion);
			quaternion.ToMatrix4(out result);
		}

		public static Matrix4 RotationYzx(Vector3 angles)
		{
			Matrix4 result;
			Matrix4.RotationYzx(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationYzx(ref Vector3 angles, out Matrix4 result)
		{
			Matrix4.RotationYzx(angles.X, angles.Y, angles.Z, out result);
		}

		public static Matrix4 RotationZxy(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.RotationZxy(x, y, z, out result);
			return result;
		}

		public static void RotationZxy(float x, float y, float z, out Matrix4 result)
		{
			Quaternion quaternion;
			Quaternion.RotationZxy(x, y, z, out quaternion);
			quaternion.ToMatrix4(out result);
		}

		public static Matrix4 RotationZxy(Vector3 angles)
		{
			Matrix4 result;
			Matrix4.RotationZxy(angles.X, angles.Y, angles.Z, out result);
			return result;
		}

		public static void RotationZxy(ref Vector3 angles, out Matrix4 result)
		{
			Matrix4.RotationZxy(angles.X, angles.Y, angles.Z, out result);
		}

		public static Matrix4 Scale(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.Scale(x, y, z, out result);
			return result;
		}

		public static void Scale(float x, float y, float z, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.M11 = x;
			result.M22 = y;
			result.M33 = z;
		}

		public static Matrix4 Scale(Vector3 scale)
		{
			Matrix4 result;
			Matrix4.Scale(scale.X, scale.Y, scale.Z, out result);
			return result;
		}

		public static void Scale(ref Vector3 scale, out Matrix4 result)
		{
			Matrix4.Scale(scale.X, scale.Y, scale.Z, out result);
		}

		public static Matrix4 Perspective(float fovy, float aspect, float n, float f)
		{
			Matrix4 result;
			Matrix4.Perspective(fovy, aspect, n, f, out result);
			return result;
		}

		public static void Perspective(float fovy, float aspect, float n, float f, out Matrix4 result)
		{
			float num = n * (float)Math.Tan((double)(fovy * 0.5f));
			float b = -num;
			float num2 = num * aspect;
			float l = -num2;
			Matrix4.Frustum(l, num2, b, num, n, f, out result);
		}

		public static Matrix4 Frustum(float l, float r, float b, float t, float n, float f)
		{
			Matrix4 result;
			Matrix4.Frustum(l, r, b, t, n, f, out result);
			return result;
		}

		public static void Frustum(float l, float r, float b, float t, float n, float f, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.M11 = 2f * n / (r - l);
			result.M22 = 2f * n / (t - b);
			result.M31 = (r + l) / (r - l);
			result.M32 = (t + b) / (t - b);
			result.M33 = -(f + n) / (f - n);
			result.M34 = -1f;
			result.M43 = -2f * f * n / (f - n);
			result.M44 = 0f;
		}

		public static Matrix4 Ortho(float l, float r, float b, float t, float n, float f)
		{
			Matrix4 result;
			Matrix4.Ortho(l, r, b, t, n, f, out result);
			return result;
		}

		public static void Ortho(float l, float r, float b, float t, float n, float f, out Matrix4 result)
		{
			float num = 1f / (r - l);
			float num2 = 1f / (t - b);
			float num3 = 1f / (n - f);
			result = Matrix4.Identity;
			result.M11 = 2f * num;
			result.M22 = 2f * num2;
			result.M33 = 2f * num3;
			result.M41 = -(r + l) * num;
			result.M42 = -(t + b) * num2;
			result.M43 = (f + n) * num3;
		}

		public static Matrix4 LookAt(Vector3 eye, Vector3 center, Vector3 up)
		{
			Matrix4 result;
			Matrix4.LookAt(ref eye, ref center, ref up, out result);
			return result;
		}

		public static void LookAt(ref Vector3 eye, ref Vector3 center, ref Vector3 up, out Matrix4 result)
		{
			Vector3 vector;
			eye.Subtract(ref center, out vector);
			vector.Normalize(out vector);
			Vector3 vector2;
			up.Cross(ref vector, out vector2);
			vector2.Normalize(out vector2);
			Vector3 vector3;
			vector.Cross(ref vector2, out vector3);
			result.M11 = vector2.X;
			result.M12 = vector3.X;
			result.M13 = vector.X;
			result.M14 = 0f;
			result.M21 = vector2.Y;
			result.M22 = vector3.Y;
			result.M23 = vector.Y;
			result.M24 = 0f;
			result.M31 = vector2.Z;
			result.M32 = vector3.Z;
			result.M33 = vector.Z;
			result.M34 = 0f;
			result.M41 = -eye.Dot(ref vector2);
			result.M42 = -eye.Dot(ref vector3);
			result.M43 = -eye.Dot(ref vector);
			result.M44 = 1f;
		}

		public static float Determinant(Matrix4 m)
		{
			return m.Determinant();
		}

		public static float Determinant(ref Matrix4 m)
		{
			return m.Determinant();
		}

		public static Matrix4 Transpose(Matrix4 m)
		{
			Matrix4 result;
			m.Transpose(out result);
			return result;
		}

		public static void Transpose(ref Matrix4 m, out Matrix4 result)
		{
			m.Transpose(out result);
		}

		public static Matrix4 Inverse(Matrix4 m)
		{
			Matrix4 result;
			m.Inverse(out result);
			return result;
		}

		public static void Inverse(ref Matrix4 m, out Matrix4 result)
		{
			m.Inverse(out result);
		}

		public static Matrix4 InverseAffine(Matrix4 m)
		{
			Matrix4 result;
			m.InverseAffine(out result);
			return result;
		}

		public static void InverseAffine(ref Matrix4 m, out Matrix4 result)
		{
			m.InverseAffine(out result);
		}

		public static Matrix4 InverseOrthonormal(Matrix4 m)
		{
			Matrix4 result;
			m.InverseOrthonormal(out result);
			return result;
		}

		public static void InverseOrthonormal(ref Matrix4 m, out Matrix4 result)
		{
			m.InverseOrthonormal(out result);
		}

		public static Matrix4 Orthonormalize(Matrix4 m)
		{
			Matrix4 result;
			m.Orthonormalize(out result);
			return result;
		}

		public static void Orthonormalize(ref Matrix4 m, out Matrix4 result)
		{
			m.Orthonormalize(out result);
		}

		public static Vector4 Transform(Matrix4 m, Vector4 v)
		{
			Vector4 result;
			m.Transform(ref v, out result);
			return result;
		}

		public static void Transform(ref Matrix4 m, ref Vector4 v, out Vector4 result)
		{
			m.Transform(ref v, out result);
		}

		public static Vector3 TransformPoint(Matrix4 m, Vector3 v)
		{
			Vector3 result;
			m.TransformPoint(ref v, out result);
			return result;
		}

		public static void TransformPoint(ref Matrix4 m, ref Vector3 v, out Vector3 result)
		{
			m.TransformPoint(ref v, out result);
		}

		public static Vector2 TransformPoint(Matrix4 m, Vector2 v)
		{
			Vector2 result;
			m.TransformPoint(ref v, out result);
			return result;
		}

		public static void TransformPoint(ref Matrix4 m, ref Vector2 v, out Vector2 result)
		{
			m.TransformPoint(ref v, out result);
		}

		public static Vector3 TransformVector(Matrix4 m, Vector3 v)
		{
			Vector3 result;
			m.TransformVector(ref v, out result);
			return result;
		}

		public static void TransformVector(ref Matrix4 m, ref Vector3 v, out Vector3 result)
		{
			m.TransformVector(ref v, out result);
		}

		public static Vector2 TransformVector(Matrix4 m, Vector2 v)
		{
			Vector2 result;
			m.TransformVector(ref v, out result);
			return result;
		}

		public static void TransformVector(ref Matrix4 m, ref Vector2 v, out Vector2 result)
		{
			m.TransformVector(ref v, out result);
		}

		public static Vector4 TransformProjection(Matrix4 m, Vector4 v)
		{
			Vector4 result;
			m.TransformProjection(ref v, out result);
			return result;
		}

		public static void TransformProjection(ref Matrix4 m, ref Vector4 v, out Vector4 result)
		{
			m.TransformProjection(ref v, out result);
		}

		public static Vector3 TransformProjection(Matrix4 m, Vector3 v)
		{
			Vector3 result;
			m.TransformProjection(ref v, out result);
			return result;
		}

		public static void TransformProjection(ref Matrix4 m, ref Vector3 v, out Vector3 result)
		{
			m.TransformProjection(ref v, out result);
		}

		public static Vector2 TransformProjection(Matrix4 m, Vector2 v)
		{
			Vector2 result;
			m.TransformProjection(ref v, out result);
			return result;
		}

		public static void TransformProjection(ref Matrix4 m, ref Vector2 v, out Vector2 result)
		{
			m.TransformProjection(ref v, out result);
		}

		public static Matrix4 Add(Matrix4 m1, Matrix4 m2)
		{
			Matrix4 result;
			m1.Add(ref m2, out result);
			return result;
		}

		public static void Add(ref Matrix4 m1, ref Matrix4 m2, out Matrix4 result)
		{
			m1.Add(ref m2, out result);
		}

		public static Matrix4 Subtract(Matrix4 m1, Matrix4 m2)
		{
			Matrix4 result;
			m1.Subtract(ref m2, out result);
			return result;
		}

		public static void Subtract(ref Matrix4 m1, ref Matrix4 m2, out Matrix4 result)
		{
			m1.Subtract(ref m2, out result);
		}

		public static Matrix4 Multiply(Matrix4 m1, Matrix4 m2)
		{
			Matrix4 result;
			m1.Multiply(ref m2, out result);
			return result;
		}

		public static void Multiply(ref Matrix4 m1, ref Matrix4 m2, out Matrix4 result)
		{
			m1.Multiply(ref m2, out result);
		}

		public static Matrix4 MultiplyAffine(Matrix4 m1, Matrix4 m2)
		{
			Matrix4 result;
			m1.MultiplyAffine(ref m2, out result);
			return result;
		}

		public static void MultiplyAffine(ref Matrix4 m1, ref Matrix4 m2, out Matrix4 result)
		{
			m1.MultiplyAffine(ref m2, out result);
		}

		public static Matrix4 Multiply(Matrix4 m, float f)
		{
			Matrix4 result;
			m.Multiply(f, out result);
			return result;
		}

		public static void Multiply(ref Matrix4 m, float f, out Matrix4 result)
		{
			m.Multiply(f, out result);
		}

		public static Matrix4 Divide(Matrix4 m, float f)
		{
			Matrix4 result;
			m.Divide(f, out result);
			return result;
		}

		public static void Divide(ref Matrix4 m, float f, out Matrix4 result)
		{
			m.Divide(f, out result);
		}

		public static Matrix4 Negate(Matrix4 m)
		{
			Matrix4 result;
			m.Negate(out result);
			return result;
		}

		public static void Negate(ref Matrix4 m, out Matrix4 result)
		{
			m.Negate(out result);
		}

		public static bool operator ==(Matrix4 m1, Matrix4 m2)
		{
			return m1.Equals(m2);
		}

		public static bool operator !=(Matrix4 m1, Matrix4 m2)
		{
			return !m1.Equals(m2);
		}

		public static Matrix4 operator +(Matrix4 m1, Matrix4 m2)
		{
			Matrix4 result;
			m1.Add(ref m2, out result);
			return result;
		}

		public static Matrix4 operator -(Matrix4 m1, Matrix4 m2)
		{
			Matrix4 result;
			m1.Subtract(ref m2, out result);
			return result;
		}

		public static Matrix4 operator -(Matrix4 m)
		{
			Matrix4 result;
			m.Negate(out result);
			return result;
		}

		public static Matrix4 operator *(Matrix4 m1, Matrix4 m2)
		{
			Matrix4 result;
			m1.Multiply(ref m2, out result);
			return result;
		}

		public static Matrix4 operator *(Matrix4 m, float f)
		{
			Matrix4 result;
			m.Multiply(f, out result);
			return result;
		}

		public static Matrix4 operator *(float f, Matrix4 m)
		{
			Matrix4 result;
			m.Multiply(f, out result);
			return result;
		}

		public static Vector4 operator *(Matrix4 m, Vector4 v)
		{
			Vector4 result;
			m.Transform(ref v, out result);
			return result;
		}

		public static Matrix4 operator /(Matrix4 m, float f)
		{
			Matrix4 result;
			m.Divide(f, out result);
			return result;
		}

		[Obsolete("Use TransformPoint")]
		public Vector3 Transform(Vector3 v)
		{
			return this.TransformPoint(v);
		}

		[Obsolete("Use TransformPoint")]
		public void Transform(ref Vector3 v, out Vector3 result)
		{
			this.TransformPoint(ref v, out result);
		}

		[Obsolete("Use TransformPoint")]
		public Vector2 Transform(Vector2 v)
		{
			return this.TransformPoint(v);
		}

		[Obsolete("Use TransformPoint")]
		public void Transform(ref Vector2 v, out Vector2 result)
		{
			this.TransformPoint(ref v, out result);
		}

		[Obsolete("Use TransformPoint")]
		public static Vector3 Transform(Matrix4 m, Vector3 v)
		{
			return Matrix4.TransformPoint(m, v);
		}

		[Obsolete("Use TransformPoint")]
		public static void Transform(ref Matrix4 m, ref Vector3 v, out Vector3 result)
		{
			Matrix4.TransformPoint(ref m, ref v, out result);
		}

		[Obsolete("Use TransformPoint")]
		public static Vector2 Transform(Matrix4 m, Vector2 v)
		{
			return Matrix4.TransformPoint(m, v);
		}

		[Obsolete("Use TransformPoint")]
		public static void Transform(ref Matrix4 m, ref Vector2 v, out Vector2 result)
		{
			Matrix4.TransformPoint(ref m, ref v, out result);
		}

		[Obsolete("Use TransformPoint")]
		public static Vector3 operator *(Matrix4 m, Vector3 v)
		{
			return m.TransformPoint(v);
		}

		[Obsolete("Use TransformPoint")]
		public static Vector2 operator *(Matrix4 m, Vector2 v)
		{
			return m.TransformPoint(v);
		}
	}
}
