using System;

namespace Sce.Pss.Core
{
	public struct Rgba : IEquatable<Rgba>
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public Rgba(int r, int g, int b, int a)
		{
			this.R = Rgba.ToByteN(r);
			this.G = Rgba.ToByteN(g);
			this.B = Rgba.ToByteN(b);
			this.A = Rgba.ToByteN(a);
		}

		public Rgba(Vector4 v)
		{
			this.R = Rgba.ToByteN(v.X);
			this.G = Rgba.ToByteN(v.Y);
			this.B = Rgba.ToByteN(v.Z);
			this.A = Rgba.ToByteN(v.W);
		}

		public Vector4 ToVector4()
		{
			float num = 0.003921569f;
			return new Vector4((float)this.R * num, (float)this.G * num, (float)this.B * num, (float)this.A * num);
		}

		public bool Equals(Rgba c)
		{
			return ((this.R ^ c.R) | (this.G ^ c.G) | (this.B ^ c.B) | (this.A ^ c.A)) == 0;
		}

		public override bool Equals(object o)
		{
			return o is Rgba && this.Equals((Rgba)o);
		}

		public override string ToString()
		{
			return string.Format("({0},{1},{2},{3})", new object[]
			{
				this.R,
				this.G,
				this.B,
				this.A
			});
		}

		public override int GetHashCode()
		{
			return (int)this.R | (int)this.G << 8 | (int)this.B << 16 | (int)this.A << 24;
		}

		public static bool operator ==(Rgba c1, Rgba c2)
		{
			return c1.Equals(c2);
		}

		public static bool operator !=(Rgba c1, Rgba c2)
		{
			return !c1.Equals(c2);
		}

		private static byte ToByteN(float f)
		{
			return Rgba.ToByteN((int)(f * 255f + 0.5f));
		}

		private static byte ToByteN(int i)
		{
			return (byte)((i < 0) ? 0 : ((i > 255) ? 255 : i));
		}
	}
}
