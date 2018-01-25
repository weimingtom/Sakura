using System;

namespace Sce.Pss.Core
{
	public struct Vector4 : IEquatable<Vector4>
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public static readonly Vector4 Zero = new Vector4(0f, 0f, 0f, 0f);
		public static readonly Vector4 One = new Vector4(1f, 1f, 1f, 1f);
		public static readonly Vector4 UnitX = new Vector4(1f, 0f, 0f, 0f);
		public static readonly Vector4 UnitY = new Vector4(0f, 1f, 0f, 0f);
		public static readonly Vector4 UnitZ = new Vector4(0f, 0f, 1f, 0f);
		public static readonly Vector4 UnitW = new Vector4(0f, 0f, 0f, 1f);
		
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
		
		public Vector4(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}
		
		public Vector4(Vector3 xyz, float w)
		{
			this.X = xyz.X;
			this.Y = xyz.Y;
			this.Z = xyz.Z;
			this.W = w;
		}
		
		public bool Equals(Vector4 v)
		{
			return this.X == v.X && this.Y == v.Y && this.Z == v.Z && this.W == v.W;
		}		
	}
}
