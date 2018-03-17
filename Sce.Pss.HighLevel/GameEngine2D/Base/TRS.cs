using Sce.Pss.Core;
using System;
using System.Diagnostics;

namespace Sce.Pss.HighLevel.GameEngine2D.Base
{
	public struct TRS
	{
		public static class Local
		{
			public static Vector2 TopLeft = new Vector2(0f, 1f);

			public static Vector2 MiddleLeft = new Vector2(0f, 0.5f);

			public static Vector2 BottomLeft = new Vector2(0f, 0f);

			public static Vector2 TopCenter = new Vector2(0.5f, 1f);

			public static Vector2 Center = new Vector2(0.5f, 0.5f);

			public static Vector2 BottomCenter = new Vector2(0.5f, 0f);

			public static Vector2 TopRight = new Vector2(1f, 1f);

			public static Vector2 MiddleRight = new Vector2(1f, 0.5f);

			public static Vector2 BottomRight = new Vector2(1f, 0f);
		}

		public Vector2 T;

		public Vector2 R;

		private Vector2 _S;
		public Vector2 S
		{
			get
			{
				return _S;
			}
			set
			{
				_S = value;
			}
		}

		public static TRS Quad0_1 = new TRS
		{
			T = Math._00,
			R = Math._10,
			S = Math._11
		};

		public static TRS QuadMinus1_1 = new TRS
		{
			T = -Math._11,
			R = Math._10,
			S = Math._11 * 2f
		};

		public Vector2 X
		{
			get
			{
//				Debug.WriteLine("[X] this.R == " + this.R + ", this.S.X == " + this.S.X);
				return this.R * this.S.X;
			}
		}

		public Vector2 Y
		{
			get
			{
//				Debug.WriteLine("[Y] Math.Perp(this.R) == " + Math.Perp(this.R) + ", this.S.Y == " + this.S.Y);
				return Math.Perp(this.R) * this.S.Y;
			}
		}

		public Vector2 Point00
		{
			get
			{
				return this.T;
			}
		}

		public Vector2 Point10
		{
			get
			{
				return this.T + this.X;
			}
		}

		public Vector2 Point01
		{
			get
			{
				return this.T + this.Y;
			}
		}

		public Vector2 Point11
		{
			get
			{
				return this.T + this.X + this.Y;
			}
		}

		public Vector2 Center
		{
			get
			{
				return this.T + (this.X + this.Y) * 0.5f;
			}
		}

		public Vector2 RotationNormalize
		{
			get
			{
				return this.R;
			}
			set
			{
				this.R = value.Normalize();
			}
		}

		public float Angle
		{
			get
			{
				return Math.Angle(this.R);
			}
			set
			{
				this.R = Vector2.Rotation(value);
			}
		}

		public void Rotate(float angle)
		{
			this.R = this.R.Rotate(angle);
		}

		public void Rotate(Vector2 rotation)
		{
			this.R = this.R.Rotate(rotation);
		}

		public TRS(Bounds2 a_bounds)
		{
			this.T = a_bounds.Min;
			this.R = Math._10;
			this._S = a_bounds.Size;
		}

		public Bounds2 Bounds2()
		{
			Bounds2 result = new Bounds2(this.Point00, this.Point00);
			result.Add(this.Point10);
			result.Add(this.Point01);
			result.Add(this.Point11);
			return result;
		}

		public static TRS Tile(Vector2i num_tiles, Vector2i tile_index, TRS source_area)
		{
			Vector2 vector = num_tiles.Vector2();
			Vector2 vector2 = tile_index.Vector2();
			Vector2 vector3 = source_area.S / vector;
			Vector2 x = source_area.X;
			Vector2 y = source_area.Y;
			return new TRS
			{
				T = source_area.T + vector2 * vector3,
				R = source_area.R,
				S = vector3
			};
		}

		public void Centering(Vector2 normalized_pos)
		{
			this.T = -this.X * normalized_pos.X - this.Y * normalized_pos.Y;
		}

		public override string ToString()
		{
			string result;
			if (this.R.X == 0f && this.R.Y == 0f)
			{
				result = string.Format("Invalid TRS (R lenght is zero)", new object[0]);
			}
			else
			{
				result = string.Format("(T={0},R={1}={2} degrees,S={3})", new object[]
				{
					this.T,
					this.R,
					Math.Rad2Deg(this.Angle),
					this.S
				});
			}
			return result;
		}
	}
}
