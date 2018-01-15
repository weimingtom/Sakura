using System;

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
		
		public Vector3 (float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;			
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
	}
}
