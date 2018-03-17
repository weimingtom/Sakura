using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct Matrix3
	{
		public Vector3 X;

		public Vector3 Y;

		public Vector3 Z;

		public static Matrix3 Identity = new Matrix3(Math._100, Math._010, Math._001);

		public static Matrix3 Zero = new Matrix3(Math._000, Math._000, Math._000);

		private static uint XNotUnitLen = 2u;

		private static uint YNotUnitLen = 4u;

		private static uint XYNotPerpendicular = 8u;

		private static uint LastRowNot001 = 16u;

		private static uint coord_sys_error = 0u;

		public Matrix3(Vector3 valx, Vector3 valy, Vector3 valz)
		{
			this.X = valx;
			this.Y = valy;
			this.Z = valz;
		}

		public static Vector3 operator *(Matrix3 m, Vector3 v)
		{
			return v.X * m.X + v.Y * m.Y + v.Z * m.Z;
		}

		public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
		{
			return new Matrix3(m1 * m2.X, m1 * m2.Y, m1 * m2.Z);
		}

		public Matrix3 Transpose()
		{
			return new Matrix3(new Vector3(this.X.X, this.Y.X, this.Z.X), new Vector3(this.X.Y, this.Y.Y, this.Z.Y), new Vector3(this.X.Z, this.Y.Z, this.Z.Z));
		}

		public static Matrix3 Translation(Vector2 value)
		{
			return new Matrix3(Math._100, Math._010, value.Xy1);
		}

		public static Matrix3 Scale(Vector2 value)
		{
			return new Matrix3(Math._100 * value.X, Math._010 * value.Y, Math._001);
		}

		public static Matrix3 Skew(Vector2 value)
		{
			return new Matrix3(new Vector3(1f, value.X, 0f), new Vector3(value.Y, 1f, 0f), Math._001);
		}

		public static Matrix3 Rotation(Vector2 unit_vector)
		{
			return new Matrix3(unit_vector.Xy0, Math.Perp(unit_vector).Xy0, Math._001);
		}

		public static Matrix3 Rotation(float angle)
		{
			return Matrix3.Rotation(Vector2.Rotation(angle));
		}

		public static Matrix3 TRS(Vector2 translation, Vector2 unit_vector, Vector2 scale)
		{
			Matrix3 result = Matrix3.Rotation(unit_vector);
			result.Z = translation.Xy1;
			result.X *= scale.X;
			result.Y *= scale.Y;
			return result;
		}

		public float Determinant()
		{
			return this.X.Dot(this.Y.Cross(this.Z));
		}

		public Matrix3 Inverse()
		{
			float num = this.Determinant();
			Matrix3 result;
			if (num == 0f)
			{
				result = Matrix3.Identity;
			}
			else
			{
				num = 1f / num;
				result = new Matrix3(new Vector3((this.Y.Y * this.Z.Z - this.Z.Y * this.Y.Z) * num, -(this.X.Y * this.Z.Z - this.Z.Y * this.X.Z) * num, (this.X.Y * this.Y.Z - this.Y.Y * this.X.Z) * num), new Vector3(-(this.Y.X * this.Z.Z - this.Z.X * this.Y.Z) * num, (this.X.X * this.Z.Z - this.Z.X * this.X.Z) * num, -(this.X.X * this.Y.Z - this.Y.X * this.X.Z) * num), new Vector3((this.Y.X * this.Z.Y - this.Z.X * this.Y.Y) * num, -(this.X.X * this.Z.Y - this.Z.X * this.X.Y) * num, (this.X.X * this.Y.Y - this.Y.X * this.X.Y) * num));
			}
			return result;
		}

		public Matrix3 InverseOrthonormal()
		{
			return new Matrix3(new Vector3(this.X.X, this.Y.X, 0f), new Vector3(this.X.Y, this.Y.Y, 0f), Math._001) * Matrix3.Translation(-this.Z.Xy);
		}

		public Matrix4 Matrix4()
		{
			return new Matrix4(this.X.Xyz0, this.Y.Xyz0, Math._0010, new Vector4(this.Z.Xy, 0f, 1f));
		}

		public string GetCoordSysError()
		{
			string text = "";
			if (0u != (Matrix3.coord_sys_error & Matrix3.XNotUnitLen))
			{
				text += "XNotUnitLen";
			}
			if (0u != (Matrix3.coord_sys_error & Matrix3.YNotUnitLen))
			{
				text += "YNotUnitLen";
			}
			if (0u != (Matrix3.coord_sys_error & Matrix3.XYNotPerpendicular))
			{
				text += "XYNotPerpendicular";
			}
			if (0u != (Matrix3.coord_sys_error & Matrix3.LastRowNot001))
			{
				text += "LastRowNot001";
			}
			return text;
		}

		public bool IsOrthonormal(float epsilon)
		{
			Matrix3.coord_sys_error = 0u;
			bool result;
			if (this.Z.Z != 1f || this.X.Z != 0f || this.Y.Z != 0f)
			{
				Matrix3.coord_sys_error |= Matrix3.LastRowNot001;
				result = false;
			}
			else if (FMath.Abs(this.X.Length() - 1f) >= epsilon)
			{
				Matrix3.coord_sys_error |= Matrix3.XNotUnitLen;
				result = false;
			}
			else if (FMath.Abs(this.Y.Length() - 1f) >= epsilon)
			{
				Matrix3.coord_sys_error |= Matrix3.YNotUnitLen;
				result = false;
			}
			else if (FMath.Abs(this.X.Dot(this.Y)) >= epsilon)
			{
				Matrix3.coord_sys_error |= Matrix3.XYNotPerpendicular;
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public bool Equals(ref Matrix3 m, float epsilon)
		{
			return this.X.Equals(m.X, epsilon) && this.Y.Equals(m.Y, epsilon) && this.Z.Equals(m.Z, epsilon);
		}

		public bool Equals(ref Matrix3 m)
		{
			return this.Equals(ref m, 1E-06f);
		}

		public override string ToString()
		{
			return string.Format("({0},{1},{2})", this.X, this.Y, this.Z);
		}
	}
}
