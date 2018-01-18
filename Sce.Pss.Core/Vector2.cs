using System;

namespace Sce.Pss.Core
{
	public struct Vector2
	{
		public float X;
		public float Y;

		public static readonly Vector2 Zero = new Vector2(0f, 0f);
		public static readonly Vector2 One = new Vector2(1f, 1f);
		public static readonly Vector2 UnitX = new Vector2(1f, 0f);
		public static readonly Vector2 UnitY = new Vector2(0f, 1f);

		public override string ToString()
		{
			return string.Format("({0:F6},{1:F6})", this.X, this.Y);
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
		
	}
}