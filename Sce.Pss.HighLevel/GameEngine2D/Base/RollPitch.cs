using Sce.Pss.Core;
using System;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public class RollPitch
	{
		public Vector2 Data;

		public static Vector2 FromVector(Vector3 v)
		{
			float num = -Math.Angle(v.Zy);
			v = v.RotateX(-num);
			Common.Assert(FMath.Abs(v.Y) < 0.001f);
			float num2 = Math.Angle(v.Zx);
			return new Vector2(num, num2);
		}

		public static Vector3 ToVector(Vector2 a)
		{
			return Math._001.RotateY(a.Y).RotateX(a.X);
		}

		public RollPitch()
		{
			this.Data = Math._00;
		}

		public RollPitch(Vector2 v)
		{
			this.Data = v;
		}

		public RollPitch(Vector3 v)
		{
			this.Data = RollPitch.FromVector(v);
		}

		public Vector3 ToVector()
		{
			return RollPitch.ToVector(this.Data);
		}

		public Matrix4 ToMatrix()
		{
			Vector3 vector = this.ToVector();
			Vector2 data = this.Data;
			data.X -= Math.Pi * 0.5f;
			Vector3 vector2 = new RollPitch(data).ToVector().Cross(vector).Normalize();
			Vector3 vector3 = vector.Cross(vector2);
			Matrix4 matrix = default(Matrix4);
			matrix.ColumnX = vector2.Xyz0;
			matrix.ColumnY = vector3.Xyz0;
			matrix.ColumnZ = vector.Xyz0;
			matrix.ColumnW = Math._0001;
			return matrix.InverseOrthonormal();
		}
	}
}
