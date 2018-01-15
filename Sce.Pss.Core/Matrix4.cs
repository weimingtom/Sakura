using System;

namespace Sce.Pss.Core
{
	public struct Matrix4
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
		
		public static readonly Matrix4 Identity = 
			new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

		public Matrix4(float m11, float m12, float m13, float m14, 
		               float m21, float m22, float m23, float m24, 
		               float m31, float m32, float m33, float m34, 
		               float m41, float m42, float m43, float m44)
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
		
		public static Matrix4 Perspective (float fovy, float aspect, float n, float f)
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
	}
}
